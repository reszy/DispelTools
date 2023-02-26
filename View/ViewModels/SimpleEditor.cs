using DispelTools.Common.DataProcessing;
using DispelTools.DataEditor;
using DispelTools.DataEditor.Data;
using DispelTools.DataEditor.Export;
using System;
using System.IO.Abstractions;
using View.Exceptions;

namespace View.ViewModels
{
    public class SimpleEditor
    {
        private readonly IFileSystem fs;
        private SimpleDataLoader? dataLoader;
        private SimpleDataContainer? container;

        public SimpleEditor(IFileSystem fs)
        {
            dataLoader = null;
            container = null;
            this.fs = fs;
        }

        public void Load(string filename, WorkReporter workReporter)
        {
            dataLoader = SimpleDataLoader.GetMapperForFilename(fs, filename);
            if(dataLoader is null)
            {
                throw new MessageException($"No mapper found for {System.IO.Path.GetFileName(filename)}", "File unsupported", MessageException.MessageType.WARNING);
            }
            container = dataLoader.LoadData(filename, workReporter);
            if(container is null)
            {
                throw new MessageException($"Could not load file:\n {filename}", MessageException.MessageType.ERROR);
            }
        }

        public DataItem GetValue(int itemIndex)
        {
            if (container is null)
            {
                throw new MessageException($"Cannot read from empty container", MessageException.MessageType.ERROR);
            }
            return container[itemIndex];
        }

        public int GetElementCount() => container?.Count ?? 0;
        public void Save(DataItem item, int itemIndex)
        {
            if (container is null) throw new MissingVariableException(nameof(container));
            if (dataLoader is null) throw new MissingVariableException(nameof(dataLoader));
            if (container.IndexOf(item) >= itemIndex) throw new MessageException("Item comes from diffrent index", MessageException.MessageType.ERROR);
            string orginalBackup = container.Path + ".orginalbackup.bak";
            if (!fs.File.Exists(orginalBackup))
            {
                fs.File.Copy(container.Path, orginalBackup);
            }
            dataLoader.SaveElement(container, item);
        }

        public void ExportDocScheme()
        {
            var exporter = new ExportAsDocScheme(fs);
            exporter.PrepareDirectoryAndMappers();
            exporter.Export();
        }

        public SimpleDataContainer GetLoadedContainer() => container ?? throw new NullReferenceException();
    }
}
