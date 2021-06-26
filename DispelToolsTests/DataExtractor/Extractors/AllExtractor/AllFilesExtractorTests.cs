using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

namespace DispelTools.DataExtractor.AllExtractor.Tests
{
    [TestClass()]
    public class AllFilesExtractorTests
    {
        [TestMethod()]
        public void InitializeTest()
        {
            string gameFolder = @"c:\game";
            string outputDirectory = @"c:\out";
            var expectedListOfFiles = new List<string>() {
                gameFolder + @"\sound1.SNF",
                gameFolder + @"\sound2.snf",
                gameFolder + @"\sprites\SPRITE1.SPR",
                gameFolder + @"\sprites\sprite2.spr",
                gameFolder + @"\sprites\sprite3.SPR",
                gameFolder + @"\sprites\Sprite4.SPR",
                gameFolder + @"\sprites\Sprite5.spr",
                gameFolder + @"\maps\map1.map",
            };

            var mockFileSystem = new MockFileSystem();
            var mockFileData = new MockFileData("");
            mockFileSystem.AddDirectory(gameFolder);
            mockFileSystem.AddDirectory(gameFolder + @"\CharacterInGame");
            mockFileSystem.AddDirectory(gameFolder + @"\Main");
            mockFileSystem.AddFile(gameFolder + @"\Dispel.exe", mockFileData);
            mockFileSystem.AddFile(gameFolder + @"\AllMap.ini", mockFileData);
            foreach (string filename in expectedListOfFiles)
            {
                mockFileSystem.AddFile(filename, mockFileData);
            }

            var extractor = new AllFilesExtractor(mockFileSystem);
            var extractionParams = new ExtractionParams()
            {
                Filename = new List<string>() { gameFolder },
                OutputDirectory = outputDirectory
            };
            var manager = new ExtractionManager(mockFileSystem, extractor, extractionParams, new System.ComponentModel.BackgroundWorker());
            var results = extractor.Initialize(manager, extractionParams.Filename, extractionParams.OutputDirectory);

            foreach (var result in results)
            {
                Assert.IsNotNull(FindSimilar(result.FileName, expectedListOfFiles), $"Not found {result} in results");
                StringAssert.StartsWith(result.OuputDirectory, outputDirectory);
            }
            CollectionAssert.AllItemsAreUnique(results);
            Assert.AreEqual(expectedListOfFiles.Count, results.Count, $"\nElements: \n{string.Join("\n", results)}");
        }

        private string FindSimilar(string path, List<string> list) => list.Where(s => s.StartsWith(Path.GetDirectoryName(path))).First();
    }
}