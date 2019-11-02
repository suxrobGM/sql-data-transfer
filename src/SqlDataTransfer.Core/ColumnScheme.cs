using Newtonsoft.Json;

namespace SqlDataTransfer.Core
{
    public class ColumnScheme
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string OrdinalPosition { get; set; }
        public string DefaultValue { get; set; }
        public string CharLength { get; set; }
        public bool IsNullable { get; set; }      

        public string ToJson(Formatting formatting = Formatting.None) => JsonConvert.SerializeObject(this, formatting);
    }
}
