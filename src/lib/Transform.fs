module Transform

open Model

let isLineBreakToken (sqlToken: SqlToken) =
  match sqlToken with
  | Select x | From x | Inner x | Outer x | Left x | Right x | Join x | And x | On x | Where x 
  | Order x | Having x | Option x | Comma x -> true
  | _ -> false

let addLineBreaks (tokens: list<SqlToken>) (newLineStr: string) =
  let newLineSqlToken = WhitespaceListToken [newLineStr]
  let rec doTransform (tokenPairs: list<SqlToken * SqlToken>) (lastToken: SqlToken) (newList: list<SqlToken>) =
    match tokenPairs with
    // Replace whitespacelist followed by lineBreak token with new line.
    | (WhitespaceListToken _, p2)::tail when isLineBreakToken(p2) -> doTransform tail p2 (newLineSqlToken::newList)
    // For non-whitespace followed by lineBreak token insert new line.
    | (p1, p2)::tail when isLineBreakToken(p2) -> doTransform tail p2 (newLineSqlToken::p1::newList)
    | (p1, p2)::tail -> doTransform tail p2 (p1::newList)
    | [] -> lastToken::newList // lastToken needed because of pairwise
  doTransform (List.pairwise tokens) EmptyToken [] |> List.rev

let private replaceTokenSequence (tokens: list<SqlToken>) (matchSequ: list<SqlToken>) (replaceSeq: list<SqlToken>) =
  let replaceSeqRev = List.rev replaceSeq
  let windowLen = List.length matchSequ
  let rec doReplace (tokens: list<list<SqlToken>>) (lastTokens: list<SqlToken>) (result: list<SqlToken>) =
    match tokens with
    | head::tail when head = matchSequ -> doReplace (List.skip (windowLen-1) tail) [] (replaceSeqRev@result)
    | head::tail -> doReplace tail (List.tail head) ((List.head head)::result)
    | [] -> (List.rev lastTokens)@result
  let windowedTokens = List.windowed windowLen tokens
  doReplace windowedTokens [] [] |> List.rev

let replaceSpaceAfterComma (tokens: list<SqlToken>) =
  replaceTokenSequence tokens [WhitespaceListToken ["\n"]; Comma ","; WhitespaceListToken [" "]] [WhitespaceListToken ["\n"]; Comma ","]

let rec tokenValueToString (token: SqlToken) =
  match token with
  | EmptyToken -> ""
  | AnyToken x -> x
  | WhitespaceToken x -> x
  | WhitespaceListToken x ->
    match x with
    | head::tail -> head + (tokenValueToString (WhitespaceListToken tail))
    | [] -> ""
  | Select x -> x
  | From x -> x
  | Inner x -> x
  | Outer x -> x
  | Left x -> x
  | Right x -> x
  | Join x -> x
  | And x -> x
  | On x -> x
  | Where x -> x
  | Order x -> x
  | Having x -> x
  | Option x -> x
  | Comma x -> x

let toString (tokens: list<SqlToken>) =
  let rec doToString (tokens: list<SqlToken>) (accumulator: string) =
    match tokens with
    | head::tail -> doToString tail (accumulator + (tokenValueToString head))
    | [] -> accumulator
  doToString tokens ""
