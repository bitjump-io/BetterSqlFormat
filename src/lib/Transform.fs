module Transform

open Model

// Returns true or false and tail of the list which was not consumed by the match.
let rec isMatch (input: list<TokenVal>) (tokenExpr: TokenExpr) =
  match input with
  | [] -> (false, input)
  | head::tail ->
    match tokenExpr with
    | Val value -> (value = head.Token, tail)
    | ZeroOrOne expr -> 
      match isMatch input expr with
      | (true, remList) -> (true, remList)
      | (false, _) -> (true, input)
    | ZeroOrMore expr -> 
      match isMatch input expr with
      | (true, [])  -> (true, [])
      | (true, remList) -> isMatch remList tokenExpr
      | (false, _) -> (true, input)
    | Or (leftExpr, rightExpr) ->
      match isMatch input leftExpr with
      | (true, remList) -> (true, remList)
      | (false, _) ->
        match isMatch input rightExpr with
        | (true, remList) -> (true, remList)
        | (false, _) -> (false, input)
    | AndThen (leftExpr, rightExpr) ->
      match isMatch input leftExpr with
      | (false, _) -> (false, input)
      | (true, remList) ->
        match isMatch remList rightExpr with
        | (true, remList2) -> (true, remList2)
        | (false, _) -> (false, input)

let private findFirstMatch (tokens: list<TokenVal>) (tokenExpr: TokenExpr) =
  let rec doFindFirstMatch (tokens: list<TokenVal>) (skippedTokens: list<TokenVal>) =
    match isMatch tokens tokenExpr with
    | (true, remList) -> MatchSuccess (skippedTokens, remList)
    | (false, _) ->
      match tokens with
      | head::tail -> doFindFirstMatch tail (head::skippedTokens)
      | [] -> MatchFailure
  doFindFirstMatch tokens []

let private replaceTokens (tokens: list<TokenVal>) (tokenExpr: TokenExpr) (replaceSeq: list<TokenVal>) =
  let rec doReplaceTokens (tokens: list<TokenVal>) (result: list<TokenVal>) =
    match findFirstMatch tokens tokenExpr with
    | MatchSuccess (skippedTokens, []) -> (List.rev skippedTokens)@(replaceSeq)@result
    | MatchSuccess (skippedTokens, remList) -> doReplaceTokens remList ((List.rev skippedTokens)@(replaceSeq)@result)
    | MatchFailure -> result@tokens
  doReplaceTokens tokens []

let addLineBreaks (tokens: list<TokenVal>) (newLineStr: string) =
  let tokens = replaceTokens tokens (AndThen(ZeroOrMore(Val(SpaceOrTabToken)), Val(From))) [{ Token = NewLineToken; Value = newLineStr}; { Token = From; Value = "FROM"}]
  let tokens = replaceTokens tokens (AndThen(ZeroOrMore(Val(SpaceOrTabToken)), Val(Where))) [{ Token = NewLineToken; Value = newLineStr}; { Token = Where; Value = "WHERE"}]
  tokens

let replaceSpaceAfterComma (tokens: list<TokenVal>) =
  replaceTokens tokens (AndThen(Val(NewLineToken), AndThen(Val(Comma), Val(SpaceOrTabToken)))) [{ Token = NewLineToken; Value = "\n"}; { Token = Comma; Value = ","}]

let toString (tokens: list<TokenVal>) =
  let rec doToString (tokens: list<TokenVal>) (accumulator: string) =
    match tokens with
    | head::tail -> doToString tail (accumulator + head.Value)
    | [] -> accumulator
  doToString tokens ""
