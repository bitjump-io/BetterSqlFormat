module Model

type SqlToken =
| EmptyToken
| AnyToken of string
| WhitespaceToken of string
| WhitespaceListToken of list<string>
| Select of string
| From of string
| Inner of string
| Outer of string
| Left of string
| Right of string
| Join of string
| And of string
| On of string
| Where of string
| Order of string
| Having of string
| Option of string
| Comma of string

// // A separator or a value between separators.
// // E.g. "order by" contains one separator (a space) and a token to the left and right.
// type Token =
// | Token of string

// type Whitespace = 
// | Whitespace of string

// type Whitespaces = Whitespace list

// type Alias =
// | Alias of string // as abc

// type Comma =
// | Comma

// A type for: { alias.* | column_name | expression }
// type SqlExpr =
// | StarChar of string
// | ColumnName of string
// | Expression of string

// type SqlExprWithAlias =
// | SqlExpr of SqlExpr * Whitespaces * Alias

// type Top =
// | Top of string // TOP ( expression ) [PERCENT] [ WITH TIES ] 

// type AllOrDistinct = 
// | All
// | Distinct

// type Select =
// | Select of Top option * AllOrDistinct option * SqlExpr list

// type Keyword =
// | Select of Select

// type Elem =
// | Unknown of string
// | Whitespaces of string
// | Keyword of Keyword // GROUP BY treated as one keyword.
// | SelectList of SqlExpr
// | StringLiteral of string
