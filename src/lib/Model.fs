module Model

// https://docs.microsoft.com/en-us/sql/t-sql/queries/select-transact-sql?view=sql-server-ver15
// https://docs.microsoft.com/en-us/sql/t-sql/queries/from-transact-sql?view=sql-server-ver15

type SqlToken =
| EmptyToken
| AnyToken
| SpaceOrTabToken
| NewLineToken
| With
| Select
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

// Value is the Value as in the unformatted input string.
[<System.Diagnostics.DebuggerDisplay("\{Token = {Token}; Value = {Value}\}")>]
type TokenVal = { Token: SqlToken; Value: string }
