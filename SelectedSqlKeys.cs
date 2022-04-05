﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Threading.Tasks;

namespace AnotherEdit
{
    public class selectedSqlKeys
    {

        private List<string> strDefault = new List<string>() { "" };

        private string strMsSql = @"ADD,EXTERNAL,PROCEDURE,ALL,FETCH,PUBLIC,ALTER,FILE,RAISERROR,AND,FILLFACTOR,READ,ANY,FOR,READTEXT,AS,FOREIGN,RECONFIGURE,ASC,FREETEXT,REFERENCES,AUTHORIZATION,FREETEXTTABLE,REPLICATION,BACKUP,FROM,RESTORE,BEGIN,FULL,RESTRICT,BETWEEN,FUNCTION,RETURN,BREAK,GOTO,REVERT,BROWSE,GRANT,REVOKE,BULK,GROUP,RIGHT,BY,HAVING,ROLLBACK,CASCADE,HOLDLOCK,ROWCOUNT,CASE,IDENTITY,ROWGUIDCOL,CHECK,IDENTITY_INSERT,RULE,CHECKPOINT,IDENTITYCOL,SAVE,CLOSE,IF,SCHEMA,CLUSTERED,IN,SECURITYAUDIT,COALESCE,INDEX,SELECT,COLLATE,INNER,SEMANTICKEYPHRASETABLE,COLUMN,INSERT,SEMANTICSIMILARITYDETAILSTABLE,COMMIT,INTERSECT,SEMANTICSIMILARITYTABLE,COMPUTE,INTO,SESSION_USER,CONSTRAINT,IS,SET,CONTAINS,JOIN,SETUSER,CONTAINSTABLE,KEY,SHUTDOWN,CONTINUE,KILL,SOME,CONVERT,LEFT,STATISTICS,CREATE,LIKE,SYSTEM_USER,CROSS,LINENO,TABLE,CURRENT,LOAD,TABLESAMPLE,CURRENT_DATE,MERGE,TEXTSIZE,CURRENT_TIME,NATIONAL,THEN,CURRENT_TIMESTAMP,NOCHECK,TO,CURRENT_USER,NONCLUSTERED,TOP,CURSOR,NOT,TRAN,DATABASE,NULL,TRANSACTION,DBCC,NULLIF,TRIGGER,DEALLOCATE,OF,TRUNCATE,DECLARE,OFF,TRY_CONVERT,DEFAULT,OFFSETS,TSEQUAL,DELETE,ON,UNION,DENY,OPEN,UNIQUE,DESC,OPENDATASOURCE,UNPIVOT,DISK,OPENQUERY,UPDATE,DISTINCT,OPENROWSET,UPDATETEXT,DISTRIBUTED,OPENXML,USE,DOUBLE,OPTION,USER,DROP,OR,VALUES,DUMP,ORDER,VARYING,ELSE,OUTER,VIEW,END,OVER,WAITFOR,ERRLVL,PERCENT,WHEN,ESCAPE,PIVOT,WHERE,EXCEPT,PLAN,WHILE,EXEC,PRECISION,WITH,EXECUTE,PRIMARY,WITHIN,GROUP,EXISTS,PRINT,WRITETEXT,EXIT,PROC";
        private string strMsSqlFunction = @"OPENDATASOURCE,OPENROWSET,OPENQUERY,OPENXML,AVG,MIN,CHECKSUM_AGG,SUM,COUNT,STDEV,COUNT_BIG,STDEVP,GROUPING,VAR,GROUPING_ID,VARP,MAX,RANK,NTILE,DENSE_RANK,ROW_NUMBER,@@DATEFIRST,@@OPTIONS,@@DBTS,@@REMSERVER,@@LANGID,@@SERVERNAME,@@LANGUAGE,@@SERVICENAME,@@LOCK_TIMEOUT,@@SPID,@@MAX_CONNECTIONS,@@TEXTSIZE,@@MAX_PRECISION,@@VERSION,@@NESTLEVEL,CAST,CONVERT,PARSE,TRY_CONVERT,TRY_PARSE,@@CURSOR_ROWS,CURSOR_STATUS,@@FETCH_STATUS,SYSDATETIME,SYSDATETIMEOFFSET,SYSUTCDATETIME,CURRENT_TIMESTAMP,GETDATE,GETUTCDATE,DATENAME,DATEPART,DAY,MONTH,YEAR,DATEFROMPARTS,DATETIME2FROMPARTS,DATETIMEFROMPARTS,DATETIMEOFFSETFROMPARTS,SMALLDATETIMEFROMPARTS,TIMEFROMPARTS,DATEDIFF,DATEADD,EOMONTH,SWITCHOFFSET,TODATETIMEOFFSET,@@DATEFIRST,SET DATEFIRST,SET DATEFORMAT,@@LANGUAGE,SET LANGUAGE,sp_helplanguage,ISDATE,CHOOSE,IIF,ABS,DEGREES,RAND,ACOS,EXP,ROUND,ASIN,FLOOR,SIGN,ATAN,LOG,SIN,ATN2,LOG10,SQRT,CEILING,PI,SQUARE,COS,POWER,TAN,COT,RADIANS,@@PROCID,INDEX_COL,APP_NAME,INDEXKEY_PROPERTY,APPLOCK_MODE,INDEXPROPERTY,APPLOCK_TEST,NEXT VALUE FOR,ASSEMBLYPROPERTY,OBJECT_DEFINITION,COL_LENGTH,OBJECT_ID,COL_NAME,OBJECT_NAME,COLUMNPROPERTY,OBJECT_SCHEMA_NAME,DATABASE_PRINCIPAL_ID,OBJECTPROPERTY,DATABASEPROPERTYEX,OBJECTPROPERTYEX,DB_ID,ORIGINAL_DB_NAME,DB_NAME,PARSENAME,FILE_ID,SCHEMA_ID,FILE_IDEX,SCHEMA_NAME,FILE_NAME,SCOPE_IDENTITY,FILEGROUP_ID,SERVERPROPERTY,FILEGROUP_NAME,STATS_DATE,FILEGROUPPROPERTY,TYPE_ID,FILEPROPERTY,TYPE_NAME,FULLTEXTCATALOGPROPERTY,TYPEPROPERTY,FULLTEXTSERVICEPROPERTY,CERTENCODED,PWDCOMPARE,CERTPRIVATEKEY,PWDENCRYPT,CURRENT_USER,SCHEMA_ID,DATABASE_PRINCIPAL_ID,SCHEMA_NAME,SESSION_USER,SUSER_ID,SUSER_SID,HAS_PERMS_BY_NAME,SUSER_SNAME,IS_MEMBER,SYSTEM_USER,IS_ROLEMEMBER,SUSER_NAME,IS_SRVROLEMEMBER,USER_ID,ORIGINAL_LOGIN,USER_NAME,PERMISSIONS,ASCII,LTRIM,SOUNDEX,CHAR,NCHAR,SPACE,CHARINDEX,PATINDEX,STR,CONCAT,QUOTENAME,STUFF,DIFFERENCE,REPLACE,SUBSTRING,FORMAT,REPLICATE,UNICODE,LEFT,REVERSE,UPPER,LEN,RIGHT,LOWER,RTRIM,$PARTITION,ERROR_SEVERITY,@@ERROR,ERROR_STATE,@@IDENTITY,FORMATMESSAGE,@@PACK_RECEIVED,GETANSINULL,@@ROWCOUNT,GET_FILESTREAM_TRANSACTION_CONTEXT,@@TRANCOUNT,HOST_ID,BINARY_CHECKSUM,HOST_NAME,CHECKSUM,ISNULL,CONNECTIONPROPERTY,ISNUMERIC,CONTEXT_INFO,MIN_ACTIVE_ROWVERSION,CURRENT_REQUEST_ID,NEWID,ERROR_LINE,NEWSEQUENTIALID,ERROR_MESSAGE,ROWCOUNT_BIG,ERROR_NUMBER,XACT_STATE,ERROR_PROCEDURE,@@CONNECTIONS,@@PACK_RECEIVED,@@CPU_BUSY,@@PACK_SENT,@@TIMETICKS,@@IDLE,@@TOTAL_ERRORS,@@IO_BUSY,@@TOTAL_READ,@@PACKET_ERRORS,@@TOTAL_WRITE,PATINDEX,TEXTVALID,TEXTPTR";

        // symbol | was added  for formatting purpose 
        private string strODBCSql = @"|,ABSOLUTE,EXEC,OVERLAPS,ACTION,EXECUTE,PAD,ADA,EXISTS,PARTIAL,ADD,EXTERNAL,PASCAL,ALL,EXTRACT,POSITION,ALLOCATE,FALSE,PRECISION,ALTER,FETCH,PREPARE,AND,FIRST,PRESERVE,ANY,FLOAT,PRIMARY,ARE,FOR,PRIOR,AS,FOREIGN,PRIVILEGES,ASC,FORTRAN,PROCEDURE,ASSERTION,FOUND,PUBLIC,AT,FROM,READ,AUTHORIZATION,FULL,REAL,AVG,GET,REFERENCES,BEGIN,GLOBAL,RELATIVE,BETWEEN,GO,RESTRICT,BIT,GOTO,REVOKE,BIT_LENGTH,GRANT,RIGHT,BOTH,GROUP,ROLLBACK,BY,HAVING,ROWS,CASCADE,HOUR,SCHEMA,CASCADED,IDENTITY,SCROLL,CASE,IMMEDIATE,SECOND,CAST,IN,SECTION,CATALOG,INCLUDE,SELECT,CHAR,INDEX,SESSION,CHAR_LENGTH,INDICATOR,SESSION_USER,CHARACTER,INITIALLY,SET,CHARACTER_LENGTH,INNER,SIZE,CHECK,INPUT,SMALLINT,CLOSE,INSENSITIVE,SOME,COALESCE,INSERT,SPACE,COLLATE,INT,SQL,COLLATION,INTEGER,SQLCA,COLUMN,INTERSECT,SQLCODE,COMMIT,INTERVAL,SQLERROR,CONNECT,INTO,SQLSTATE,CONNECTION,IS,SQLWARNING,CONSTRAINT,ISOLATION,SUBSTRING,CONSTRAINTS,JOIN,SUM,CONTINUE,KEY,SYSTEM_USER,CONVERT,LANGUAGE,TABLE,CORRESPONDING,LAST,TEMPORARY,COUNT,LEADING,THEN,CREATE,LEFT,TIME,CROSS,LEVEL,TIMESTAMP,CURRENT,LIKE,TIMEZONE_HOUR,CURRENT_DATE,LOCAL,TIMEZONE_MINUTE,CURRENT_TIME,LOWER,TO,CURRENT_TIMESTAMP,MATCH,TRAILING,CURRENT_USER,MAX,TRANSACTION,CURSOR,MIN,TRANSLATE,DATE,MINUTE,TRANSLATION,DAY,MODULE,TRIM,DEALLOCATE,MONTH,TRUE,DEC,NAMES,UNION,DECIMAL,NATIONAL,UNIQUE,DECLARE,NATURAL,UNKNOWN,DEFAULT,NCHAR,UPDATE,DEFERRABLE,NEXT,UPPER,DEFERRED,NO,USAGE,DELETE,NONE,USER,DESC,NOT,USING,DESCRIBE,NULL,VALUE,DESCRIPTOR,NULLIF,VALUES,DIAGNOSTICS,NUMERIC,VARCHAR,DISCONNECT,OCTET_LENGTH,VARYING,DISTINCT,OF,VIEW,DOMAIN,ON,WHEN,DOUBLE,ONLY,WHENEVER,DROP,OPEN,WHERE,ELSE,OPTION,WITH,END,OR,WORK,END-EXEC,ORDER,WRITE,ESCAPE,OUTER,YEAR,EXCEPT,OUTPUT,ZONE,EXCEPTION";

        private string strFuturedSql = @"ABSOLUTE,HOST,RELATIVE,ACTION,HOUR,RELEASE,ADMIN,IGNORE,RESULT,AFTER,IMMEDIATE,RETURNS,AGGREGATE,INDICATOR,ROLE,ALIAS,INITIALIZE,ROLLUP,ALLOCATE,INITIALLY,ROUTINE,ARE,INOUT,ROW,ARRAY,INPUT,ROWS,ASENSITIVE,INT,SAVEPOINT,ASSERTION,INTEGER,SCROLL,ASYMMETRIC,INTERSECTION,SCOPE,AT,INTERVAL,SEARCH,ATOMIC,ISOLATION,SECOND,BEFORE,ITERATE,SECTION,BINARY,LANGUAGE,SENSITIVE,BIT,LARGE,SEQUENCE,BLOB,LAST,SESSION,BOOLEAN,LATERAL,SETS,BOTH,LEADING,SIMILAR,BREADTH,LESS,SIZE,CALL,LEVEL,SMALLINT,CALLED,LIKE_REGEX,SPACE,CARDINALITY,LIMIT,SPECIFIC,CASCADED,LN,SPECIFICTYPE,CAST,LOCAL,SQL,CATALOG,LOCALTIME,SQLEXCEPTION,CHAR,LOCALTIMESTAMP,SQLSTATE,CHARACTER,LOCATOR,SQLWARNING,CLASS,MAP,START,CLOB,MATCH,STATE,COLLATION,MEMBER,STATEMENT,COLLECT,METHOD,STATIC,COMPLETION,MINUTE,STDDEV_POP,CONDITION,MOD,STDDEV_SAMP,CONNECT,MODIFIES,STRUCTURE,CONNECTION,MODIFY,SUBMULTISET,CONSTRAINTS,MODULE,SUBSTRING_REGEX,CONSTRUCTOR,MONTH,SYMMETRIC,CORR,MULTISET,SYSTEM,CORRESPONDING,NAMES,TEMPORARY,COVAR_POP,NATURAL,TERMINATE,COVAR_SAMP,NCHAR,THAN,CUBE,NCLOB,TIME,CUME_DIST,NEW,TIMESTAMP,CURRENT_CATALOG,NEXT,TIMEZONE_HOUR,CURRENT_DEFAULT_TRANSFORM_GROUP,NO,TIMEZONE_MINUTE,CURRENT_PATH,NONE,TRAILING,CURRENT_ROLE,NORMALIZE,TRANSLATE_REGEX,CURRENT_SCHEMA,NUMERIC,TRANSLATION,CURRENT_TRANSFORM_GROUP_FOR_TYPE,OBJECT,TREAT,CYCLE,OCCURRENCES_REGEX,TRUE,DATA,OLD,UESCAPE,DATE,ONLY,UNDER,DAY,OPERATION,UNKNOWN,DEC,ORDINALITY,UNNEST,DECIMAL,OUT,USAGE,DEFERRABLE,OVERLAY,USING,DEFERRED,OUTPUT,VALUE,DEPTH,PAD,VAR_POP,DEREF,PARAMETER,VAR_SAMP,DESCRIBE,PARAMETERS,VARCHAR,DESCRIPTOR,PARTIAL,VARIABLE,DESTROY,PARTITION,WHENEVER,DESTRUCTOR,PATH,WIDTH_BUCKET,DETERMINISTIC,POSTFIX,WITHOUT,DICTIONARY,PREFIX,WINDOW,DIAGNOSTICS,PREORDER,WITHIN,DISCONNECT,PREPARE,WORK,DOMAIN,PERCENT_RANK,WRITE,DYNAMIC,PERCENTILE_CONT,XMLAGG,EACH,PERCENTILE_DISC,XMLATTRIBUTES,ELEMENT,POSITION_REGEX,XMLBINARY,END-EXEC,PRESERVE,XMLCAST,EQUALS,PRIOR,XMLCOMMENT,EVERY,PRIVILEGES,XMLCONCAT,EXCEPTION,RANGE,XMLDOCUMENT,FALSE,READS,XMLELEMENT,FILTER,REAL,XMLEXISTS,FIRST,RECURSIVE,XMLFOREST,FLOAT,REF,XMLITERATE,FOUND,REFERENCING,XMLNAMESPACES,FREE,REGR_AVGX,XMLPARSE,FULLTEXTTABLE,REGR_AVGY,XMLPI,FUSION,REGR_COUNT,XMLQUERY,GENERAL,REGR_INTERCEPT,XMLSERIALIZE,GET,REGR_R2,XMLTABLE,GLOBAL,REGR_SLOPE,XMLTEXT,GO,REGR_SXX,XMLVALIDATE,GROUPING,REGR_SXY,YEAR,HOLD,REGR_SYY,ZONE";

        private string strCompactSql = @"@@IDENTITY,ENCRYPTION,ORDER,ADD,END,OUTER,ALL,ERRLVL,OVER,ALTER,ESCAPE,PERCENT,AND,EXCEPT,PLAN,ANY,EXEC,PRECISION,AS,EXECUTE,PRIMARY,ASC,EXISTS,PRINT,AUTHORIZATION,EXIT,PROC,AVG,EXPRESSION,PROCEDURE,BACKUP,FETCH,PUBLIC,BEGIN,FILE,RAISERROR,BETWEEN,FILLFACTOR,READ,BREAK,FOR,READTEXT,BROWSE,FOREIGN,RECONFIGURE,BULK,FREETEXT,REFERENCES,BY,FREETEXTTABLE,REPLICATION,CASCADE,FROM,RESTORE,CASE,FULL,RESTRICT,CHECK,FUNCTION,RETURN,CHECKPOINT,GOTO,REVOKE,CLOSE,GRANT,RIGHT,CLUSTERED,GROUP,ROLLBACK,COALESCE,HAVING,ROWCOUNT,COLLATE,HOLDLOCK,ROWGUIDCOL,COLUMN,IDENTITY,RULE,COMMIT,IDENTITY_INSERT,SAVE,COMPUTE,IDENTITYCOL,SCHEMA,CONSTRAINT,IF,SELECT,CONTAINS,IN,SESSION_USER,CONTAINSTABLE,INDEX,SET,CONTINUE,INNER,SETUSER,CONVERT,INSERT,SHUTDOWN,COUNT,INTERSECT,SOME,CREATE,INTO,STATISTICS,CROSS,IS,SUM,CURRENT,JOIN,SYSTEM_USER,CURRENT_DATE,KEY,TABLE,CURRENT_TIME,KILL,TEXTSIZE,CURRENT_TIMESTAMP,LEFT,THEN,CURRENT_USER,LIKE,TO,CURSOR,LINENO,TOP,DATABASE,LOAD,TRAN,DATABASEPASSWORD,MAX,TRANSACTION,DATEADD,MIN,TRIGGER,DATEDIFF,NATIONAL,TRUNCATE,DATENAME,NOCHECK,TSEQUAL,DATEPART,NONCLUSTERED,UNION,DBCC,NOT,UNIQUE,DEALLOCATE,NULL,UPDATE,DECLARE,NULLIF,UPDATETEXT,DEFAULT,OF,USE,DELETE,OFF,USER,DENY,OFFSETS,VALUES,DESC,ON,VARYING,DISK,OPEN,VIEW,DISTINCT,OPENDATASOURCE,WAITFOR,DISTRIBUTED,OPENQUERY,WHEN,DOUBLE,OPENROWSET,WHERE,DROP,OPENXML,WHILE,DUMP,OPTION,WITH,ELSE,OR,WRITETEXT";
        private string strCompactFunction = @"AVGAVG,COUNT,MAX,MIN,SUM,DATEADD,DATEDIFF,DATENAME,DATEPART,GETDATE,ABS,ACOS,ASIN,ATAN,ATN2,CEILING,COS,COT,DEGREES,EXP,FLOOR,LOG,LOG10,PI,POWER,RADIANS,RAND,ROUND,SIGN,SIN,SQRT,TAN,NCHAR,CHARINDEX,LEN,LOWER,LTRIM_lce_ltrim,PATINDEX,REPLACE,REPLICATE,RTRIM,SPACE_lce_space,STR,STUFF,SUBSTRING,UNICODE,UPPER_lce_upper,@@IDENTITY,COALESCE,DATALENGTH";

        private string strMySql = @"ACCESSIBLE,ADD,ALL,ALTER,ANALYZE,AND,AS,ASC,ASENSITIVE,BEFORE,BETWEEN,BIGINT,BINARY,BLOB,BOTH,BY,CALL,CASCADE,CASE,CHANGE,CHAR,CHARACTER,CHECK,COLLATE,COLUMN,CONDITION,CONSTRAINT,CONTINUE,CONVERT,CREATE,CROSS,CURRENT_DATE,CURRENT_TIME,CURRENT_TIMESTAMP,CURRENT_USER,CURSOR,DATABASE,DATABASES,DAY_HOUR,DAY_MICROSECOND,DAY_MINUTE,DAY_SECOND,DEC,DECIMAL,DECLARE,DEFAULT,DELAYED,DELETE,DESC,DESCRIBE,DETERMINISTIC,DISTINCT,DISTINCTROW,DIV,DOUBLE,DROP,DUAL,EACH,ELSE,ELSEIF,ENCLOSED,ESCAPED,EXISTS,EXIT,EXPLAIN,FALSE,FETCH,FLOAT,FLOAT4,FLOAT8,FOR,FORCE,FOREIGN,FROM,FULLTEXT,GRANT,GROUP,HAVING,HIGH_PRIORITY,HOUR_MICROSECOND,HOUR_MINUTE,HOUR_SECOND,IF,IGNORE,IN,INDEX,INFILE,INNER,INOUT,INSENSITIVE,INSERT,INT,INT1,INT2,INT3,INT4,INT8,INTEGER,INTERVAL,INTO,IS,ITERATE,JOIN,KEY,KEYS,KILL,LEADING,LEAVE,LEFT,LIKE,LIMIT,LINEAR,LINES,LOAD,LOCALTIME,LOCALTIMESTAMP,LOCK,LONG,LONGBLOB,LONGTEXT,LOOP,LOW_PRIORITY,MASTER_SSL_VERIFY_SERVER_CERT,MATCH,MAXVALUE,MEDIUMBLOB,MEDIUMINT,MEDIUMTEXT,MIDDLEINT,MINUTE_MICROSECOND,MINUTE_SECOND,MOD,MODIFIES,NATURAL,NOT,NO_WRITE_TO_BINLOG,NULL,NUMERIC,ON,OPTIMIZE,OPTION,OPTIONALLY,OR,ORDER,OUT,OUTER,OUTFILE,PRECISION,PRIMARY,PROCEDURE,PURGE,RANGE,READ,READS,READ_WRITE,REAL,REFERENCES,REGEXP,RELEASE,RENAME,REPEAT,REPLACE,REQUIRE,RESIGNAL,RESTRICT,RETURN,REVOKE,RIGHT,RLIKE,SCHEMA,SCHEMAS,SECOND_MICROSECOND,SELECT,SENSITIVE,SEPARATOR,SET,SHOW,SIGNAL,SMALLINT,SPATIAL,SPECIFIC,SQL,SQLEXCEPTION,SQLSTATE,SQLWARNING,SQL_BIG_RESULT,SQL_CALC_FOUND_ROWS,SQL_SMALL_RESULT,SSL,STARTING,STRAIGHT_JOIN,TABLE,TERMINATED,THEN,TINYBLOB,TINYINT,TINYTEXT,TO,TRAILING,TRIGGER,TRUE,UNDO,UNION,UNIQUE,UNLOCK,UNSIGNED,UPDATE,USAGE,USE,USING,UTC_DATE,UTC_TIME,UTC_TIMESTAMP,VALUES,VARBINARY,VARCHAR,VARCHARACTER,VARYING,WHEN,WHERE,WHILE,WITH,WRITE,XOR,YEAR_MONTH,ZEROFILL,GENERAL,IGNORE_SERVER_IDS,MASTER_HEARTBEAT_PERIOD,MAXVALUE,RESIGNAL,SIGNAL,SLOW";
        private string strMySqlFunction = @"ABS,ACOS,ADDDATE,ADDTIME,AES_DECRYPT,AES_ENCRYPT,AND,ASCII,ASIN,ATAN2,ATAN,ATAN,AVG,BENCHMARK,BETWEEN,BIN,BINARY,BIT_AND,BIT_COUNT,BIT_LENGTH,BIT_OR,BIT_XOR,CASE,CAST,CEIL,CEILING,CHAR_LENGTH,CHAR,CHARACTER_LENGTH,CHARSET,COALESCE,COERCIBILITY,COLLATION,COMPRESS,CONCAT_WS,CONCAT,CONNECTION_ID,CONV,CONVERT_TZ,CONVERT,COS,COT,COUNT(DISTINCT),COUNT,CRC32,CURDATE,CURRENT_DATE,CURRENT_TIME,CURRENT_TIMESTAMP,CURRENT_USER,CURTIME,DATABASE,DATE_ADD,DATE_FORMAT,DATE_SUB,DATE,DATEDIFF,DAY,DAYNAME,DAYOFMONTH,DAYOFWEEK,DAYOFYEAR,DECODE,DEFAULT,DEGREES,DES_DECRYPT,DES_ENCRYPT,DIV,ELT,ENCODE,ENCRYPT,EXP,EXPORT_SET,EXTRACT,FIELD,FIND_IN_SET,FLOOR,FORMAT,FOUND_ROWS,FROM_DAYS,FROM_UNIXTIME,GET_FORMAT,GET_LOCK,GREATEST,GROUP_CONCAT,HEX,HOUR,IF,IFNULL,IN,INET_ATON,INET_NTOA,INSERT,INSTR,INTERVAL,IS_FREE_LOCK,IS NOT NULL,IS NOT,IS NULL,IS_USED_LOCK,IS,ISNULL,LAST_DAY,LAST_INSERT_ID,LCASE,LEAST,LEFT,LENGTH,LIKE,LN,LOAD_FILE,LOCALTIME,LOCALTIME,LOCALTIMESTAMP,LOCALTIMESTAMP,LOCATE,LOG10,LOG2,LOG,LOWER,LPAD,LTRIM,MAKE_SET,MAKEDATE,MAKETIME,MASTER_POS_WAIT,MATCH,MAX,MD5,MICROSECOND,MID,MIN,MINUTE,MOD,MONTH,MONTHNAME,NAME_CONST,NOT BETWEEN,REGEXP,NOT,NOW,NULLIF,OCT,OCTET_LENGTH,OLD_PASSWORD,OR,ORD,PASSWORD,PERIOD_ADD,PERIOD_DIFF,PI,POSITION,POW,POWER,PROCEDURE ANALYSE,QUARTER,QUOTE,RADIANS,RAND,REGEXP,RELEASE_LOCK,REPEAT,REPLACE,REVERSE,RIGHT,RLIKE,ROUND,ROW_COUNT,RPAD,RTRIM,SCHEMA,SEC_TO_TIME,SECOND,SESSION_USER,SHA,SIGN,SIN,SLEEP,SOUNDEX,SOUNDS LIKE,SPACE,SQRT,STD,STDDEV_POP,STDDEV_SAMP,STDDEV,STR_TO_DATE,STRCMP,SUBDATE,SUBSTR,SUBSTRING_INDEX,SUBSTRING,SUBTIME,SUM,SYSDATE,SYSTEM_USER,TAN,TIME_FORMAT,TIME_TO_SEC,TIME,TIMEDIFF,TIMESTAMP,TIMESTAMPADD,TIMESTAMPDIFF,TO_DAYS,TRIM,TRUNCATE,UCASE,UNCOMPRESS,UNCOMPRESSED_LENGTH,UNHEX,UNIX_TIMESTAMP,UPPER,USER,UTC_DATE,UTC_TIME,UTC_TIMESTAMP,UUID,VALUES,VAR_POP,VAR_SAMP,VARIANCE,VERSION,WEEK,WEEKDAY,WEEKOFYEAR,XOR,YEAR,YEARWEEK";

        private string strDB2Sql = @"ACTIVATE,DOCUMENT,LOCK,ROUND_CEILING,ADD,DOUBLE,LOCKMAX,ROUND_DOWN,AFTER,DROP,LOCKSIZE,ROUND_FLOOR,ALIAS,DSSIZE,LONG,ROUND_HALF_DOWN,ALL,DYNAMIC,LOOP,ROUND_HALF_EVEN,ALLOCATE,EACH,MAINTAINED,ROUND_HALF_UP,ALLOW,EDITPROC,MATERIALIZED,ROUND_UP,ALTER,ELSE,MAXVALUE,ROUTINE,AND,ELSEIF,MICROSECOND,ROW,ANY,ENABLE,MICROSECONDS,ROW_NUMBER,AS,ENCODING,MINUTE,ROWNUMBER,ASENSITIVE,ENCRYPTION,MINUTES,ROWS,ASSOCIATE,END,MINVALUE,ROWSET,ASUTIME,END-EXEC,MODE,RRN,AT,ENDING,MODIFIES,RUN,ATTRIBUTES,ERASE,MONTH,SAVEPOINT,AUDIT,ESCAPE,MONTHS,SCHEMA,AUTHORIZATION,EVERY,NAN,SCRATCHPAD,AUX,EXCEPT,NEW,SCROLL,AUXILIARY,EXCEPTION,NEW_TABLE,SEARCH,BEFORE,EXCLUDING,NEXTVAL,SECOND,BEGIN,EXCLUSIVE,NO,SECONDS,BETWEEN,EXECUTE,NOCACHE,SECQTY,BINARY,EXISTS,NOCYCLE,SECURITY,BUFFERPOOL,EXIT,NODENAME,SELECT,BY,EXPLAIN,NODENUMBER,SENSITIVE,CACHE,EXTERNAL,NOMAXVALUE,SEQUENCE,CALL,EXTRACT,NOMINVALUE,SESSION,CALLED,FENCED,NONE,SESSION_USER,CAPTURE,FETCH,NOORDER,SET,CARDINALITY,FIELDPROC,NORMALIZED,SIGNAL,CASCADED,FILE,NOT,SIMPLE,CASE,FINAL,NULL,SNAN,CAST,FOR,NULLS,SOME,CCSID,FOREIGN,NUMPARTS,SOURCE,CHAR,FREE,OBID,SPECIFIC,CHARACTER,FROM,OF,SQL,CHECK,FULL,OLD,SQLID,CLONE,FUNCTION,OLD_TABLE,STACKED,CLOSE,GENERAL,ON,STANDARD,CLUSTER,GENERATED,OPEN,START,COLLECTION,GET,OPTIMIZATION,STARTING,COLLID,GLOBAL,OPTIMIZE,STATEMENT,COLUMN,GO,OPTION,STATIC,COMMENT,GOTO,OR,STATMENT,COMMIT,GRANT,ORDER,STAY,CONCAT,GRAPHIC,OUT,STOGROUP,CONDITION,GROUP,OUTER,STORES,CONNECT,HANDLER,OVER,STYLE,CONNECTION,HASH,OVERRIDING,SUBSTRING,CONSTRAINT,HASHED_VALUE,PACKAGE,SUMMARY,CONTAINS,HAVING,PADDED,SYNONYM,CONTINUE,HINT,PAGESIZE,SYSFUN,COUNT,HOLD,PARAMETER,SYSIBM,COUNT_BIG,HOUR,PART,SYSPROC,CREATE,HOURS,PARTITION,SYSTEM,CROSS,IDENTITY,PARTITIONED,SYSTEM_USER,CURRENT,IF,PARTITIONING,TABLE,CURRENT_DATE,IMMEDIATE,PARTITIONS,TABLESPACE,CURRENT_LC_CTYPE,IN,PASSWORD,THEN,CURRENT_PATH,INCLUDING,PATH,TIME,CURRENT_SCHEMA,INCLUSIVE,PIECESIZE,TIMESTAMP,CURRENT_SERVER,INCREMENT,PLAN,TO,CURRENT_TIME,INDEX,POSITION,TRANSACTION,CURRENT_TIMESTAMP,INDICATOR,PRECISION,TRIGGER,CURRENT_TIMEZONE,INF,PREPARE,TRIM,CURRENT_USER,INFINITY,PREVVAL,TRUNCATE,CURSOR,INHERIT,PRIMARY,TYPE,CYCLE,INNER,PRIQTY,UNDO,DATA,INOUT,PRIVILEGES,UNION,DATABASE,INSENSITIVE,PROCEDURE,UNIQUE,DATAPARTITIONNAME,INSERT,PROGRAM,UNTIL,DATAPARTITIONNUM,INTEGRITY,PSID,UPDATE,DATE,INTERSECT,PUBLIC,USAGE,DAY,INTO,QUERY,USER,DAYS,IS,QUERYNO,USING,DB2GENERAL,ISOBID,RANGE,VALIDPROC,DB2GENRL,ISOLATION,RANK,VALUE,DB2SQL,ITERATE,READ,VALUES,DBINFO,JAR,READS,VARIABLE,DBPARTITIONNAME,JAVA,RECOVERY,VARIANT,DBPARTITIONNUM,JOIN,REFERENCES,VCAT,DEALLOCATE,KEEP,REFERENCING,VERSION,DECLARE,KEY,REFRESH,VIEW,DEFAULT,LABEL,RELEASE,VOLATILE,DEFAULTS,LANGUAGE,RENAME,VOLUMES,DEFINITION,LATERAL,REPEAT,WHEN,DELETE,LC_CTYPE,RESET,WHENEVER,DENSE_RANK,LEAVE,RESIGNAL,WHERE,DENSERANK,LEFT,RESTART,WHILE,DESCRIBE,LIKE,RESTRICT,WITH,DESCRIPTOR,LINKTYPE,RESULT,WITHOUT,DETERMINISTIC,LOCAL,RESULT_SET_LOCATOR,WLM,DIAGNOSTICS,LOCALDATE,RETURN,WRITE,DISABLE,LOCALE,RETURNS,XMLELEMENT,DISALLOW,LOCALTIME,REVOKE,XMLEXISTS,DISCONNECT,LOCALTIMESTAMP,RIGHT,XMLNAMESPACES,DISTINCT,LOCATOR,ROLE,YEAR,DO,LOCATORS,ROLLBACK,YEAR";
        private string strDB2SqlFunction = @"ABS,GROUPING,REGR_INTERCEPT,ARE,INT,REGR_R2,ARRAY,INTEGER,REGR_SLOPE,ASYMMETRIC,INTERSECTION,REGR_SXX,ATOMIC,INTERVAL,REGR_SXY,AVG,LARGE,REGR_SYY,BIGINT,LEADING,ROLLUP,BLOB,LN,SCOPE,BOOLEAN,LOWER,SIMILAR,BOTH,MATCH,SMALLINT,CEIL,MAX,SPECIFICTYPE,CEILING,MEMBER,SQLEXCEPTION,CHAR_LENGTH,MERGE,SQLSTATE,CHARACTER_LENGTH,METHOD,SQLWARNING,CLOB,MIN,SQRT,COALESCE,MOD,STDDEV_POP,COLLATE,MODULE,STDDEV_SAMP,COLLECT,MULTISET,SUBMULTISET,CONVERT,NATIONAL,SUM,CORR,NATURAL,SYMMETRIC,CORRESPONDING,NCHAR,TABLESAMPLE,COVAR_POP,NCLOB,TIMEZONE_HOUR,COVAR_SAMP,NORMALIZE,TIMEZONE_MINUTE,CUBE,NULLIF,TRAILING,CUME_DIST,NUMERIC,TRANSLATE,CURRENT_DEFAULT_TRANSFORM_GROUP,OCTET_LENGTH,TRANSLATION,CURRENT_ROLE,ONLY,TREAT,CURRENT_TRANSFORM_GROUP_FOR_TYPE,OVERLAPS,TRUE,DEC,OVERLAY,UESCAPE,DECIMAL,PERCENT_RANK,UNKNOWN,DEREF,PERCENTILE_CONT,UNNEST,ELEMENT,PERCENTILE_DISC,UPPER,EXEC,POWER,VAR_POP,EXP,REAL,VAR_SAMP,FALSE,RECURSIVE,VARCHAR,FILTER,REF,VARYING,FLOAT,REGR_AVGX,WIDTH_BUCKET,FLOOR,REGR_AVGY,WINDOW,FUSION,REGR_COUNT,WITHIN";


        //       private string strFindLineBefore = @" GO?☼GO☼║ SELECT ?☼SELECT ║ FROM ?☼FROM ║ ORDER BY ?☼ORDER BY ║ USE ?☼USE ║ WHERE ?☼WHERE ║ INNER ?☼INNER ║ LEFT ?☼LEFT ║ RIGHT ?☼RIGHT ║ UNION ?☼☼UNION☼☼║ IF ?☼IF ║ ALTER ?☼ALTER ║),(?),☼(║([?(☼[║,[?,☼[║ GROUP ?☼GROUP ║, [?,☼ [║ HAVING ?☼HAVING ║ DELETE ?☼DELETE ║ UPDATE ?☼UPDATE ║ INSERT ?☼INSERT ║ DROP ?☼DROP ║ CREATE ?☼CREATE ║ OUTPUT ?☼OUTPUT ║ COMMIT ?☼COMMIT║ COMMIT ?☼COMMIT║ EXEC?☼EXEC║ VALUES?☼VALUES☼║ DEFAULT ?☼DEFAULT ║ COLLATE ?☼COLLATE ║ BULK ?☼BULK ║ EXCEPT ?☼☼EXCEPT☼☼║ INTERSECT?☼☼INTERSECT ║ DECLARE ?☼DECLARE ║ SET ?☼SET ║ PRIMARY ?☼PRIMARY ║ FILENAME ?☼FILENAME ║ FILEGROUP ?☼FILEGROUP ║ LOG ?☼LOG ║)WITH ?)☼WITH ║)WITH(?)☼WITH( ║ ON,? ON,☼║ ON ?☼ ON ║ AND ?☼ AND ║ OR ?☼ OR ║)ON?☼)ON║]([?](☼[║,[?,☼[║ OFF,? OFF,☼║ CONSTRAINT ?☼ CONSTRAINT ║ CASE ?☼ CASE ║ WHEN ?☼ WHEN ║ THEN ?☼ THEN ║ END ?☼ END ";  //║ INTO ║ OPTION ║
        private string strFindLineBefore = @" GO ?☼ GO ☼║ SELECT ?☼ SELECT ☼ ║ PROCEDURE ? ☼ PROCEDURE ║ BEGIN ? ☼ BEGIN ║ FROM ?☼ FROM ║ ORDER BY ?☼ ORDER BY ║ USE ?☼ USE ║ WHERE ?☼ WHERE ║ INNER ?☼ INNER ║ LEFT OUTER ?☼ LEFT OUTER ║ RIGHT OUTER ?☼ RIGHT OUTER ║ INNER JOIN ?☼ INNER JOIN ║ UNION ?☼ UNION ║ IF ?☼ IF ║ ALTER ?☼ ALTER ║ ) , (? ) ,☼ (║ ( [ ? (☼ [ ║ GROUP ? ☼ GROUP ║ HAVING ?☼ HAVING ║ DELETE ?☼ DELETE ║ UPDATE ?☼ UPDATE ║ INSERT ?☼ INSERT ║ DROP ?☼ DROP ║ CREATE ?☼ CREATE ║ OUTPUT ?☼ OUTPUT ║ COMMIT ?☼ COMMIT ║ EXEC ?☼ EXEC ║ VALUES ?☼ VALUES ☼║ DEFAULT ?☼ DEFAULT ║ COLLATE ?☼ COLLATE ║ BULK ?☼ BULK ║ EXCEPT ?☼☼ EXCEPT ☼☼║ INTERSECT ?☼ INTERSECT ║ ECLARE ?☼ DECLARE ║ SET ?☼ SET ║ PRIMARY ?☼ PRIMARY ║ FILENAME ?☼ FILENAME ║ FILEGROUP ?☼ FILEGROUP ║ LOG ?☼ LOG ║ ) WITH ( ? )☼ WITH ( ║ ON ,? ON ,☼ ║ AND ?☼ AND ║ OR ?☼ OR ║ ) ON ?☼ ) ON ║ ] ( [ ? ] ( ☼ [ ║ OFF , ? OFF , ☼║ CONSTRAINT ?☼ CONSTRAINT ║ CASE ?☼ CASE ║ WHEN ?☼ WHEN ║ THEN ?☼ THEN ║ END ?☼ END ☼║ ] ] ? ] ☼ ] ║ ] [ ? ] ☼ [ ║ } ] ? } ☼] ║|?☼ |";  //║ INTO ║ OPTION ║


        public List<string> listSqlReserved
        {
            get
            {
                return strDefault;
            }
        }

        public List<string> newLineBefore {
            get {
                return strFindLineBefore.Split('║').ToList();
            }
        }


        public selectedSqlKeys()
        {
            strDefault = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="strDB"></param>
        /// <param name="blnODBC"></param>
        /// <param name="blnFunction"></param>
        public selectedSqlKeys(string connStr, string strDB, bool blnODBC, bool blnFunction)
        {
            if (strDB.ToUpper().IndexOf("SQLCLIENT")>=0 || connStr.IndexOf("SQL")>=0) strDefault = (strMsSql +(blnFunction ? "," + strMsSqlFunction:""))  .Split(',').ToList();
            if (strDB.ToUpper().IndexOf("COMPACT")>=0) strDefault = (strCompactSql + (blnFunction ? "," + strCompactFunction : "")).Split(',').ToList();
            if (strDB.ToUpper().IndexOf("MYSQL")>=0) strDefault = (strMySql + (blnFunction ? "," + strMySqlFunction : "")).Split(',').ToList();
           // if (strDB == "FUTURED") strDefault = strFuturedSql.Split(',').ToList();
            if (strDB.ToUpper().IndexOf("ODBC") >= 0 || strDB.ToUpper().IndexOf("OLEDB") >= 0) strDefault = (strODBCSql + (blnFunction ? "," + strMsSqlFunction : "")).Split(',').ToList();
            if (strDB.ToUpper().IndexOf("IBM") >=0) strDefault = (strDB2Sql + (blnFunction ? "," + strDB2SqlFunction : "")).Split(',').ToList();


            if (blnODBC && !(strDB.ToUpper().IndexOf("ODBC") >= 0 || strDB.ToUpper().IndexOf("OLEDB") >= 0))
            {
                List<string> listStr = strODBCSql.Split(',').ToList();
                Parallel.For(0, listStr.Count, (int i) =>
                {
                    if (!strDefault.Contains(listStr[i])) strDefault.Add(listStr[i]);

               }); 
            }
        }
    }
}