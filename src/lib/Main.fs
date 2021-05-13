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
  //let withLineBreaks = addLineBreaks markedTokens newLineStr
  let noSpaceAfterComma = replaceSpaceAfterComma markedTokens
  let asStr = toString noSpaceAfterComma
  asStr