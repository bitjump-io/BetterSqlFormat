module SimpleFormatTests

open System
open Xunit
open Main

[<Fact>]
let ``select 3 test`` () =
  Assert.Equal(formatSql "select 3", "select 3")