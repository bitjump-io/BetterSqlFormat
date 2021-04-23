module Main

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

type CommentToken =
| NoComment of string
| InComment of string
| StartOfComment of string // Should always be '-'.
| EndOfComment of string // The last character in the line before \n.

// Converts list of string in list of CommentToken.
let rec convertToCommentTokens (tokens: list<string>) (lastResult: CommentToken) (result: list<CommentToken>) =
  match tokens with
  | token :: tail ->
    let nextToken = List.tryHead tail
    let converted = 
      match lastResult with
      | NoComment _ | EndOfComment _ when (token = "-" && nextToken = Some("-")) ->
        StartOfComment token
      | NoComment _ | EndOfComment _ ->
        NoComment token
      | InComment _ when nextToken = Some("\n") || nextToken = None ->
        EndOfComment token
      | InComment _ | StartOfComment _ ->
        InComment token
      | _ ->
        failwith token
    convertToCommentTokens tail converted (converted::result)
  | [] ->
    List.rev result

// Combine tokens that together are one comment.
// This is useful because a comment may contain a keyword as in -- Make an update.
let rec combineCommentTokens (tokens: list<string>) =
  let markedTokens = convertToCommentTokens tokens (NoComment "") []
  
  let rec doCombine (tokens: list<CommentToken>) (currentCommentTokens: list<string>) (newTokens: list<string>) =
    match tokens with
    | head::tail ->
      let (currentCommentParts, newTokensResult) = 
        match head with
        | NoComment value -> [], (value::newTokens)
        | StartOfComment value -> [value], newTokens
        | InComment value -> 
          (value::currentCommentTokens), newTokens
        | EndOfComment value -> 
          let tokensOfComment = List.rev (value::currentCommentTokens)
          let strWithComments = String.Join("", tokensOfComment)
          [], (strWithComments::newTokens)
      doCombine tail currentCommentParts newTokensResult
    | [] -> newTokens

  List.rev (doCombine markedTokens [] [])

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