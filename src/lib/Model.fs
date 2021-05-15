module Model

// https://docs.microsoft.com/en-us/sql/t-sql/queries/select-transact-sql?view=sql-server-ver15
// https://docs.microsoft.com/en-us/sql/t-sql/queries/from-transact-sql?view=sql-server-ver15

// Value is not part of union item to be able to compare a list of SqlToken with another list of SqlToken independent of the value.
type SqlToken =
| EmptyToken
| AnyToken
| SpaceOrTabToken
| NewLineToken
| With
| Select
| Star // *
| Comma
| From
| Inner
| Outer
| Left
| Right
| Full
| Cross
| Apply
| Join
| And
| On
| Where
| GroupBy
| Having
| OrderBy
| Option
| UnionAll
| Union
| Except
| Intersect
| Into
| VariableName // starts with @
| OtherKeyword // see module Keywords
| DataType // see module DataTypes
| BuildInFunctionCall // see module BuildInFunctions, also for functions starting with @@
| BuildInVariable // e.g. @@ROWCOUNT
| UdfCall // User-defined-function
| Semicolon
| NullLitaral
| NumberLiteral
| BooleanLiteral
| StringLiteral
| ScopeResolutionOp // ::
| AssignmentOp // = += -= *= /= %= &= ^= |=
| ComparisionOp // = > < >= <= <> !< != !>
| ArithmeticOp // + - * / ~ & | ^ ~
| MemberSeparatorOp // .
| OpenParenthesis
| CloseParenthesis
| MultiLineComment
| SingleLineComment
| Temptable // #Bicycles

// Value is the Value as in the unformatted input string.
[<System.Diagnostics.DebuggerDisplay("\{Token = {Token}; Value = {Value}\}")>]
type TokenVal = { Token: SqlToken; Value: string }

type TokenExpr =
  | Val of SqlToken
  | ZeroOrOne of TokenExpr
  | ZeroOrMore of TokenExpr
  // OneOrMore
  | Or of TokenExpr * TokenExpr
  | AndThen of TokenExpr * TokenExpr // Left side matches current element, right side matches next element.

type MatchResult =
  | MatchSuccess of list<TokenVal> * list<TokenVal> // skippedTokens * remainingTokens
  | MatchFailure