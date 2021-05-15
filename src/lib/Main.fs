module Main

open Model
open Tokenize
open General
open Transform
open Keywords
open BuildInFunctions

let format sqlStr =
  let tokens = tokenize sqlStr
  let tokens = combineTokens tokens
  let markedTokens = markTokens tokens
  let newLineStr = getLineBreakStr tokens
  let withLineBreaks = addLineBreaks markedTokens newLineStr
  let noSpaceAfterComma = replaceSpaceAfterComma withLineBreaks
  let asStr = toString noSpaceAfterComma
  asStr

let keywords2: list<string> = keywords
let buildInFuncs: list<string> = buildInFunctions