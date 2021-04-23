module Main

open Model
open System

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
| StartOfString of string // sql string start token.
| InString of string // sql string text token.
| InStringDoubleQuoteStart of string // sql string text token.
| InStringDoubleQuoteEnd of string // sql string text token.
| EndOfString of string // sql string end token.

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
      | NoString _ when (token = "'") ->
        StartOfString token
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
        | StartOfString value -> [value], newTokens
        | InString value | InStringDoubleQuoteStart value | InStringDoubleQuoteEnd value -> 
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
| EndOfComment of string // Should always be "\n" or "" (empty string if last character in text).

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
