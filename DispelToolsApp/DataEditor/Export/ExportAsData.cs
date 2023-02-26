using DispelTools.Common.DataProcessing;
using DispelTools.DataEditor.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Abstractions;

namespace DispelTools.DataEditor.Export
{
    public class ExportAsData : Exporter
    {
        private readonly SimpleDataContainer container;

        public ExportAsData(IFileSystem fs, WorkReporter workReporter, SimpleDataContainer container) : base(fs, workReporter)
        {
            this.container = container;
        }

        public int DataItemIndex { get; set; }
        public bool AllProperties { get; set; }

        public override void Export(string path)
        {
            JArray root = new();
            if (AllProperties)
            {
                for (var i = 0; i < container.Count; i++)
                {
                    root.Add(CreateObject(container[i]));
                }
            }
            else {
                if (DataItemIndex < 0 || DataItemIndex >= container.Count) { throw new IndexOutOfRangeException(); }
                root.Add(CreateObject(container[DataItemIndex]));
            }
            using var file = fs.File.Create(path);
            using var stringWriter = new StreamWriter(file);
            using var jsonWriter = new JsonTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented,
            };
            root.WriteTo(jsonWriter);
        }


        private JObject CreateObject(DataItem fields)
        {
            JObject obj = new JObject();
            int byteCount = 0;
            int byteGapCounter = 0;
            for(int i =0; i < fields.Count; i++)
            {
                var field = fields[i];
                var fieldDescriptor = container.FieldDescriptors[i];
                if (field.Name.StartsWith("?"))
                {
                    byteCount += fieldDescriptor.ItemFieldDescriptorType.ByteSize;
                }
                else
                {
                    if(byteCount > 0)
                    {
                        obj.Add($"UnknownBytes{++byteGapCounter}", new JValue(byteCount));
                        byteCount = 0;
                    }
                    obj.Add(field.Name, CreateField(field));
                }
            }
            return obj;
        }
        private JValue CreateField(IField field)
        {
            if(field is IPrimitiveField)
            {
                return new JValue(field.Value);
            }
            return new JValue(field.GetDecodedText());
        }
    }
}
