module Keywords

// Keywords that are used in processing logic are also inluded in the SqlToken module.
// Also see https://en.wikipedia.org/wiki/SQL_reserved_words

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
  "$node_id";
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
  "unpivot";
  "xmlnamespaces";
  "cube";
  "rollup";
  "UNIQUE"; // INT NULL UNIQUE
  "VALUES";
  "NO_BROWSETABLE"; // strictly this is an option not a keyword
  "xml";
  "path";
  "auto"; // XML mode. FOR XML AUTO, TYPE, XMLSCHEMA, ELEMENTS XSINIL;
  "raw"; // XML mode.
  "explicit"; // XML mode.
  "type"; // strictly this is an option not a keyword
  "xmldata"; // strictly this is an option not a keyword
  "xmlschema"; // strictly this is an option not a keyword
  "elements"; // strictly this is an option not a keyword
  "xsinil";  // strictly this is an option not a keyword
  "absent";  // strictly this is an option not a keyword
  "binary";
  "base64";
  "root";
  "json";
  "include_null_values";
  "without_array_wrapper";
  "rollup"; // GROUP BY ROLLUP (col1, col2, col3, col4) 
  "cube"; // GROUP BY CUBE (Country, Region)
  "grouping"; // GROUP BY GROUPING SETS ( ROLLUP (Country, Region), CUBE (Country, Region) )
  "sets";
  "distributed_agg"; // GROUP BY CustomerKey WITH (DISTRIBUTED_AGG)
  "exec";
  "add";
  "file";
  "to";
  "filegroup";
  "collate";
  "row";
  "rows";
  "first";
  "next";
  "only";
  "over"; // count(*) over(order by object_id ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW )
  "partition";
  "range";
  "unbounded";
  "preceding";
  "following";
  "current";
  "system_time"; // FOR SYSTEM_TIME <system_time> ] [ AS ] table_alia
  "openxml";
  "tablesample";
  "system";
  "repeatable";
  "contained"; // CONTAINED IN (<start_date_time> , <end_date_time>)
  "of"; // AS OF <date_time>
  "reduce"; // join hint
  "replicate"; // join hint
  "redistribute"; // join hint
  "explain";
  "last_node";
  "shortest_path";
  "raiserror";
  "checkpoint";
  "kill";
  "stats";
  "job";
  "reconfigure";
  "shutdown";
  "within_group";
  "when";
  "varying";
  "user";
  "unique";
  "tsequal";
  "truncate";
  "trigger";
  "tran";
  "textsize";
  "statistics";
  "setuser";
  "semanticsimilaritytable";
  "semanticsimilaritydetailstable";
  "semantickeyphrasetable";
  "securityaudit";
  "schema";
  "rule";
  "rowguidcol";
  "rowcount";
  "revoke";
  "revert";
  "restrict";
  "restore";
  "replication";
  "references";
  "read";
  "public";
  "proc";
  "primary";
  "precision";
  "offsets";
  "nonclustered";
  "nocheck";
  "national";
  "load";
  "lineno";
  "key";
  "is";
  "identitycol";
  "identity_insert";
  "grant";
  "function";
  "freetexttable";
  "foreign";
  "fillfactor";
  "external";
  "exit";
  "execute";
  "errlvl";
  "dump";
  "double";
  "disk";
  "deny";
  "dbcc";
  "current_user";
  "current_timestamp";
  "current_time";
  "current_date";
  "containstable";
  "constraint";
  "compute";
  "column";
  "clustered";
  "check";
  "cascade";
  "backup";
  "authorization";
  // todo: https://docs.microsoft.com/en-us/sql/t-sql/queries/hints-transact-sql?view=sql-server-ver15
]
