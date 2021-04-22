module SimpleFormatTests

open System
open Xunit
open Main

let sql1 = "select 3"

let sql2 = "select 3,4"

let sql3 = @"SELECT *  
FROM DimEmployee  
ORDER BY LastName;"

let sql4 = @"SELECT*
-- Test comment
FROM DimEmployee"

[<Fact>]
let ``tokenize test sql1`` () =
  let actual = tokenize sql1
  let expected = ["select"; " "; "3"]
  Assert.True((actual = expected))

[<Fact>]
let ``tokenize test sql2`` () =
  let actual = tokenize sql2
  let expected = ["select"; " "; "3"; ","; "4"]
  Assert.True((actual = expected))

[<Fact>]
let ``tokenize test sql3`` () =
  let actual = tokenize sql3
  let expected = ["SELECT"; " "; "*"; " "; " "; "\n"; "FROM"; " "; "DimEmployee"; " "; " "; "\n"; "ORDER"; " "; "BY"; " "; "LastName"; ";"]
  Assert.True((actual = expected))

[<Fact>]
let ``tokenize test sql4`` () =
  let actual = tokenize sql4
  let expected = ["SELECT"; "*"; "\n"; "-"; "-"; " "; "Test"; " "; "comment"; "\n"; "FROM"; " "; "DimEmployee"]
  Assert.True((actual = expected))