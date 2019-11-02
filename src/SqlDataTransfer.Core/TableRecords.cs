using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SqlDataTransfer.Core
{
    public class TableRecords
    {
        public TableRecords()
        {
            Timestamp = DateTime.Now;
        }

        public string TableName { get; set; }
        public DateTime Timestamp { get; set; }
        public TableScheme TableScheme { get; set; }
        public JToken Records { get; set; }

        public override int GetHashCode()
        {
            return Records.GetHashCode();
        }

        public string ToJson(Formatting formatting = Formatting.None) => JsonConvert.SerializeObject(this, formatting);
    }
}
