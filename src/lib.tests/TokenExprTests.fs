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
let ``isMatch Optional(Val(From)) matches From`` () =
  let tokens = [{ Token = SqlToken.From; Value = "From" }]
  let expr = TokenExpr.Optional(TokenExpr.Val(From))
  let actual = isMatch tokens expr
  let expected = (true, [])
  Assert.True((actual = expected))

[<Fact>]
let ``isMatch Optional(Val(From)) matches Select without consuming`` () =
  let tokens = [{ Token = SqlToken.Select; Value = "Select" }]
  let expr = TokenExpr.Optional(TokenExpr.Val(From))
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
let ``isMatch Or(Val(Inner), Or(Val(From), Optional(Val(Where)))) matches Where`` () =
  let tokens = [{ Token = SqlToken.Where; Value = "Where" }]
  let expr = TokenExpr.Or(Val(Inner), Or(Val(From), Optional(Val(Where))))
  let actual = isMatch tokens expr
  let expected = (true, [])
  Assert.True((actual = expected))
  