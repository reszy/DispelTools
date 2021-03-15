using DispelTools.Components.CustomPropertyGridControl;
using System;
using System.Collections.Generic;
using System.IO;

namespace DispelTools.DataEditor
{
    public class SimpleEditor
    {
        private string filename;
        private bool validated;
        private Mapper mapper;
        private List<PropertyItem> values;

        public string ValidationMessage { get; private set; }

        public SimpleEditor(string filename)
        {
            this.filename = filename;
            mapper = null;
            validated = false;
            values = null;
            ValidationMessage = "Not validated";
        }

        public bool CanOpen()
        {
            if (!validated)
            {
                try
                {
                    mapper = FindMapper();
                    ValidationMessage = "File is valid";
                }
                catch (ArgumentException e)
                {
                    ValidationMessage = e.Message;
                }
                validated = true;
            }
            return mapper != null;
        }
        private Mapper FindMapper()
        {
            string filenameWithExtension = Path.GetFileName(filename);
            if (filenameWithExtension.ToUpper().StartsWith("NPC") && filenameWithExtension.ToUpper().EndsWith("REF"))
            {
                return new NpcRefMapper();
            }

            throw new ArgumentException($"No mapper found for {filenameWithExtension}");
        }

        public PropertyItem ReadValue(int element)
        {
            if (values == null)
            {
                if (mapper == null)
                {
                    throw new ArgumentNullException("Mapper not found");
                }
                values = mapper.ReadFile(filename);
            }
            return values[element];
        }

        public int GetElementCount() => values?.Count - 1 ?? 0;
        internal void Save(PropertyItem element, int elementNumber) {
            if (mapper == null)
            {
                throw new ArgumentNullException("Mapper not found");
            }
            mapper.SaveElement(element , elementNumber, filename);
        }
    }
}
