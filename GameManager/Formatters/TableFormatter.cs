using GameManager.Exceptions;
using GameManager.Tools;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Formatters
{
    public class TableFormatter
    {
        private readonly Dictionary<string, int> _columnWidths;
        private readonly DataTable _dataTable;

        private const char VERTICAL = '║';
        private const char HORIZONTAL = '═';
        private const char TOP_LEFT = '╔';
        private const char TOP_RIGHT = '╗';
        private const char BOTTOM_LEFT = '╚';
        private const char BOTTOM_RIGHT = '╝';
        private const char TOP_INTERSECTION = '╦';
        private const char BOTTOM_INTERSECTION = '╩';
        private const char LEFT_INTERSECTION = '╠';
        private const char RIGHT_INTERSECTION = '╣';
        private const char CROSS_INTERSECTION = '╬';

        public TableFormatter(DataTable dataTable)
        {
            _dataTable = dataTable;
            _columnWidths = new Dictionary<string, int>();
        }

        

        private void CalculateColumnWidths()
        {
            foreach (DataColumn column in _dataTable.Columns)
            {
                _columnWidths[column.ColumnName] = GetMaxColumnWidth(column);
            }
        }

        private int GetMaxColumnWidth(DataColumn column)
        {
            int maxWidth = column.ColumnName.Length;

            foreach (DataRow row in _dataTable.Rows)
            {
                string formattedValue = FormatCellValue(row[column]);
                maxWidth = Math.Max(maxWidth, formattedValue.Length);
            }

            return maxWidth;
        }

        private string FormatCellValue(object value)
        {
            if (value == null) return string.Empty;

            return value switch
            {
                DateTime dateValue => dateValue.ToString("yyyy-MM-dd HH:mm:ss"),
                bool boolValue => boolValue.ToString(),
                _ => value.ToString()
            };
        }

        public string BuildFormattedTable()
        {
            CalculateColumnWidths();
            var sb = new StringBuilder();

            // Construire le haut de la table
            sb.AppendLine(BuildTopLine());
            sb.AppendLine(BuildHeaderLine());
            sb.AppendLine(BuildMiddleLine());

            // Construire les lignes de données
            foreach (DataRow row in _dataTable.Rows)
            {
                sb.AppendLine(BuildDataLine(row));
            }

            // Construire le bas de la table
            sb.AppendLine(BuildBottomLine());

            return sb.ToString();
        }

        private string BuildTopLine()
        {
            return TOP_LEFT +
                   string.Join(TOP_INTERSECTION, _dataTable.Columns
                       .Cast<DataColumn>()
                       .Select(col => new string(HORIZONTAL, _columnWidths[col.ColumnName] + 2))) +
                   TOP_RIGHT;
        }

        private string BuildMiddleLine()
        {
            return LEFT_INTERSECTION +
                   string.Join(CROSS_INTERSECTION, _dataTable.Columns
                       .Cast<DataColumn>()
                       .Select(col => new string(HORIZONTAL, _columnWidths[col.ColumnName] + 2))) +
                   RIGHT_INTERSECTION;
        }

        private string BuildBottomLine()
        {
            return BOTTOM_LEFT +
                   string.Join(BOTTOM_INTERSECTION, _dataTable.Columns
                       .Cast<DataColumn>()
                       .Select(col => new string(HORIZONTAL, _columnWidths[col.ColumnName] + 2))) +
                   BOTTOM_RIGHT;
        }

        private string BuildHeaderLine()
        {
            return VERTICAL +
                   string.Join(VERTICAL, _dataTable.Columns
                       .Cast<DataColumn>()
                       .Select(col => $" {col.ColumnName.PadRight(_columnWidths[col.ColumnName])} ")) +
                   VERTICAL;
        }

        private string BuildDataLine(DataRow row)
        {
            return VERTICAL +
                   string.Join(VERTICAL, _dataTable.Columns
                       .Cast<DataColumn>()
                       .Select(col =>
                       {
                           string value = FormatCellValue(row[col]);
                           return $" {value.PadRight(_columnWidths[col.ColumnName])} ";
                       })) +
                   VERTICAL;
        }
    }
}
