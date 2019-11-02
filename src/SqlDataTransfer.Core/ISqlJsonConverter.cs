using System.Collections.Generic;

namespace SqlDataTransfer.Core
{
    public interface ISqlJsonConverter
    {
        IList<string> GetAllTableNames();
        TableScheme GetTableScheme(string tableName);
        string GetTableJsonScheme(string tableName);
        TableRecords GetTableRecords(string tableName);
        string GetTableRecordsJson(string tableName);
        string GetRecordsJson(string tableName);
    }
}
