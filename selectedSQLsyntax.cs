using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnotherEdit
{
    class selectedSQLsyntax
    {
        private string strSelectSQLMain = //"[ WITH <common_table_expression>]  \n\r" +
                                          "SELECT *  \n\r" +
                                          "  FROM    \n\r" +   
                                          "WHERE     \n\r" +
                                          "GROUP BY  \n\r" +
                                          "HAVING    \n\r" +
                                          "ORDER BY  \n\r  ASC  DESC  \n\r" +
                                          "  \n\r" +
                                          "The UNION, EXCEPT and INTERSECT operators can be used between queries to combine or compare their results into one result set.  \n\r";
        private string strSelectSQLFull =
                                        "SELECT statement ::=  \n\r" +
                                        "< query_expression >  \n\r" +
                                        "[ ORDER BY { order_by_expression | column_position [ ASC | DESC ] } \n\r" +
                                        "[ ,...n ]    ] \n\r" +
                                        "[ COMPUTE  \n\r" +
                                        "{ { AVG | COUNT | MAX | MIN | SUM } ( expression ) } [ ,...n ]  \n\r" +
                                        "[ BY expression [ ,...n ] ]  \n\r" +
                                        "]  \n\r" +
                                        "[ FOR { BROWSE | XML { RAW | AUTO | EXPLICIT }  \n\r" +
                                        "[ , XMLDATA ]  \n\r" +
                                        "[ , ELEMENTS ] \n\r" +
                                        "[ , BINARY base64 ] \n\r" +
                                        "}  \n\r" +
                                        "]  \n\r" +
                                        "[ OPTION ( < query_hint > [ ,...n ]) ]  \n\r" +
                                        "\n\r" +
                                        "< query expression > ::=  \n\r" +
                                        "{ < query specification > | ( < query expression > ) }  \n\r" +
                                        "[ UNION [ ALL ] < query specification | ( < query expression > ) [...n ] ]  \n\r" +
                                        "\n\r" +
                                        "< query specification > ::=  \n\r" +
                                        "SELECT [ ALL | DISTINCT ]  \n\r" +
                                        "[ { TOP integer | TOP integer PERCENT } [ WITH TIES ] ]  \n\r" +
                                        "< select_list >  \n\r" +
                                        "[ INTO new_table ]  \n\r" +
                                        "[ FROM { < table_source > } [ ,...n ] ]  \n\r" +
                                        "[ WHERE < search_condition > ]  \n\r" +
                                        "[ GROUP BY [ ALL ] group_by_expression [ ,...n ]  \n\r" +
                                        "[ WITH { CUBE | ROLLUP } ] \n\r" +
                                        "] \n\r" +
                                        "[ HAVING < search_condition > ]  \n\r";

        
        private string strDeleteSQLMain =
                                         "DELETE \n\r" +
                                         " [ FROM ] \n\r" +
                                         " { table_name WITH ( < table_hint_limited > [ ...n ] ) \n\r" +
                                         " | view_name \n\r" +
                                         " | rowset_function_limited \n\r" +
                                         " } \n\r" +
                                         "\n\r" +
                                         " [ FROM { < table_source > } [ ,...n ] ] \n\r" +
                                         "\n\r" +
                                         " [ WHERE \n\r" +
                                         " { < search_condition > \n\r" +
                                         " | { [ CURRENT OF \n\r" +
                                         " { { [ GLOBAL ] cursor_name } \n\r" +
                                         " | cursor_variable_name \n\r" +
                                         " } \n\r" +
                                         " ] }\n\r" +
                                         " } \n\r" +
                                         " ] \n\r" +
                                         " [ OPTION ( < query_hint > [ ,...n ] ) ] \n\r";

        private string strDeleteSQLFull =
                                        "DELETE \n\r" +
                                        " [ FROM ] \n\r" +
                                        " { table_name WITH ( < table_hint_limited > [ ...n ] ) \n\r" +
                                        " | view_name \n\r" +
                                        " | rowset_function_limited \n\r" +
                                        " } \n\r" +
                                        " \n\r" +
                                        " [ FROM { < table_source > } [ ,...n ] ] \n\r" +
                                        " \n\r" +
                                        " [ WHERE \n\r" +
                                        " { < search_condition > \n\r" +
                                        " | { [ CURRENT OF \n\r" +
                                        " { { [ GLOBAL ] cursor_name } \n\r" +
                                        " | cursor_variable_name \n\r" +
                                        " } \n\r" +
                                        " ] } \n\r" +
                                        " } \n\r" +
                                        " ] \n\r" +
                                        " [ OPTION ( < query_hint > [ ,...n ] ) ] \n\r" +
                                        " \n\r" +
                                        "< table_source > ::= \n\r" +
                                        " table_name [ [ AS ] table_alias ] [ WITH ( < table_hint > [ ,...n ] ) ] \n\r" +
                                        " | view_name [ [ AS ] table_alias ] \n\r" +
                                        " | rowset_function [ [ AS ] table_alias ] \n\r" +
                                        " | derived_table [ AS ] table_alias [ ( column_alias [ ,...n ] ) ] \n\r" +
                                        " | < joined_table > \n\r" +
                                        " \n\r" +
                                        "< joined_table > ::= \n\r" +
                                        " < table_source > < join_type > < table_source > ON < search_condition > \n\r" +
                                        " | < table_source > CROSS JOIN < table_source > \n\r" +
                                        " | < joined_table > \n\r" +
                                        " \n\r" +
                                        "< join_type > ::= \n\r" +
                                        " [ INNER | { { LEFT | RIGHT | FULL } [OUTER] } ] \n\r" +
                                        " [ < join_hint > ] \n\r" +
                                        " JOIN \n\r" +
                                        " \n\r" +
                                        "< table_hint_limited > ::= \n\r" +
                                        " { FASTFIRSTROW \n\r" +
                                        " | HOLDLOCK \n\r" +
                                        " | PAGLOCK \n\r" +
                                        " | READCOMMITTED \n\r" +
                                        " | REPEATABLEREAD \n\r" +
                                        " | ROWLOCK \n\r" +
                                        " | SERIALIZABLE \n\r" +
                                        " | TABLOCK \n\r" +
                                        " | TABLOCKX \n\r" +
                                        " | UPDLOCK \n\r" +
                                        " } \n\r" +
                                        " \n\r" +
                                        "< table_hint > ::= \n\r" +
                                        " { INDEX ( index_val [ ,...n ] ) \n\r" +
                                        " | FASTFIRSTROW \n\r" +
                                        " | HOLDLOCK \n\r" +
                                        " | NOLOCK \n\r" +
                                        " | PAGLOCK \n\r" +
                                        " | READCOMMITTED \n\r" +
                                        " | READPAST \n\r" +
                                        " | READUNCOMMITTED \n\r" +
                                        " | REPEATABLEREAD \n\r" +
                                        " | ROWLOCK \n\r" +
                                        " | SERIALIZABLE \n\r" +
                                        " | TABLOCK \n\r" +
                                        " | TABLOCKX \n\r" +
                                        " | UPDLOCK \n\r" +
                                        " } \n\r" +
                                        " \n\r" +
                                        "< query_hint > ::= \n\r" +
                                        " { { HASH | ORDER } GROUP \n\r" +
                                        " | { CONCAT | HASH | MERGE } UNION \n\r" +
                                        " | FAST number_rows \n\r" +
                                        " | FORCE ORDER \n\r" +
                                        " | MAXDOP \n\r" +
                                        " | ROBUST PLAN \n\r" +
                                        " | KEEP PLAN \n\r" +
                                        " } \n\r";

        private string strUpdateSQLMain =
                                        "UPDATE \n\r" +
                                        " { \n\r" +
                                        " table_name WITH ( < table_hint_limited > [ ...n ] ) \n\r" +
                                        " | view_name \n\r" +
                                        " | rowset_function_limited \n\r" +
                                        " } \n\r" +
                                        " SET \n\r" +
                                        " { column_name = { expression | DEFAULT | NULL } \n\r" +
                                        " | @variable = expression \n\r" +
                                        " | @variable = column = expression } [ ,...n ] \n\r" +
                                        " \n\r" +
                                        " { { [ FROM { < table_source > } [ ,...n ] ] \n\r" +
                                        " \n\r" +
                                        " [ WHERE \n\r" +
                                        " < search_condition > ] } \n\r" +
                                        " | \n\r" +
                                        " [ WHERE CURRENT OF \n\r" +
                                        " { { [ GLOBAL ] cursor_name } | cursor_variable_name } \n\r" +
                                        " ] } \n\r" +
                                        " [ OPTION ( < query_hint > [ ,...n ] ) ] \n\r";


        private string strUpdateSQLFull =
                                        "UPDATE \n\r" +
                                        " { \n\r" +
                                        " table_name WITH ( < table_hint_limited > [ ...n ] ) \n\r" +
                                        " | view_name \n\r" +
                                        " | rowset_function_limited \n\r" +
                                        " } \n\r" +
                                        " SET \n\r" +
                                        " { column_name = { expression | DEFAULT | NULL } \n\r" +
                                        " | @variable = expression \n\r" +
                                        " | @variable = column = expression } [ ,...n ] \n\r" +
                                        " \n\r" +
                                        " { { [ FROM { < table_source > } [ ,...n ] ] \n\r" +
                                        " \n\r" +
                                        " [ WHERE \n\r" +
                                        " < search_condition > ] } \n\r" +
                                        " | \n\r" +
                                        " [ WHERE CURRENT OF \n\r" +
                                        " { { [ GLOBAL ] cursor_name } | cursor_variable_name } \n\r" +
                                        " ] } \n\r" +
                                        " [ OPTION ( < query_hint > [ ,...n ] ) ] \n\r" +
                                        " \n\r" +
                                        "< table_source > ::= \n\r" +
                                        " table_name [ [ AS ] table_alias ] [ WITH ( < table_hint > [ ,...n ] ) ]\n\r" +
                                        " | view_name [ [ AS ] table_alias ] \n\r" +
                                        " | rowset_function [ [ AS ] table_alias ] \n\r" +
                                        " | derived_table [ AS ] table_alias [ ( column_alias [ ,...n ] ) ] \n\r" +
                                        " | < joined_table > \n\r" +
                                        " \n\r" +
                                        "< joined_table > ::= \n\r" +
                                        " < table_source > < join_type > < table_source > ON < search_condition >\n\r" +
                                        " | < table_source > CROSS JOIN < table_source > \n\r" +
                                        " | < joined_table > \n\r" +
                                        " \n\r" +
                                        "< join_type > ::= \n\r" +
                                        " [ INNER | { { LEFT | RIGHT | FULL } [ OUTER ] } ] \n\r" +
                                        " [ < join_hint > ] \n\r" +
                                        " JOIN \n\r" +
                                        " \n\r" +
                                        "< table_hint_limited > ::= \n\r" +
                                        " { FASTFIRSTROW \n\r" +
                                        " | HOLDLOCK \n\r" +
                                        " | PAGLOCK \n\r" +
                                        " | READCOMMITTED \n\r" +
                                        " | REPEATABLEREAD \n\r" +
                                        " | ROWLOCK \n\r" +
                                        " | SERIALIZABLE \n\r" +
                                        " | TABLOCK \n\r" +
                                        " | TABLOCKX \n\r" +
                                        " | UPDLOCK \n\r" +
                                        " } \n\r" +
                                        " \n\r" +
                                        "< table_hint > ::= \n\r" +
                                        " { INDEX ( index_val [ ,...n ] ) \n\r" +
                                        " | FASTFIRSTROW \n\r" +
                                        " | HOLDLOCK \n\r" +
                                        " | NOLOCK \n\r" +
                                        " | PAGLOCK \n\r" +
                                        " | READCOMMITTED \n\r" +
                                        " | READPAST \n\r" +
                                        " | READUNCOMMITTED \n\r" +
                                        " | REPEATABLEREAD \n\r" +
                                        " | ROWLOCK \n\r" +
                                        " | SERIALIZABLE \n\r" +
                                        " | TABLOCK \n\r" +
                                        " | TABLOCKX \n\r" +
                                        " | UPDLOCK \n\r" +
                                        " } \n\r" +
                                        " \n\r" +
                                        "< query_hint > ::= \n\r" +
                                        " { { HASH | ORDER } GROUP \n\r" +
                                        " | { CONCAT | HASH | MERGE } UNION \n\r" +
                                        " | {LOOP | MERGE | HASH } JOIN \n\r" +
                                        " | FAST number_rows \n\r" +
                                        " | FORCE ORDER \n\r" +
                                        " | MAXDOP \n\r" +
                                        " | ROBUST PLAN \n\r" +
                                        " | KEEP PLAN \n\r" +
                                        " } \n\r";


        private string strInsertSQLMain =
                                        "INSERT [ INTO] \n\r" +
                                        " { table_name WITH ( < table_hint_limited > [ ...n ] )\n\r" +
                                        " | view_name \n\r" +
                                        " | rowset_function_limited \n\r" +
                                        " } \n\r" +
                                        " \n\r" +
                                        " { [ ( column_list ) ] \n\r" +
                                        " { VALUES \n\r" +
                                        " ( { DEFAULT | NULL | expression } [ ,...n] ) \n\r" +
                                        " | derived_table \n\r" +
                                        " | execute_statement \n\r" +
                                        " } \n\r" +
                                        " } \n\r" +
                                        " | DEFAULT VALUES \n\r" +
                                        " \n\r";


        private string strInsertSQLFull = 
                                        "INSERT [ INTO] \n\r" +
                                        " { table_name WITH ( < table_hint_limited > [ ...n ] )\n\r" +
                                        " | view_name \n\r" +
                                        " | rowset_function_limited \n\r" +
                                        " } \n\r" +
                                        " \n\r" +
                                        " { [ ( column_list ) ] \n\r" +
                                        " { VALUES \n\r" +
                                        " ( { DEFAULT | NULL | expression } [ ,...n] ) \n\r" +
                                        " | derived_table \n\r" +
                                        " | execute_statement \n\r" +
                                        " } \n\r" +
                                        " } \n\r" +
                                        " | DEFAULT VALUES \n\r" +
                                        " \n\r" +
                                        "< table_hint_limited > ::= \n\r" +
                                        " { FASTFIRSTROW \n\r" +
                                        " | HOLDLOCK \n\r" +
                                        " | PAGLOCK \n\r" +
                                        " | READCOMMITTED \n\r" +
                                        " | REPEATABLEREAD \n\r" +
                                        " | ROWLOCK \n\r" +
                                        " | SERIALIZABLE \n\r" +
                                        " | TABLOCK \n\r" +
                                        " | TABLOCKX \n\r" +
                                        " | UPDLOCK \n\r" +
                                        " } \n\r" +
                                        " \n\r";

        private string strSelectMySQLMain = "SELECT select_list \n\r[ INTO new_table | OUTFILE | DUMPFILE | var_list  ] \n\rFROM table_source \n\r[ WHERE search_condition ] \n\r[ GROUP BY group_by_expression ] \n\r[ HAVING search_condition ] \n\r[ ORDER BY order_expression [ ASC | DESC ] ] ";
        private string strSelectMySQLFull =
                                        "SELECT \n\r" +
                                        " [ALL | DISTINCT | DISTINCTROW ] \n\r" +
                                        " [HIGH_PRIORITY] \n\r" +
                                        " [STRAIGHT_JOIN] \n\r" +
                                        " [SQL_SMALL_RESULT] [SQL_BIG_RESULT] [SQL_BUFFER_RESULT]\n\r" +
                                        " [SQL_CACHE | SQL_NO_CACHE] [SQL_CALC_FOUND_ROWS] \n\r" +
                                        " select_expr [, select_expr ...] \n\r" +
                                        " [FROM table_references \n\r" +
                                        " [WHERE where_condition] \n\r" +
                                        " [GROUP BY {col_name | expr | position} \n\r" +
                                        " [ASC | DESC], ... [WITH ROLLUP]] \n\r" +
                                        " [HAVING where_condition] \n\r" +
                                        " [ORDER BY {col_name | expr | position} \n\r" +
                                        " [ASC | DESC], ...] \n\r" +
                                        " [LIMIT {[offset,] row_count | row_count OFFSET offset}] \n\r" +
                                        " [PROCEDURE procedure_name(argument_list)] \n\r" +
                                        " [INTO OUTFILE 'file_name' export_options \n\r" +
                                        " | INTO DUMPFILE 'file_name' \n\r" +
                                        " | INTO var_name [, var_name]] \n\r" +
                                        " [FOR UPDATE | LOCK IN SHARE MODE]] \n\r" +
                                        " \n\r";

        private string strDeleteMySQLMain =
                                        "DELETE \n\r" +
                                        "[ FROM ] \n\r" +
                                        "{ table_name } \n\r" +
                                        "[ WHERE \n\r" +
                                        "< search_condition >] \n\r";

        private string strDeleteMySQLFull =
                                        "DELETE [LOW_PRIORITY] [QUICK] [IGNORE] FROM tbl_name \n\r" +
                                        " [WHERE where_condition] \n\r" +
                                        " [ORDER BY ...] \n\r" +
                                        " [LIMIT row_count] \n\r" +
                                        " \n\r" +
                                        "Multiple-table syntax: \n\r" +
                                        "DELETE [LOW_PRIORITY] [QUICK] [IGNORE] \n\r" +
                                        " tbl_name[.*] [, tbl_name[.*]] ... \n\r" +
                                        " FROM table_references \n\r" +
                                        " [WHERE where_condition] \n\r" +
                                        " \n\r" +
                                        "Or: \n\r" +
                                        "DELETE [LOW_PRIORITY] [QUICK] [IGNORE] \n\r" +
                                        " FROM tbl_name[.*] [, tbl_name[.*]] ... \n\r" +
                                        " USING table_references \n\r" +
                                        " [WHERE where_condition] \n\r" +
                                        " \n\r";


        private string strUpdateMySQLMain =
                                        "UPDATE \n\r" +
                                        " { \n\r" +
                                        " table_name WITH ( < table_hint_limited > [ ...n ] ) \n\r" +
                                        " | view_name \n\r" +
                                        " | rowset_function_limited \n\r" +
                                        " } \n\r" +
                                        " SET \n\r" +
                                        " { column_name = { expression | DEFAULT | NULL } \n\r" +
                                        " \n\r" +
                                        " { { [ FROM { < table_source > } [ ,...n ] ] \n\r" +
                                        " \n\r" +
                                        " [ WHERE \n\r" +
                                        " < search_condition > ] } \n\r" +
                                        " | \n\r";



        private string strUpdateMySQLFull =  
                                        "UPDATE [LOW_PRIORITY] [IGNORE] table_reference \n\r " +
                                        " SET col_name1={expr1|DEFAULT} [, col_name2={expr2|DEFAULT}] ... \n\r " +
                                        " [WHERE where_condition] \n\r " +
                                        " [ORDER BY ...] \n\r " +
                                        " [LIMIT row_count] \n\r " +
                                        " \n\r " +
                                        "Multiple-table syntax: \n\r " +
                                        "UPDATE [LOW_PRIORITY] [IGNORE] table_references \n\r " +
                                        " SET col_name1={expr1|DEFAULT} [, col_name2={expr2|DEFAULT}] ... \n\r " +
                                        " [WHERE where_condition] \n\r " +
                                        " \n\r ";


        private string strInsertMySQLMain =
                                        "INSERT [ INTO] \n\r" +
                                        " { table_name \n\r" +
                                        " | view_name \n\r" +
                                        " | rowset_function_limited \n\r" +
                                        " } \n\r" +
                                        " \n\r" +
                                        " { [ ( column_list ) ] \n\r" +
                                        " { VALUES \n\r";

        private string strInsertMySQLFull =
                                        "INSERT [LOW_PRIORITY | DELAYED | HIGH_PRIORITY] [IGNORE]\n\r " +
                                        " [INTO] tbl_name [(col_name,...)] \n\r " +
                                        " {VALUES | VALUE} ({expr | DEFAULT},...),(...),... \n\r " +
                                        " [ ON DUPLICATE KEY UPDATE \n\r " +
                                        " col_name=expr \n\r " +
                                        " [, col_name=expr] ... ] \n\r " +
                                        " \n\r " +
                                        "Or: \n\r " +
                                        "INSERT [LOW_PRIORITY | DELAYED | HIGH_PRIORITY] [IGNORE]\n\r " +
                                        " [INTO] tbl_name \n\r " +
                                        " SET col_name={expr | DEFAULT}, ... \n\r " +
                                        " [ ON DUPLICATE KEY UPDATE \n\r " +
                                        " col_name=expr \n\r " +
                                        " [, col_name=expr] ... ] \n\r " +
                                        " \n\r " +
                                        "Or: \n\r " +
                                        "INSERT [LOW_PRIORITY | HIGH_PRIORITY] [IGNORE] \n\r " +
                                        " [INTO] tbl_name [(col_name,...)] \n\r " +
                                        " SELECT ... \n\r " +
                                        " [ ON DUPLICATE KEY UPDATE \n\r " +
                                        " col_name=expr \n\r " +
                                        " [, col_name=expr] ... ] \n\r " +
                                        " \n\r ";


        private string strSQLselect = "";
        private string strSQLinsert = "";
        private string strSQLdelete = "";
        private string strSQLupdate = "";


        public selectedSQLsyntax(string strDB, bool blnFull){
            if (strDB=="MSSQL"){
                strSQLselect = blnFull ? strSelectSQLFull : strSelectSQLMain;
                strSQLinsert = blnFull ? strInsertSQLFull : strInsertSQLMain;
                strSQLdelete = blnFull ? strDeleteSQLFull : strDeleteSQLMain;
                strSQLupdate = blnFull ? strUpdateSQLFull : strUpdateSQLMain;
            }
            if (strDB == "MSCOMP")
            {
                strSQLselect = blnFull ? strSelectSQLFull : strSelectSQLMain;
                strSQLinsert = blnFull ? strInsertSQLFull : strInsertSQLMain;
                strSQLdelete = blnFull ? strDeleteSQLFull : strDeleteSQLMain;
                strSQLupdate = blnFull ? strUpdateSQLFull : strUpdateSQLMain;
            }
            if (strDB == "MYSQL")
            {
                strSQLselect = blnFull ? strSelectMySQLFull : strSelectMySQLMain;
                strSQLinsert = blnFull ? strInsertMySQLFull : strInsertMySQLMain;
                strSQLdelete = blnFull ? strDeleteMySQLFull : strDeleteMySQLMain;
                strSQLupdate = blnFull ? strUpdateMySQLFull : strUpdateMySQLMain;
            }


        }


        public string SelectSQL
        {
            get
            {
                return strSQLselect;
            }

        }

        public string InsertSQL
        {
            get
            {
                return strSQLinsert;
            }

        }

        public string DeleteSQL
        {
            get
            {
                return strSQLdelete;
            }

        }

        public string UpdateSQL
        {
            get
            {
                return strSQLupdate;
            }

        }

        // String Function
        // Math Function
        // DateTime Function
        // Aggregate Function








    }
}
