module Main

open Model

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


// Combine tokens that together are one string.
// This is useful because a string may contain a keyword as in 'Make an update.'
// let combineStringTokens sqlStr =
//   let tokens = tokenize sqlStr
//   let result = []
//   let mutable combinedTokens = []
//   let tokensLen = List.length tokens
//   for i in [0..tokensLen] do
//     if (token = "'") then
//       combinedTokens <- token :: combinedTokens



// // Combine tokens that together are one comment.
// let combineCommentTokens sqlStr =
//   let tokens = tokenize sqlStr

let formatSql sqlStr =
  sqlStr
