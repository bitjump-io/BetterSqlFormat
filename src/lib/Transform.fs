module Transform

open Model

let isLineBreakToken (sqlToken: SqlToken) =
  match sqlToken with
  | Select | From | Inner | Outer | Left | Right | Join | And | On | Where 
  | OrderBy | Having | Option | Comma -> true
  | _ -> false

let addLineBreaks (tokens: list<TokenVal>) (newLineStr: string) =
  let newLineSqlToken = { Token = NewLineToken; Value = newLineStr }
  let rec doTransform (tokenPairs: list<TokenVal * TokenVal>) (lastToken: TokenVal) (newList: list<TokenVal>) =
    match tokenPairs with
    // Replace whitespacelist followed by lineBreak token with new line.
    | ({ Token = SpaceOrTabToken; Value = _ }, p2)::tail when isLineBreakToken(p2.Token) -> doTransform tail p2 (newLineSqlToken::newList)
    // For non-whitespace followed by lineBreak token insert new line.
    | (p1, p2)::tail when isLineBreakToken(p2.Token) -> doTransform tail p2 (newLineSqlToken::p1::newList)
    | (p1, p2)::tail -> doTransform tail p2 (p1::newList)
    | [] -> lastToken::newList // lastToken needed because of pairwise
  doTransform (List.pairwise tokens) { Token = EmptyToken; Value = "" } [] |> List.rev

let private replaceTokenSequence (tokens: list<TokenVal>) (matchSequ: list<SqlToken>) (replaceSeq: list<TokenVal>) =
  let replaceSeqRev = List.rev replaceSeq
  let windowLen = List.length matchSequ
  let rec doReplace (tokens: list<list<TokenVal>>) (lastTokens: list<TokenVal>) (result: list<TokenVal>) =
    match tokens with
    | head::tail when (head |> List.map (fun x -> x.Token)) = matchSequ -> doReplace (List.skip (windowLen-1) tail) [] (replaceSeqRev@result)
    | head::tail -> doReplace tail (List.tail head) ((List.head head)::result)
    | [] -> (List.rev lastTokens)@result
  let windowedTokens = List.windowed windowLen tokens
  doReplace windowedTokens [] [] |> List.rev

let replaceSpaceAfterComma (tokens: list<TokenVal>) =
  replaceTokenSequence tokens [NewLineToken; Comma; SpaceOrTabToken ] [{ Token = NewLineToken; Value = "\n"}; { Token = Comma; Value = ","}]

let toString (tokens: list<TokenVal>) =
  let rec doToString (tokens: list<TokenVal>) (accumulator: string) =
    match tokens with
    | head::tail -> doToString tail (accumulator + head.Value)
    | [] -> accumulator
  doToString tokens ""
