module MainTests

open System
open Xunit
open Main

// "Scalar functions must be invoked by using at least the two-part name of the function (.). "
// see https://docs.microsoft.com/en-us/sql/t-sql/statements/create-function-transact-sql?view=sql-server-ver15

let sql1 = "select 3, abc from dbo.david where x = 4; select * from abc group by x"

[<Fact>]
let ``main test sql1`` () =
  let result = format sql1
  let actual = result
  printfn "%A" actual
  //let expected = ["select"; " "; "3"]
  //Assert.True((actual = expected))