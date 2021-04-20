module Main

open Model

let tokenize (sqlStr: string) =
  // Add characters to the token until there is a separator.
  // Each separtor is a separate token.
  // A single operator as <> is divided into two tokens.
  // A word like "declare" is always one token.
  let mutable tokens = []
  let mutable pos = 0
  let mutable lastTokenEndPos = -1
  let sqlStrLen = sqlStr.Length
  while pos < sqlStrLen do
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
    ) then
      // Current char is a separator/operator so complete pevious token.
      let newTokenLen = pos - (lastTokenEndPos + 1)
      if (newTokenLen > 0) then
        let newToken = sqlStr.Substring(lastTokenEndPos + 1, newTokenLen)
        tokens <- newToken :: tokens
      // Also create token for current separator/operator character.
      let newToken = sqlStr.Substring(pos, 1)
      tokens <- newToken :: tokens
      lastTokenEndPos <- pos
    pos <- pos + 1
  // At the end push remaining chars as token.
  let newTokenLen = pos - (lastTokenEndPos + 1)
  if (newTokenLen > 0) then
    let newToken = sqlStr.Substring(lastTokenEndPos + 1, newTokenLen)
    tokens <- newToken :: tokens
  List.rev tokens

// let parseSql sqlStr =

let formatSql sqlStr =
  sqlStr
