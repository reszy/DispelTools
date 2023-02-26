using System;
using System.Collections;
using System.Collections.Generic;

namespace DispelTools.DataEditor.Data
{
    public class DataItem : IEnumerable<IField>
    {
        private readonly List<IField> fields = new List<IField>();
        public void AddField(IField field) => fields.Add(field);
        public void Add(IField field) => fields.Add(field);
        public IEnumerator<IField> GetEnumerator() => fields.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => fields.GetEnumerator();
        public IField this[int i] => fields[i];
        public IField? FindByName(string name) => fields.Find(field => field.Name == name);
        internal int Count { get => fields.Count; }

        public static DataItem Sample()
        {
            var item = new DataItem();
            item.AddField(new PrimitiveField<int>("exField1", 1, Field.DisplayType.DEC));
            item.AddField(new PrimitiveField<int>("exField2readOnly", 2, Field.DisplayType.DEC) { ReadOnly = true });
            item.AddField(new PrimitiveField<int>("exField3", 3, Field.DisplayType.DEC));
            item.AddField(new PrimitiveField<int>("exField4Int", 254, Field.DisplayType.DEC));
            item.AddField(new PrimitiveField<int>("exField5Hex", 321, Field.DisplayType.HEX));//0x141
            item.AddField(new TextField("exField6Str", "abcde", Field.SupportedEncoding.ASCII));
            item.AddField(new TextField("exField7LongLabel____________asdasdasdasdasdasdasdasd", "abcde", Field.SupportedEncoding.ASCII));
            item.AddField(new TextField("exField8ReadOnlyLongLabel____________asdasdasdasdasdd", "abcde", Field.SupportedEncoding.ASCII) { ReadOnly = true });
            item.AddField(new TextField("exField9WithDescription", "abcde", Field.SupportedEncoding.ASCII) { ReadOnly = true, Description = "This should be in second line" });
            return item;
        }
    }
}
