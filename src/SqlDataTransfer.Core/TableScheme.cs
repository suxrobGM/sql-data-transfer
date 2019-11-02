using System.Collections.Generic;
using Newtonsoft.Json;

namespace SqlDataTransfer.Core
{
    public class TableScheme
    {
        public TableScheme()
        {
            Columns = new List<ColumnScheme>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public ICollection<ColumnScheme> Columns { get; set; }

        public string ToJson(Formatting formatting = Formatting.None) => JsonConvert.SerializeObject(this, formatting);
    }
}
