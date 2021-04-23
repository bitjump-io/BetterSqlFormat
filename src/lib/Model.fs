module Model

// A separator or a value between separators.
// E.g. "order by" contains one separator (a space) and a token to the left and right.
type Token =
| Token of string

type Whitespace = 
| Whitespace of string

type Whitespaces = Whitespace list

type Alias =
| Alias of string // as abc

type Comma =
| Comma

// A type for: { * | column_name | expression }
type SelectColumn =
| StarChar of string
| ColumnName of string
| Expression of string

type SelectColumnWithAlias =
| SelectColumn of SelectColumn * Whitespaces * Alias

type Top =
| Top of string // TOP ( expression ) [PERCENT] [ WITH TIES ] 

type AllOrDistinct = 
| All
| Distinct

type Select =
| Select of Top option * AllOrDistinct option * SelectColumn list

type Keyword =
| Select of Select

type Elem =
| Unknown of string
| Whitespaces of string
| Keyword of Keyword // GROUP BY treated as one keyword.
| SelectList of SelectColumn
| StringLiteral of string
