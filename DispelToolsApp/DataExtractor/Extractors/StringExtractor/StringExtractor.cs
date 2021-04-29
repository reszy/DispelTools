using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace DispelTools.DataExtractor.StringExtractor
{
    public class StringExtractor : Extractor
    {

        public override void ExtractFile(ExtractionFileProcess process)
        {
            var file = process.File;
            var strings = new Dictionary<long, string>();
            string oneString = "";
            long position = -1;
            while (process.Stream.Position < process.Stream.Length)
            {
                char c = (char)file.ReadByte();
                if (isTextCharacter(c))
                {
                    if (position < 0)
                    {
                        position = file.BaseStream.Position;
                    }
                    oneString += c;
                }
                else if (position > 0 && oneString.Length > 2)
                {
                    strings.Add(position, oneString);
                    oneString = "";
                    position = -1;
                }
            }

            var finalname = $"{process.OutputDirectory}\\{process.Filename}_strings.txt";
            using (var writer = new StreamWriter(new FileStream(finalname, FileMode.Create)))
            {
                foreach (var entry in strings)
                {
                    writer.WriteLine($"{entry.Key:X}({entry.Key}): {entry.Value}");
                }
            }
            process.Extractor.RaportFileCreatedDetail(process,finalname);
            //ResultDetails.Add($"Total entires: {strings.Count}");
        }

        private bool isTextCharacter(char c) => c >= 32 && c <= 0x7d;
    }
}
