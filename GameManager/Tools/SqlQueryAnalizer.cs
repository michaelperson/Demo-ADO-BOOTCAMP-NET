using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Tools
{
    public static class SqlQueryAnalyzer
    {
        public enum SqlOperationType
        {
            Select,
            Update,
            Delete,
            Insert,
            Merge,
            Unknown
        }

        private static readonly Dictionary<SqlOperationType, string[]> SqlPatterns = new()
    {
        {
            SqlOperationType.Select,
            new[]
            {
                @"\bSELECT\b",
                @"WITH\s+.*?\bSELECT\b"  // Pour les CTE
            }
        },
        {
            SqlOperationType.Update,
            new[]
            {
                @"\bUPDATE\b",
                @"BULK\s+UPDATE",
                @"MERGE\b.*\bWHEN\s+MATCHED\s+THEN\s+UPDATE\b"
            }
        },
        {
            SqlOperationType.Delete,
            new[]
            {
                @"\bDELETE\b",
                @"TRUNCATE\s+TABLE",
                @"MERGE\b.*\bWHEN\s+MATCHED\s+THEN\s+DELETE\b"
            }
        },
        {
            SqlOperationType.Insert,
            new[]
            {
                @"\bINSERT\b",
                @"BULK\s+INSERT",
                @"MERGE\b.*\bWHEN\s+NOT\s+MATCHED\s+THEN\s+INSERT\b"
            }
        },
        {
            SqlOperationType.Merge,
            new[]
            {
                @"\bMERGE\b"
            }
        }
    };

        public static string NormalizeQuery(string sqlQuery)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                return string.Empty;

            return sqlQuery
                .Replace("\n", " ")
                .Replace("\r", " ")
                .Trim()
                .ToUpperInvariant();
        }

        private static string RemoveSqlComments(string sql)
        {
            // Supprimer les commentaires en ligne
            sql = System.Text.RegularExpressions.Regex.Replace(
                sql,
                @"--.+?$",
                "",
                System.Text.RegularExpressions.RegexOptions.Multiline
            );

            // Supprimer les commentaires multi-lignes
            sql = System.Text.RegularExpressions.Regex.Replace(
                sql,
                @"/\*.*?\*/",
                "",
                System.Text.RegularExpressions.RegexOptions.Singleline
            );

            return sql;
        }

        public static bool ContainsOperation(string sqlQuery, SqlOperationType operationType)
        {
            if (string.IsNullOrWhiteSpace(sqlQuery))
                return false;

            var normalizedQuery = RemoveSqlComments(NormalizeQuery(sqlQuery));

            if (!SqlPatterns.ContainsKey(operationType))
                return false;

            return SqlPatterns[operationType].Any(pattern =>
                System.Text.RegularExpressions.Regex.IsMatch(normalizedQuery, pattern));
        }
 

         
    }
}
