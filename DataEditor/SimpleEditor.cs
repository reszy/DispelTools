using DispelTools.Components.CustomPropertyGridControl;
using DispelTools.DataEditor.Mappers;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace DispelTools.DataEditor
{
    public class SimpleEditor
    {
        private IFileSystem fs;
        private string filename;
        private bool validated;
        private Mapper mapper;
        private List<PropertyItem> values;

        public string ValidationMessage { get; private set; }

        public SimpleEditor(string filename, IFileSystem fs)
        {
            this.filename = filename;
            mapper = null;
            validated = false;
            values = null;
            ValidationMessage = "Not validated";
            this.fs = fs;
        }
        public SimpleEditor(string filename) : this(filename, new FileSystem())
        {
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

        public void SetMapper(Mapper mapper)
        {
            this.mapper = mapper;
            ValidationMessage = "Mapper was added manually";
            validated = true;
        }
        private Mapper FindMapper()
        {
            string filenameWithExtension = fs.Path.GetFileName(filename);
            if (filenameWithExtension.ToUpper().StartsWith("NPC") && filenameWithExtension.ToUpper().EndsWith("REF"))
            {
                return new NpcRefMapper();
            }
            if (filenameWithExtension.ToUpper().StartsWith("MON") && filenameWithExtension.ToUpper().EndsWith("REF"))
            {
                return new MonRefMapper();
            }
            if (filenameWithExtension.ToUpper().Equals("EDITITEM.DB"))
            {
                return new EditItemDbMapper();
            }
            if (filenameWithExtension.ToUpper().Equals("HEALITEM.DB"))
            {
                return new HealItemDbMapper();
            }
            if (filenameWithExtension.ToUpper().Equals("EVENTITEM.DB"))
            {
                return new EventItemDbMapper();
            }
            if (filenameWithExtension.ToUpper().Equals("MISCITEM.DB"))
            {
                return new MiscItemDbMapper();
            }
            if (filenameWithExtension.ToUpper().Equals("WEAPONITEM.DB"))
            {
                return new WeaponItemDbMapper();
            }
            if (filenameWithExtension.ToUpper().Equals("STORE.DB"))
            {
                return new StoreDbMapper();
            }
            if (filenameWithExtension.ToUpper().Equals("MONSTER.DB"))
            {
                return new MonsterDbMapper();
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
        public void Save(PropertyItem element, int elementNumber)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException("Mapper not found");
            }
            string orginalBackup = filename + ".orginalbackup.bak";
            if (!fs.File.Exists(orginalBackup))
            {
                fs.File.Copy(filename, orginalBackup);
            }
            mapper.SaveElement(element, elementNumber, filename);
        }
    }
}
