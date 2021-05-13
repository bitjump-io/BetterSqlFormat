module Program

open TokenizeTests
open ParseTests
open MainTests
open TokenExprTests

[<EntryPoint>]
let main _ =
  // Add the test to debug here.
  ``isMatch Optional(Val(From)) matches Select without consuming``()
  0
