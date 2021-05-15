module Keywords

// Keywords that are used in processing logic are also inluded in the SqlToken module.

let keywords = [
  "with";
  "select";
  "distinct";
  "from";
  "inner";
  "outer";
  "left";
  "right";
  "full";
  "cross";
  "apply";
  "join";
  "and";
  "on";
  "where";
  "group";
  "by";
  "having";
  "order";
  "option";
  "union";
  "except";
  "intersect";
  "all";
  "any";
  "between";
  "exists";
  "in";
  "not";
  "like";
  "escape"; // LIKE '%greena_%' ESCAPE 'a'
  "or";
  "some";
  "into";
  "as";
  "go";
  "set";
  "declare";
  "use";
  "begin";
  "end";
  "break";
  "continue";
  "goto";
  "if";
  "else";
  "return";
  "throw";
  "try";
  "catch";
  "waitfor";
  "while";
  "close";
  "create";
  "procedure";
  "deallocate";
  "cursor";
  "delete";
  "fetch";
  "open";
  "update";
  "case";
  "coalesce";
  "nullif";
  "distributed";
  "transaction";
  "commit";
  "rollback";
  "work"; // ROLLBACK WORK
  "save"; // SAVE TRANSACTION
  "nocount";
  "off";
  "drop";
  "table";
  "bulk";
  "insert";
  "merge";
  "readtext";
  "updatetext";
  "writetext";
  "holdlock";
  "at"; // AT TIME ZONE
  "time";
  "zone";
  "option";
  "label"; // OPTION ( LABEL = 'q17' )
  "hash";
  "loop";
  "force"; // OPTION (FORCE ORDER)
  "disable";
  "externalpushdown";
  "fast"; // OPTION (HASH GROUP, FAST 10)
  "readpast"; //  WITH (READPAST)
  "tablock";
  "maxrecursion";
  "concat";
  "expand";
  "views";
  "scaleoutexecution";
  "ignore_nonclustered_columnstore_index";
  "keep";
  "plan";
  "keepfixed";
  "max_grant_percent";
  "min_grant_percent";
  "maxdop";
  "no_performance_spool";
  "optimize";
  "unknown";
  "parameterization";
  "simple";
  "forced";  
  "querytraceon";
  "recompile";
  "robust";
  "hint";
  "noexpand";
  "index";
  "forceseek";
  "forcescan";
  "holdlock";
  "nolock";
  "nowait";
  "paglock";
  "readcommitted";
  "readcommittedlock";
  "readpast";
  "readuncommitted";
  "repeatableread";
  "rowlock";
  "serializable";
  "snapshot";
  "spatial_window_max_cells";
  "tablock";
  "tablockx";
  "updlock";
  "xlock";
  "matched"; // WHEN MATCHED AND ... THEN UPDATE
  "$action"; // OUTPUT $action
  "$identity";
  "$rowguid";
  "then";
  "view";
  "output";
  "deleted";
  "inserted";
  "print";
  "top";
  "percent";
  "ties";
  "offset";
  "match";
  "contains";
  "freetext";
  "values";
  "default";
  "null"; // Null is a value but also a keyword as in DEFAULT NULL.
  "using";
  "asc";
  "desc";
  "log"; // WITH LOG
  "alter";
  "database";
  "recovery"; //  SET RECOVERY SIMPLE
  "simple";
  "bulk_logged";
  "for"; // FOR BROWSE 
  "browse";
  "pivot";
  "xmlnamespaces";
  "cube";
  "rollup";
  // todo: https://docs.microsoft.com/en-us/sql/t-sql/queries/select-for-clause-transact-sql?view=sql-server-ver15
]