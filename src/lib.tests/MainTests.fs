module MainTests

open System
open Xunit
open Main

// "Scalar functions must be invoked by using at least the two-part name of the function (.). "
// see https://docs.microsoft.com/en-us/sql/t-sql/statements/create-function-transact-sql?view=sql-server-ver15

let sql1 = "select 3, abc from dbo.david where x = 4; select * from abc group by x"

[<Fact>]
let ``main test sql1`` () =
  let result = format sql1
  let actual = result
  printfn "%A" actual
  //let expected = ["select"; " "; "3"]
  //Assert.True((actual = expected))

let wiki = [
  "WRITETEXT";
  "WITHIN_GROUP";
  "WITH";
  "WHILE";
  "WHERE";
  "WHEN";
  "WAITFOR";
  "VIEW";
  "VARYING";
  "VALUES";
  "USER";
  "USE";
  "UPDATETEXT";
  "UPDATE";
  "UNPIVOT";
  "UNIQUE";
  "UNION";
  "TSEQUAL";
  "TRY_CONVERT";
  "TRUNCATE";
  "TRIGGER";
  "TRANSACTION";
  "TRAN";
  "TOP";
  "TO";
  "THEN";
  "TEXTSIZE";
  "TABLESAMPLE";
  "TABLE";
  "SYSTEM_USER";
  "STATISTICS";
  "SOME";
  "SHUTDOWN";
  "SETUSER";
  "SET";
  "SESSION_USER";
  "SEMANTICSIMILARITYTABLE";
  "SEMANTICSIMILARITYDETAILSTABLE";
  "SEMANTICKEYPHRASETABLE";
  "SELECT";
  "SECURITYAUDIT";
  "SCHEMA";
  "SAVE";
  "RULE";
  "ROWGUIDCOL";
  "ROWCOUNT";
  "ROLLBACK";
  "RIGHT";
  "REVOKE";
  "REVERT";
  "RETURN";
  "RESTRICT";
  "RESTORE";
  "REPLICATION";
  "REFERENCES";
  "RECONFIGURE";
  "READTEXT";
  "READ";
  "RAISERROR";
  "PUBLIC";
  "PROCEDURE";
  "PROC";
  "PRINT";
  "PRIMARY";
  "PRECISION";
  "PLAN";
  "PIVOT";
  "PERCENT";
  "OVER";
  "OUTER";
  "ORDER";
  "OR";
  "OPTION";
  "OPENXML";
  "OPENROWSET";
  "OPENQUERY";
  "OPENDATASOURCE";
  "OPEN";
  "ON";
  "OFFSETS";
  "OFF";
  "OF";
  "NULLIF";
  "NULL";
  "NOT";
  "NONCLUSTERED";
  "NOCHECK";
  "NATIONAL";
  "MERGE";
  "LOAD";
  "LINENO";
  "LIKE";
  "LEFT";
  "KILL";
  "KEY";
  "JOIN";
  "IS";
  "INTO";
  "INTERSECT";
  "INSERT";
  "INNER";
  "INDEX";
  "IN";
  "IF";
  "IDENTITYCOL";
  "IDENTITY_INSERT";
  "IDENTITY";
  "HOLDLOCK";
  "HAVING";
  "GROUP";
  "GRANT";
  "GOTO";
  "FUNCTION";
  "FULL";
  "FROM";
  "FREETEXTTABLE";
  "FREETEXT";
  "FOREIGN";
  "FOR";
  "FILLFACTOR";
  "FILE";
  "FETCH";
  "EXTERNAL";
  "EXIT";
  "EXISTS";
  "EXECUTE";
  "EXEC";
  "EXCEPT";
  "ESCAPE";
  "ERRLVL";
  "END";
  "ELSE";
  "DUMP";
  "DROP";
  "DOUBLE";
  "DISTRIBUTED";
  "DISTINCT";
  "DISK";
  "DESC";
  "DENY";
  "DELETE";
  "DEFAULT";
  "DECLARE";
  "DEALLOCATE";
  "DBCC";
  "DATABASE";
  "CURSOR";
  "CURRENT_USER";
  "CURRENT_TIMESTAMP";
  "CURRENT_TIME";
  "CURRENT_DATE";
  "CURRENT";
  "CROSS";
  "CREATE";
  "CONVERT";
  "CONTINUE";
  "CONTAINSTABLE";
  "CONTAINS";
  "CONSTRAINT";
  "COMPUTE";
  "COMMIT";
  "COLUMN";
  "COLLATE";
  "COALESCE";
  "CLUSTERED";
  "CLOSE";
  "CHECKPOINT";
  "CHECK";
  "CASE";
  "CASCADE";
  "BY";
  "BULK";
  "BROWSE";
  "BREAK";
  "BETWEEN";
  "BEGIN";
  "BACKUP";
  "AUTHORIZATION";
  "ASC";
  "AS";
  "ANY";
  "AND";
  "ALTER";
  "ALL";
  "ADD";
]

[<Fact>]
let ``keywordTest`` () =
  let existing = keywords2
  let existingF = buildInFuncs
  let wiki = wiki
  for x in wiki do
    let lower = x.ToLower()
    if (List.contains lower existing) = false && (List.contains x existingF) = false then
      printfn "%A;" lower
  //let expected = ["select"; " "; "3"]
  //Assert.True((actual = expected))