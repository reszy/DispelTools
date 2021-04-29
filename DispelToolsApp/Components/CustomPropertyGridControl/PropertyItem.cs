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
            item.AddField(new Field("exField1", 1, Field.DisplayType.DEC));
            item.AddField(new Field("exField2readOnly", 2, Field.DisplayType.DEC, true));
            item.AddField(new Field("exField3", 3, Field.DisplayType.DEC));
            item.AddField(new Field("exField4Int", 254, Field.DisplayType.DEC));
            item.AddField(new Field("exField5Hex", 321, Field.DisplayType.HEX));//0x141
            item.AddField(new Field("exField6Str", "abcde", Field.DisplayType.ASCII));
            item.AddField(new Field("exField6LongLabel", "abcde", Field.DisplayType.ASCII));
            item.AddField(new Field("exField7LongLabelReadOnly", "abcde", Field.DisplayType.ASCII, true));
            return item;
        }
    }
}
