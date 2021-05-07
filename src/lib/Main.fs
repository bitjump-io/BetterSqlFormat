module Main

open Model
open Tokenize
open General
open Transform

let format sqlStr =
  let tokens = tokenize sqlStr
  let tokens = combineTokens tokens
  let markedTokens = markTokens tokens
  let newLineStr = getLineBreakStr tokens
  let combined = combineSqlWhitespaceTokens markedTokens
  let withLineBreaks = addLineBreaks combined newLineStr
  let noSpaceAfterComma = replaceSpaceAfterComma withLineBreaks
  let asStr = toString noSpaceAfterComma
  asStr