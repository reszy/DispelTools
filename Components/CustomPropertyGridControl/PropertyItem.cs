using System;
using System.Collections;
using System.Collections.Generic;

namespace DispelTools.Components.CustomPropertyGridControl
{
    public class PropertyItem : IEnumerable<Field>
    {
        private readonly List<Field> fields = new List<Field>();
        public void AddField(Field field) => fields.Add(field);
        public IEnumerator<Field> GetEnumerator() => fields.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => fields.GetEnumerator();
        public Field this[int i] => fields[i];
        public Field FindByName(string name) => fields.Find(field => field.Name == name);
        internal int Count { get => fields.Count; }

        public static PropertyItem Sample()
        {
            var item = new PropertyItem();
            item.AddField(new Field("exField1", 1));
            item.AddField(new Field("exField2readOnly", 2, true));
            item.AddField(new Field("exField3", 3));
            item.AddField(new Field("exField4Int", 254));
            item.AddField(new Field("exField5Hex", 321)//0x141
            {
                Type = Field.FieldType.HEX
            });
            item.AddField(new Field("exField6Str", "abcde"));
            item.AddField(new Field("exField6LongLabel", "abcde"));
            item.AddField(new Field("exField7LongLabelReadOnly", "abcde", true));
            return item;
        }

        public static PropertyItem Sample(int number)
        {
            var item = new PropertyItem();
            for (int i = 0; i < number; i++)
            {
                item.AddField(new Field()
                {
                    Name = $"exField{i + 1}",
                    Value = 1,
                    ReadOnly = false
                });
            }
            return item;
        }
    }
}
