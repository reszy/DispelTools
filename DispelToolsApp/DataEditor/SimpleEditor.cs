using DispelTools.Common.DataProcessing;
using DispelTools.DataEditor.Data;
using DispelTools.DataEditor.Export;
using DispelTools.DataEditor.Mappers;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace DispelTools.DataEditor
{
    public class SimpleEditor
    {
        private readonly IFileSystem fs;
        private readonly string filename;
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
        private Mapper FindMapper() => Mapper.GetMapperForFilename(fs, filename) ?? throw new ArgumentException($"No mapper found for {fs.Path.GetFileName(filename)}");

        public PropertyItem ReadValue(int element, WorkReporter workReporter)
        {
            if (values == null)
            {
                if (mapper == null)
                {
                    throw new ArgumentNullException("Mapper not found");
                }
                values = mapper.ReadFile(filename, workReporter);
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

        public void ExportDocScheme()
        {
            var exporter = new ExportAsDocScheme(fs);
            exporter.PrepareDirectoryAndMappers();
            exporter.Export();
        }
    }
}
