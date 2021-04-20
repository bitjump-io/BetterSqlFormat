module SimpleFormatTests

open System
open Xunit
open Main

[<Fact>]
let ``tokenize test 1`` () =
  let actual = tokenize "select 3"
  let expected = ["select"; " "; "3"]
  Assert.True((actual = expected))