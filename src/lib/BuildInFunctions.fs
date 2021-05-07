module BuildInFunctions
// This module contains a list of t-sql build-in functions that can be called without schema prefix.

// note that sql server stored procedures start with sp_

// See https://docs.microsoft.com/en-us/sql/t-sql/functions/functions?view=sql-server-ver15
let buildInFunctions = [
  // Aggregate
  "APPROX_COUNT_DISTINCT";
  "AVG"; // AVG(SalesYTD) OVER (PARTITION BY TerritoryID ORDER BY DATEPART(yy,ModifiedDate) ),1) AS MovingAvg  
  "CHECKSUM_AGG";
  "COUNT";
  "COUNT_BIG";
  "GROUPING";
  "GROUPING_ID";
  "MAX";
  "MIN";
  "STDEV";
  "STDEVP";
  "STRING_AGG";
  "SUM";
  "VAR";
  "VARP";

  // Analytic
  "CUME_DIST"; // CUME_DIST () OVER (PARTITION BY Department ORDER BY Rate) AS CumeDist, 
  "FIRST_VALUE";
  "LAG";
  "LAST_VALUE";
  "LEAD";
  "PERCENT_RANK";
  "PERCENTILE_CONT";
  "PERCENTILE_DISC";

  "COLLATIONPROPERTY";
  "TERTIARY_WEIGHTS";

  // Configuration
  // none

  // Conversion
  "CAST";
  "TRY_CAST";
  "CONVERT";
  "TRY_CONVERT";
  "PARSE";
  "TRY_PARSE"; 

  // Cryptographic
  "ASYMKEY_ID";
  "ASYMKEYPROPERTY";
  "CERTPROPERTY";
  "Cert_ID";
  "CRYPT_GEN_RANDOM";
  "DECRYPTBYASYMKEY";
  "DECRYPTBYCERT";
  "DECRYPTBYKEY";
  "DECRYPTBYKEYAUTOASYMKEY";
  "DecryptByKeyAutoCert";
  "DECRYPTBYPASSPHRASE";
  "ENCRYPTBYASYMKEY";
  "ENCRYPTBYCERT";
  "ENCRYPTBYKEY";
  "ENCRYPTBYPASSPHRASE";
  "HASHBYTES";
  "IS_OBJECTSIGNED";
  "KEY_GUID";
  "KEY_ID";
  "KEY_NAME";
  "SIGNBYASYMKEY";
  "SIGNBYCERT";
  "SYMKEYPROPERTY";
  "VERIGYSIGNEDBYCERT";
  "VERIFYSIGNEDBYASMKEY";

  // Certificate copying
  "CERTENCODED";
  "CERTPRIVATEKEY";
  
  // Cursor
  "CURSOR_STATUS";
  
  // Data Type
  "DATALENGTH";
  "IDENT_CURRENT";
  "IDENT_INCR";
  "IDENT_SEED";
  "IDENTITY";
  "SQL_VARIANT_PROPERTY";
  
  // Date and Time
  "SYSDATETIME"; "SYSDATETIMEOFFSET"; "SYSUTCDATETIME"; 
  "GETDATE"; "GETUTCDATE"; 
  "DATENAME"; "DATEPART"; "DAY"; "MONTH"; "YEAR"; 
  "DATEFROMPARTS"; "DATETIME2FROMPARTS"; "DATETIMEFROMPARTS"; "DATETIMEOFFSETFROMPARTS"; "SMALLDATETIMEFROMPARTS"; "TIMEFROMPARTS";
  "DATEDIFF"; "DATEDIFF_BIG";
  "DATEADD"; "EOMONTH"; "SWITCHOFFSET"; "TODATETIMEOFFSET";
  "ISDATE";
  "CURRENT_TIMEZONE";
  "CURRENT_TIMEZONE_ID";

  // Json
  "ISJSON";
  "JSON_VALUE";
  "JSON_QUERY";
  "JSON_MODIFY";

  // Mathematical
  "ABS";
  "ACOS";
  "ASIN";
  "ATAN";
  "ATN2";
  "CEILING";
  "COS";
  "COT";
  "DEGREES";
  "EXP";
  "FLOOR";
  "LOG";
  "LOG10";
  "PI";
  "POWER";
  "RADIANS";
  "RAND";
  "ROUND";
  "SIGN";
  "SIN";
  "SQRT";
  "SQUARE";
  "TAN";
  
  // Logical
  "CHOOSE";
  "GREATEST";
  "IIF";
  "LEAST";

  // Metadata
  "APP_NAME";
  "APPLOCK_MODE";
  "APPLOCK_TEST";
  "ASSEMBLYPROPERTY";
  "COL_LENGTH";
  "COL_NAME";
  "COLUMNPROPERTY";
  "DATABASE_PRINCIPAL_ID";
  "DATABASEPROPERTYEX";
  "DB_ID";
  "DB_NAME";
  "FILE_ID";
  "FILE_IDEX";
  "FILE_NAME";
  "FILEGROUP_ID";
  "FILEGROUP_NAME";
  "FILEGROUPPROPERTY";
  "FILEPROPERTY";
  "FULLTEXTCATALOGPROPERTY";
  "FULLTEXTSERVICEPROPERTY";
  "INDEX_COL";
  "INDEXKEY_PROPERTY";
  "INDEXPROPERTY";
  "NEXT VALUE FOR";  // has spaces
  "OBJECT_DEFINITION";
  "OBJECT_ID";
  "OBJECT_NAME";
  "OBJECT_SCHEMA_NAME";
  "OBJECTPROPERTY";
  "OBJECTPROPERTYEX";
  "ORIGINAL_DB_NAME";
  "PARSENAME";
  "SCHEMA_ID";
  "SCHEMA_NAME";
  "SCOPE_IDENTITY";
  "SERVERPROPERTY";
  "STATS_DATE";
  "TYPE_ID";
  "TYPE_NAME";
  "TYPEPROPERTY";
  "VERSION";

  // Ranking
  "RANK";
  "NTILE";
  "DENSE_RANK";
  "ROW_NUMBER";

  // Replication
  "PUBLISHINGSERVERNAME";

  // Rowset
  "OPENDATASOURCE";
  "OPENJSON";
  "OPENQUERY";
  "OPENROWSET";
  "OPENXML";

  // Security
  "PWDCOMPARE";
  "PWDENCRYPT";
  "SESSION_USER";
  "SUSER_ID";
  "SUSER_SID";
  "HAS_PERMS_BY_NAME";
  "SUSER_SNAME";
  "IS_MEMBER";
  "SYSTEM_USER";
  "IS_ROLEMEMBER";
  "SUSER_NAME";
  "IS_SRVROLEMEMBER";
  "USER_ID";
  "LOGINPROPERTY";
  "USER_NAME";
  "ORIGINAL_LOGIN";
  "PERMISSIONS";
  "HAS_DBACCESS";
  "SESSIONPROPERTY";

  // String
  "ASCII";
  "CHAR";
  "CHARINDEX";
  "CONCAT";
  "CONCAT_WS";
  "DIFFERENCE";
  "FORMAT";
  "LEFT";
  "LEN";
  "LOWER";
  "LTRIM";
  "NCHAR";
  "PATINDEX";
  "QUOTENAME";
  "REPLACE";
  "REPLICATE";
  "REVERSE";
  "RIGHT";
  "RTRIM";
  "SOUNDEX";
  "SPACE";
  "STR";
  "STRING_AGG";
  "STRING_ESCAPE";
  "STRING_SPLIT";
  "STUFF";
  "SUBSTRING";
  "TRANSLATE";
  "TRIM";
  "UNICODE";
  "UPPER";

  // System
  "ERROR_PROCEDURE";
  "ERROR_SEVERITY";
  "ERROR_STATE";
  "FORMATMESSAGE";
  "GET_FILESTREAM_TRANSACTION_CONTEXT";
  "GETANSINULL";
  "BINARY_CHECKSUM";
  "HOST_ID";
  "CHECKSUM";
  "HOST_NAME";
  "COMPRESS";
  "ISNULL";
  "CONNECTIONPROPERTY";
  "ISNUMERIC";
  "CONTEXT_INFO";
  "MIN_ACTIVE_ROWVERSION";
  "CURRENT_REQUEST_ID";
  "NEWID";
  "CURRENT_TRANSACTION_ID";
  "NEWSEQUENTIALID";
  "DECOMPRESS";
  "ROWCOUNT_BIG";
  "ERROR_LINE";
  "SESSION_CONTEXT";
  "ERROR_MESSAGE";
  "SESSION_ID";
  "ERROR_NUMBER";
  "XACT_STATE";

  // System Metadata
  "fn_helpcollations";
  "fn_listextendedproperty";
  "fn_servershareddrives";
  "fn_virtualfilestats";
  "fn_virtualfileservermodes";
  "fn_PageResCracker";

  // System Security (prefixed by sys.)
  "fn_builtin_permissions";
  "fn_check_object_signatures";
  "fn_get_audit_file";
  "fn_my_permissions";
  "fn_translate_permissions";

  // System Trace
  "fn_trace_geteventinfo";
  "fn_trace_getfilterinfo";
  "fn_trace_getinfo";
  "fn_trace_gettable";

  // Trigger
  "COLUMNS_UPDATED";
  "EVENTDATA";
  "TRIGGER_NESTLEVEL";
  "UPDATE";

  // Test & Image
  "TEXTPTR";
  "TEXTVALID";

  "FORMAT";
  "APPROX_COUNT_DISTINCT";
  "AVG";
  "CHECKSUM_AGG"]

// Functions that are called without ().
let propertyFunctions = ["CURRENT_TIMESTAMP"; "CURRENT_USER"]