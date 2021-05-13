module Transform

open Model

// Returns true or false and tail of the list which was not consumed by the match.
let rec isMatch (input: list<TokenVal>) (tokenExpr: TokenExpr) =
  match input with
  | [] -> (false, input)
  | head::tail ->
    match tokenExpr with
    | Val value -> (value = head.Token, tail)
    | Optional expr -> 
      match isMatch input expr with
      | (true, remList) -> (true, remList)
      | (false, _) -> (true, input)
    | Or (leftExpr, rightExpr) ->
      match isMatch input leftExpr with
      | (true, remList) -> (true, remList)
      | (false, _) ->
        match isMatch input rightExpr with
        | (true, remList) -> (true, remList)
        | (false, _) -> (false, input)

let rec private findFirstMatch (tokens: list<TokenVal>) (tokenExpr: TokenExpr) =
  match isMatch tokens tokenExpr with
  | (true, remList) -> (true, remList)
  | (false, _) ->
    match tokens with
    | _::tail -> findFirstMatch tail tokenExpr
    | [] -> (false, [])

let rec private replaceTokens (tokens: list<TokenVal>) (tokenExpr: TokenExpr) (replaceSeq: list<TokenVal>) (result: list<TokenVal>) =
  match findFirstMatch tokens tokenExpr with
  | (true, []) -> List.rev ((List.rev replaceSeq)@result)
  | (true, remList) -> replaceTokens remList tokenExpr replaceSeq (List.rev replaceSeq)@result
  | (false, []) -> List.rev result
  | _ -> failwith "should not happen"

let private replaceMatchedTokens (tokens: list<TokenVal>) (matchSequ: list<SqlToken>) (replaceSeq: list<TokenVal>) =
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
  replaceMatchedTokens tokens [NewLineToken; Comma; SpaceOrTabToken ] [{ Token = NewLineToken; Value = "\n"}; { Token = Comma; Value = ","}]

let toString (tokens: list<TokenVal>) =
  let rec doToString (tokens: list<TokenVal>) (accumulator: string) =
    match tokens with
    | head::tail -> doToString tail (accumulator + head.Value)
    | [] -> accumulator
  doToString tokens ""
