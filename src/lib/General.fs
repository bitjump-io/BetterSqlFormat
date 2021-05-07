module General

let rec getLineBreakStr (tokens: list<string>) =
  match tokens with
  | head::tail ->
    if head = "\r" && List.head tail = "\n" then "\r\n"
    else getLineBreakStr tail
  | [] -> "\n"
