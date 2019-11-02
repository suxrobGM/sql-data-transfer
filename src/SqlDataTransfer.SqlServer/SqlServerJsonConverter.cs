using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlDataTransfer.Core;

namespace SqlDataTransfer.SqlServer
{
    public class SqlServerJsonConverter : ISqlJsonConverter
    {
        private readonly SqlConnection _sqlConnection;

        public SqlServerJsonConverter(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
        }

        public IList<string> GetAllTableNames()
        {
            _sqlConnection.Open();
            var rows = _sqlConnection.GetSchema("Tables").Rows;
            var tableNames = new List<string>();

            foreach (DataRow row in rows)
            {
                tableNames.Add((string)row[2]);
            }
            _sqlConnection.Close();

            return tableNames;
        }

        public TableScheme GetTableScheme(string tableName)
        {
            _sqlConnection.Open();
            var table = _sqlConnection.GetSchema("Tables", new[] { _sqlConnection.Database, null, tableName });
            var columns = _sqlConnection.GetSchema("Columns", new[] { _sqlConnection.Database, null, tableName });
            var tableScheme = new TableScheme()
            {
                Name = tableName,
                Type = table.Rows[0].ItemArray[3].ToString(),
                Columns = new List<ColumnScheme>(),
            };

            foreach (DataRow column in columns.Rows)
            {
                tableScheme.Columns.Add(new ColumnScheme()
                {
                    Name = column.ItemArray[3].ToString(),
                    OrdinalPosition = column.ItemArray[4].ToString(),
                    DefaultValue = column.ItemArray[5].ToString(),
                    IsNullable = column.ItemArray[6].ToString() == "YES" ? true : false,
                    DataType = column.ItemArray[7].ToString(),
                    CharLength = column.ItemArray[8].ToString()
                });
            }

            _sqlConnection.Close();
            return tableScheme;
        }

        public string GetTableJsonScheme(string tableName)
        {
            return GetTableScheme(tableName).ToJson(Formatting.Indented);
        }


        public TableRecords GetTableRecords(string tableName)
        {
            _sqlConnection.Open();

            // Table records
            var cmd = _sqlConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM {tableName} FOR JSON AUTO";
            var reader = cmd.ExecuteReader();
            var stringBuilder = new StringBuilder();

            while (reader.Read())
            {
                stringBuilder.Append(reader[0]);
            }
            var recordsJson = JToken.Parse(stringBuilder.ToString());

            // Table scheme 
            var table = _sqlConnection.GetSchema("Tables", new[] { _sqlConnection.Database, null, tableName });
            var columns = _sqlConnection.GetSchema("Columns", new[] { _sqlConnection.Database, null, tableName });
            var tableScheme = new TableScheme()
            {
                Name = tableName,
                Type = table.Rows[0].ItemArray[3].ToString(),
                Columns = new List<ColumnScheme>(),
            };

            foreach (DataRow column in columns.Rows)
            {
                tableScheme.Columns.Add(new ColumnScheme()
                {
                    Name = column.ItemArray[3].ToString(),
                    OrdinalPosition = column.ItemArray[4].ToString(),
                    DefaultValue = column.ItemArray[5].ToString(),
                    IsNullable = column.ItemArray[6].ToString() == "YES" ? true : false,
                    DataType = column.ItemArray[7].ToString(),
                    CharLength = column.ItemArray[8].ToString()
                });
            }

            var tableRecords = new TableRecords()
            {
                TableName = tableName,
                Records = recordsJson,
                TableScheme = tableScheme
            };

            _sqlConnection.Close();
            return tableRecords;
        }

        public string GetTableRecordsJson(string tableName)
        {
            return GetTableRecords(tableName).ToJson(Formatting.Indented);
        }       

        public string GetRecordsJson(string tableName)
        {
            _sqlConnection.Open();
            var cmd = _sqlConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM {tableName} FOR JSON AUTO";
            var reader = cmd.ExecuteReader();
            var stringBuilder = new StringBuilder();

            while (reader.Read())
            {
                stringBuilder.Append(reader[0]);
            }
            var jsonData = JToken.Parse(stringBuilder.ToString()).ToString(Formatting.Indented);

            _sqlConnection.Close();
            return jsonData;
        }
    }
}
