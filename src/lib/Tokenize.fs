module Tokenize

open Model
open System

// See Rules for Regular Identifiers: https://docs.microsoft.com/en-us/previous-versions/sql/sql-server-2008-r2/ms175874(v=sql.105)


let tokenize (sqlStr: string) =
  // Add characters to the token until there is a separator.
  // Each separtor is a separate token.
  // A single operator as <> is divided into two tokens.
  // A word like "declare" is always one token.
  let mutable tokens = []
  let mutable lastTokenEndPos = -1
  let sqlStrLen = sqlStr.Length
  for pos in [0..sqlStrLen-1] do
    let c = sqlStr.[pos]
    if (c = ' ' // Whitespace
    || c = '\t' // Whitespace
    || c = '\n' // Whitespace
    || c = '\r' // Whitespace
    || c = '(' // Grouping
    || c = ')' // Grouping
    || c = '"' // String delimiter
    || c = ''' // String delimiter
    || c = '[' // Operator Wildcard - Character(s) to Match
    || c = ']' // Operator Wildcard - Character(s) to Match
    || c = '@' // Variable prefix
    || c = '+' // Operator
    || c = '-' // Operator
    || c = '*' // Operator
    || c = '/' // Operator
    || c = '%' // Operator
    || c = '=' // Operator
    || c = '&' // Operator
    || c = '|' // Operator
    || c = '^' // Operator
    || c = '~' // Operator
    || c = '>' // Operator
    || c = '<' // Operator
    || c = '!' // Operator as !=
    || c = '.' // Member separator
    || c = ':' // Scope resolution as ::
    || c = ';' // Delimitor
    || c = ',' // Separator
    ) then
      // Current char is a separator/operator so complete pevious token.
      let newToken = sqlStr.[lastTokenEndPos + 1 .. pos - 1]
      if (newToken <> "") then
        tokens <- newToken::tokens
      // Also create token for current separator/operator character.
      let newToken = sqlStr.[pos..pos]
      tokens <- newToken::tokens
      lastTokenEndPos <- pos
    if (pos = sqlStrLen-1) then
      // At the end push remaining chars as token.
      let newToken = sqlStr.[lastTokenEndPos + 1 .. pos]
      if (newToken <> "") then
        tokens <- newToken::tokens
  List.rev tokens

type StringToken =
| NoString of string // no sql string token.
| UnicodePrefix of string // The N before a stirng.
| StartOfString of string // sql string start token.
| InString of string // sql string text token.
| InStringDoubleQuoteStart of string // sql string text token.
| InStringDoubleQuoteEnd of string // sql string text token.
| EndOfString of string // sql string end token.

// todo: test if N can be lower case and if space as in N 'some text' is allowed.

// Converts list of string in list of StringToken.
let rec convertToStringTokens (tokens: list<string>) (lastResult: StringToken) (result: list<StringToken>) =
  match tokens with
  | token :: tail ->
    let nextToken = List.tryHead tail
    let converted = 
      match lastResult with
      | StartOfString _ | InString _ | InStringDoubleQuoteEnd _ when (token = "'" && nextToken = Some("'")) ->
        InStringDoubleQuoteStart token
      | InStringDoubleQuoteStart _ when (token = "'") ->
        InStringDoubleQuoteEnd token
      | StartOfString _ | InString _ | InStringDoubleQuoteEnd _ when (token <> "'") ->
        InString token
      | InString _ | InStringDoubleQuoteEnd _ when (token = "'") ->
        EndOfString token
      | NoString _ | UnicodePrefix _ when (token = "'") ->
        StartOfString token
      | NoString _ when (token = "N" && nextToken = Some("'")) ->
        UnicodePrefix token
      | EndOfString _ | NoString _ when (token <> "'") ->
        NoString token
      | _ ->
        failwith token
    convertToStringTokens tail converted (converted::result)
  | [] -> List.rev result

// Combine tokens that together are one string.
// This is useful because a string may contain a keyword as in 'Make an update.'
let rec combineStringTokens (tokens: list<string>) =
  let markedTokens = convertToStringTokens tokens (NoString "") []
  
  let rec doCombine (tokens: list<StringToken>) (currentStringTokens: list<string>) (newTokens: list<string>) =
    match tokens with
    | head::tail ->
      let (currentStringParts, newTokensResult) = 
        match head with
        | NoString value -> [], (value::newTokens)
        // currentStringTokens will contain "N" if previous token was UnicodePrefix.
        | StartOfString value | UnicodePrefix value when currentStringTokens = [] -> [value], newTokens
        | StartOfString value | InString value | InStringDoubleQuoteStart value | InStringDoubleQuoteEnd value -> 
          (value::currentStringTokens), newTokens
        | EndOfString value -> 
          let tokensOfString = List.rev (value::currentStringTokens)
          let strWithQuotes = String.Join("", tokensOfString)
          [], (strWithQuotes::newTokens)
      doCombine tail currentStringParts newTokensResult
    | [] -> newTokens

  List.rev (doCombine markedTokens [] [])

type Simple4StateToken =
| OutsideToken of string
| InToken of string
| StartOfToken of string
| EndOfToken of string // For line comment tokens, the last character in the line before \n.

// Must first combine multiline tokens because then there is just one token for multiline.
// Converts list of string in list of CommentToken.
let rec convertToSimple4StateTokens (tokens: list<string>) (lastResult: Simple4StateToken) (result: list<Simple4StateToken>) 
  (fnIsTokenStart: string * Option<string> -> bool) (fnIsTokenEnd: string * Option<string> -> bool) =
  match tokens with
  | token :: tail ->
    let nextToken = List.tryHead tail
    let converted = 
      match lastResult with
      | OutsideToken _ | EndOfToken _ when fnIsTokenStart(token, nextToken) -> // token = startOfTokenChar1 && (startOfTokenChar2 = None || nextToken = startOfTokenChar2) ->
        StartOfToken token
      | OutsideToken _ | EndOfToken _ ->
        OutsideToken token
      | InToken _ when fnIsTokenEnd(token, nextToken) -> //nextToken = Some(endOfTokenChar) || nextToken = None ->
        EndOfToken token
      | InToken _ | StartOfToken _ ->
        InToken token
      | _ ->
        failwith token
    convertToSimple4StateTokens tail converted (converted::result) fnIsTokenStart fnIsTokenEnd
  | [] ->
    List.rev result

// Combine tokens that together are one comment.
// This is useful because a comment may contain a keyword as in -- Make an update.
let rec combineSimple4StateTokens (tokens: list<string>) 
  (fnIsTokenStart: string * Option<string> -> bool) (fnIsTokenEnd: string * Option<string> -> bool) =
  let markedTokens = convertToSimple4StateTokens tokens (OutsideToken "") [] fnIsTokenStart fnIsTokenEnd
  
  let rec doCombine (tokens: list<Simple4StateToken>) (currentCommentTokens: list<string>) (newTokens: list<string>) =
    match tokens with
    | head::tail ->
      let (currentCommentParts, newTokensResult) = 
        match head with
        | OutsideToken value -> [], (value::newTokens)
        | StartOfToken value -> [value], newTokens
        | InToken value -> 
          (value::currentCommentTokens), newTokens
        | EndOfToken value -> 
          let tokensOfComment = List.rev (value::currentCommentTokens)
          let strWithComments = String.Join("", tokensOfComment)
          [], (strWithComments::newTokens)
      doCombine tail currentCommentParts newTokensResult
    | [] -> newTokens

  List.rev (doCombine markedTokens [] [])

let rec combineSLCommentTokens (tokens: list<string>) =
  combineSimple4StateTokens tokens (fun (token, nextToken) -> token = "-" && nextToken = Some("-")) (fun (_, nextToken) -> nextToken = Some("\n") || nextToken = None)

type MultiLineCommentToken =
| NoMLComment of string * int // int refers to the nesting level starting at 0. For this case always 0.
| InMLComment of string * int // int refers to the nesting level starting at 0.
| StartOfMLComment of string * int // Should always be '/'. int refers to the nesting level starting at 0.
| EndOfMLComment of string * int // Should always be '/'. int refers to the nesting level starting at 0.

// Converts list of string in list of MultiLineCommentToken.
let rec convertToMLCommentTokens (tokens: list<string>) (lastResult: MultiLineCommentToken) (result: list<MultiLineCommentToken>) =
  match tokens with
  | token :: tail ->
    let nextToken = List.tryHead tail
    let converted = 
      match lastResult with
      | InMLComment (_, nestingLevel) when token = "/" && nextToken = Some("*") ->
        StartOfMLComment (token, nestingLevel + 1)
      | NoMLComment (_, nestingLevel) | EndOfMLComment (_, nestingLevel) when token = "/" && nextToken = Some("*") ->
        StartOfMLComment (token, nestingLevel)
      | EndOfMLComment (_, nestingLevel) when nestingLevel > 0 ->
        InMLComment (token, nestingLevel - 1)
      | NoMLComment _ | EndOfMLComment _ ->
        NoMLComment (token, 0)
      | InMLComment (previousToken, nestingLevel) when previousToken = "*" && token = "/" ->
        EndOfMLComment (token, nestingLevel)
      | InMLComment (_, nestingLevel) | StartOfMLComment (_, nestingLevel) ->
        InMLComment (token, nestingLevel)
      | _ ->
        failwith token
    convertToMLCommentTokens tail converted (converted::result)
  | [] ->
    List.rev result

// Combine tokens that together are one multiline comment.
let rec combineMLCommentTokens (tokens: list<string>) =
  let markedTokens = convertToMLCommentTokens tokens (NoMLComment ("", 0)) []
  
  let rec doCombine (tokens: list<MultiLineCommentToken>) (currentCommentTokens: list<string>) (newTokens: list<string>) =
    match tokens with
    | head::tail ->
      let (currentCommentParts, newTokensResult) = 
        match head with
        | NoMLComment (value, _) -> [], (value::newTokens)
        | StartOfMLComment (value, nestingLevel) when nestingLevel = 0 -> [value], newTokens
        | EndOfMLComment (value, nestingLevel) when nestingLevel = 0 -> 
          let tokensOfComment = List.rev (value::currentCommentTokens)
          let strWithComments = String.Join("", tokensOfComment)
          [], (strWithComments::newTokens)
        | InMLComment (value, _) | StartOfMLComment (value, _) | EndOfMLComment (value, _) -> 
          (value::currentCommentTokens), newTokens
      doCombine tail currentCommentParts newTokensResult
    | [] -> newTokens

  List.rev (doCombine markedTokens [] [])

let rec combineBracketTokens (tokens: list<string>) =
  combineSimple4StateTokens tokens (fun (token, _) -> token = "[") (fun (token, _) -> token = "]")

let rec combineQuotationTokens (tokens: list<string>) =
  combineSimple4StateTokens tokens (fun (token, _) -> token = "\"") (fun (token, _) -> token = "\"")

let combineTokens (tokens: list<string>) =
  // Order is important because combining tokens eg in one string literal prevents later combiners from matching the original tokens
  // as they only see one token for the string.
  let withMLComments = combineMLCommentTokens tokens
  let withSLComments = combineSLCommentTokens withMLComments
  let withStrings = combineStringTokens withSLComments
  let withBrackets = combineBracketTokens withStrings
  let withQuotations = combineQuotationTokens withBrackets
  withQuotations

let private getToken (token: string) =
  match token.ToUpper() with
  | " " | "\n"| "\n"| "\r"| "\t" -> WhitespaceToken token
  | "SELECT" -> Select token
  | "FROM" -> From token
  | "INNER" -> Inner token
  | "OUTER" -> Outer token
  | "LEFT" -> Left token
  | "RIGHT" -> Right token
  | "JOIN" -> Join token
  | "AND" -> And token
  | "ON" -> On token
  | "WHERE" -> Where token
  | "ORDER" -> Order token
  | "HAVING" -> Having token
  | "OPTION" -> Option token
  | "," -> Comma token
  | _ -> (AnyToken token)
  

let markTokens (tokens: list<string>) =
  let rec doMark (tokens: list<string>) (marked: list<SqlToken>) =
    match tokens with
    | head::tail -> doMark tail ((getToken head)::marked)
    | [] -> marked
  List.rev (doMark tokens [])

let combineSqlWhitespaceTokens (marked: list<SqlToken>) =
  let rec doCombine (tokens: list<SqlToken>) (result: list<SqlToken>) (spaceTokens: list<string>) =
    match tokens with
    // This token is a whitespace ...
    | (WhitespaceToken value)::tail -> 
      match tail with
      // and next token is also a whitespace.
      | (WhitespaceToken _)::_ -> doCombine tail result (value::spaceTokens)
      // and next token is no whitespace or next token is list end.
      | _ -> doCombine tail ((WhitespaceListToken (value::spaceTokens))::result) []
    // This token is not a whitespace.
    | head::tail -> doCombine tail (head::result) []
    | [] -> result
  List.rev (doCombine marked [] [])

//   | AnyToken of string
// | WhitespaceListToken of list<string>
// | Select of string
// | From of string
// | Inner of string
// | Outer of string
// | Left of string
// | Right of string
// | Join of string
// | And of string
// | On of string
// | Where of string
// | Order of string
// | Having of string
// | Option of string
// | Comma of string