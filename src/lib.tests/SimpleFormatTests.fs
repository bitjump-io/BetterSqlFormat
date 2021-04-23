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

let sql7 = @"select N'some unicode',N'and Nmore'"

let sql8 = @"select '''','''''valid sql''' "

let sql9 = @"select -- hello world!"

let sql10 = @"select -- hello
-- world
select 4"

let sql11 = @"select 4/*-- hello
-- world
select 4*/"

let sql12 = @"select /* some comment */'world'"

let sql13 = @"select /* nested /* so*/me */4"

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
  let expected = ["select"; " "; "N'some unicode'"; ","; "N'and Nmore'"]
  Assert.True((actual = expected))

[<Fact>]
let ``combineStringTokens test sql8`` () =
  let tokens = tokenize sql8
  let actual = combineStringTokens tokens
  let expected = ["select"; " "; "''''"; ","; "'''''valid sql'''"; " "]
  Assert.True((actual = expected))

[<Fact>]
let ``combineCommentTokens test sql9`` () =
  let tokens = tokenize sql9
  let actual = combineCommentTokens tokens
  let expected = ["select"; " "; "-- hello world!"]
  Assert.True((actual = expected))

[<Fact>]
let ``combineCommentTokens test sql10`` () =
  let tokens = tokenize sql10
  let actual = combineCommentTokens tokens
  let expected = ["select"; " "; "-- hello"; "\n"; "-- world"; "\n"; "select"; " "; "4"]
  Assert.True((actual = expected))

[<Fact>]
let ``combineMLCommentTokens test sql11`` () =
  let tokens = tokenize sql11
  let actual = combineMLCommentTokens tokens
  let expected = ["select"; " "; "4"; "/*-- hello\n-- world\nselect 4*/"]
  Assert.True((actual = expected))

[<Fact>]
let ``combineMLCommentTokens test sql12`` () =
  let tokens = tokenize sql12
  let actual = combineMLCommentTokens tokens
  let expected = ["select"; " "; "/* some comment */"; "'"; "world"; "'"]
  Assert.True((actual = expected))

[<Fact>]
let ``combineMLCommentTokens test sql13`` () =
  let tokens = tokenize sql13
  let actual = combineMLCommentTokens tokens
  let expected = ["select"; " "; "/* nested /* so*/me */"; "4"]
  Assert.True((actual = expected))