module ParseTests

open System
open Xunit
open Tokenize

// "Scalar functions must be invoked by using at least the two-part name of the function (.). "
// see https://docs.microsoft.com/en-us/sql/t-sql/statements/create-function-transact-sql?view=sql-server-ver15

let sql1 = "select 3"

[<Fact>]
let ``parse test sql1`` () =
  let tokens = tokenize sql1
  let tokens = combineTokens tokens
  let actual = tokens
  let expected = ["select"; " "; "3"]
  Assert.True((actual = expected))