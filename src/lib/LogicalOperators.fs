module LogicalOperators

// see https://docs.microsoft.com/en-us/sql/t-sql/language-elements/logical-operators-transact-sql?view=sql-server-ver15

let logicalOperators = [
  // scalar_expression { = | <> | != | > | >= | !> | < | <= | !< } { ALL | SOME | ANY } ( subquery )  
  "all";
  "some";
  "any";
  // boolean_expression { AND | OR } boolean_expression  
  "and";
  "or";
  // test_expression [ NOT ] BETWEEN begin_expression AND end_expression
  "between";
  // EXISTS ( subquery )
  "exists";
  // test_expression [ NOT ] IN ( subquery | expression [ ,...n ] )
  "in";
  // match_expression [ NOT ] LIKE pattern [ ESCAPE escape_character ] 
  "like";
  // [ NOT ] boolean_expression  
  "not";
]