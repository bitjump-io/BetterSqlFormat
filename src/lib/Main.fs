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
        tokens <- newToken :: tokens
      // Also create token for current separator/operator character.
      let newToken = sqlStr.[pos .. pos]
      tokens <- newToken :: tokens
      lastTokenEndPos <- pos
    if (pos = sqlStrLen-1) then
      // At the end push remaining chars as token.
      let newToken = sqlStr.[lastTokenEndPos + 1 .. pos]
      if (newToken <> "") then
        tokens <- newToken :: tokens
  List.rev tokens

type StringTokenResult =
| NoString of string // no sql string token
| StartOfString of string // sql string start token
| InString of string // sql string start token
| EndOfString of string // sql string end token

// Converts list of string in list of StringTokenResult. The result list is reversed.
let rec convertToStringTokenResults (tokens: list<string>) (lastResult: StringTokenResult) (result: list<StringTokenResult>) =
  match tokens with
  | token :: tail ->
    match lastResult with
    | InString lastToken when (token = "'" && lastToken <> "\\") ->
      convertToStringTokenResults tail (EndOfString token) ((EndOfString token) :: result)
    | InString _ ->
      convertToStringTokenResults tail (InString token) ((InString token) :: result)
    | StartOfString _ when (token <> "'") ->
      convertToStringTokenResults tail (InString token) ((InString token) :: result)
    | _ when (token = "'") ->
      convertToStringTokenResults tail (StartOfString token) ((StartOfString token) :: result)
    | _ ->
      convertToStringTokenResults tail (NoString token) ((NoString token) :: result)
  | [] -> result

// Combine tokens that together are one string.
// This is useful because a string may contain a keyword as in 'Make an update.'
let rec combineStringTokens (tokens: list<string>) =
  let markedTokens = convertToStringTokenResults tokens (NoString "") []
  
  let rec doCombine (tokens: list<StringTokenResult>) (currentStringTokens: list<string>) (newTokens: list<string>) =
    match tokens with
    | head::tail ->
      match head with
      | NoString value -> doCombine tail [] (value :: newTokens)
      | StartOfString value -> doCombine tail [value] (newTokens)
      | InString value -> doCombine tail (value::currentStringTokens) (newTokens)
      | EndOfString value -> 
        let tokensOfString = List.rev (value::currentStringTokens)
        let strWithQuotes = String.Join("", tokensOfString)
        doCombine tail [] (strWithQuotes :: newTokens)
    | [] -> newTokens

  doCombine markedTokens [] []
  // let result = []
  // let mutable combinedTokens = []
  // let tokensLen = List.length tokens
  
  // if tokens do
  //   if (token = "'") then
  //     combinedTokens <- token :: combinedTokens

// // Combine tokens that together are one comment.
// let combineCommentTokens sqlStr =
//   let tokens = tokenize sqlStr

let formatSql sqlStr =
  sqlStr
