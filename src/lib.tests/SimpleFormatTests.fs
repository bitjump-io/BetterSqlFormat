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

let sql5 = "select 3, 'hello world'"

let sql6 = @"select 'What''s \that?; .-|\"" bike.', 'x',4+5"

let sql7 = @"select '''','''''valid sql''' "

let sql8 = @"select -- hello world!"

let sql9 = @"select -- hello
-- world
select 4"

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

[<Fact>]
let ``combineStringTokens test sql5`` () =
  let tokens = tokenize sql5
  let actual = combineStringTokens tokens
  let expected = ["select"; " "; "3"; ","; " "; "'hello world'"]
  Assert.True((actual = expected))

[<Fact>]
let ``combineStringTokens test sql6`` () =
  let tokens = tokenize sql6
  let actual = combineStringTokens tokens
  let expected = ["select"; " "; @"'What''s \that?; .-|\"" bike.'"; ","; " "; "'x'"; ","; "4"; "+"; "5"]
  Assert.True((actual = expected))

[<Fact>]
let ``combineStringTokens test sql7`` () =
  let tokens = tokenize sql7
  let actual = combineStringTokens tokens
  let expected = ["select"; " "; "''''"; ","; "'''''valid sql'''"; " "]
  Assert.True((actual = expected))

[<Fact>]
let ``combineCommentTokens test sql8`` () =
  let tokens = tokenize sql8
  let actual = combineCommentTokens tokens
  let expected = ["select"; " "; "-- hello world!"]
  Assert.True((actual = expected))

[<Fact>]
let ``combineCommentTokens test sql9`` () =
  let tokens = tokenize sql9
  let actual = combineCommentTokens tokens
  let expected = ["select"; " "; "-- hello"; "\n"; "-- world"; "\n"; "select"; " "; "4"]
  Assert.True((actual = expected))