module TokenExprTests

open System
open Xunit
open Transform
open Model

[<Fact>]
let ``isMatch Val(From) matches From`` () =
  let tokens = [{ Token = SqlToken.From; Value = "From" }]
  let expr = TokenExpr.Val(From)
  let actual = isMatch tokens expr
  let expected = (true, [])
  Assert.True((actual = expected))

[<Fact>]
let ``isMatch ZeroOrOne(Val(From)) matches From`` () =
  let tokens = [{ Token = SqlToken.From; Value = "From" }]
  let expr = TokenExpr.ZeroOrOne(TokenExpr.Val(From))
  let actual = isMatch tokens expr
  let expected = (true, [])
  Assert.True((actual = expected))

[<Fact>]
let ``isMatch ZeroOrOne(Val(From)) matches Select without consuming`` () =
  let tokens = [{ Token = SqlToken.Select; Value = "Select" }]
  let expr = TokenExpr.ZeroOrOne(TokenExpr.Val(From))
  let actual = isMatch tokens expr
  let expected = (true, [{ Token = SqlToken.Select; Value = "Select" }])
  Assert.True((actual = expected))

[<Fact>]
let ``isMatch Or(Val(Inner), Val(Outer)) matches Inner`` () =
  let tokens = [{ Token = SqlToken.Inner; Value = "Inner" }; { Token = SqlToken.Select; Value = "Select" }]
  let expr = TokenExpr.Or(TokenExpr.Val(Inner), TokenExpr.Val(Outer))
  let actual = isMatch tokens expr
  let expected = (true, [{ Token = SqlToken.Select; Value = "Select" }])
  Assert.True((actual = expected))

[<Fact>]
let ``isMatch Or(Val(Inner), Val(Outer)) matches Outer`` () =
  let tokens = [{ Token = SqlToken.Outer; Value = "Outer" }; { Token = SqlToken.Select; Value = "Select" }]
  let expr = TokenExpr.Or(TokenExpr.Val(Inner), TokenExpr.Val(Outer))
  let actual = isMatch tokens expr
  let expected = (true, [{ Token = SqlToken.Select; Value = "Select" }])
  Assert.True((actual = expected))

[<Fact>]
let ``isMatch Or(Val(Inner), Or(Val(From), ZeroOrOne(Val(Where)))) matches Where`` () =
  let tokens = [{ Token = SqlToken.Where; Value = "Where" }]
  let expr = TokenExpr.Or(Val(Inner), Or(Val(From), ZeroOrOne(Val(Where))))
  let actual = isMatch tokens expr
  let expected = (true, [])
  Assert.True((actual = expected))
  
[<Fact>]
let ``isMatch ZeroOrMore(Val(SpaceOrTabToken)) matches <space><tab><space>`` () =
  let tokens = [{ Token = SqlToken.SpaceOrTabToken; Value = " " }; { Token = SqlToken.SpaceOrTabToken; Value = "\t" }; { Token = SqlToken.SpaceOrTabToken; Value = " " }]
  let expr = TokenExpr.ZeroOrMore(Val(SpaceOrTabToken))
  let actual = isMatch tokens expr
  let expected = (true, [])
  Assert.True((actual = expected))

[<Fact>]
let ``isMatch AndThen(Val(Left), AndThen(Val(SpaceOrTabToken), Val(Join))) matches Left Join`` () =
  let tokens = [{ Token = SqlToken.Left; Value = "Left" }; { Token = SqlToken.SpaceOrTabToken; Value = " " }; { Token = SqlToken.Join; Value = "Join" }]
  let expr = TokenExpr.AndThen(Val(Left), AndThen(Val(SpaceOrTabToken), Val(Join)))
  let actual = isMatch tokens expr
  let expected = (true, [])
  Assert.True((actual = expected))

[<Fact>]
let ``isMatch AndThen(Val(Left), AndThen(Val(SpaceOrTabToken), Val(Outer))) does not match`` () =
  let tokens = [{ Token = SqlToken.Left; Value = "Left" }; { Token = SqlToken.SpaceOrTabToken; Value = " " }; { Token = SqlToken.Outer; Value = "Outer" }]
  let expr = TokenExpr.AndThen(Val(Left), AndThen(Val(SpaceOrTabToken), Val(Join)))
  let actual = isMatch tokens expr
  let expected = (false, tokens)
  Assert.True((actual = expected))