﻿using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

namespace DispelTools.DataExtractor.AllExtractor.Tests
{
    public class AllFilesExtractorTests
    {
        private readonly MockFileSystem mockFileSystem = new();

        [Test]
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
            var results = extractor.Initialize(extractionParams.Filename, extractionParams.OutputDirectory);

            foreach (var result in results)
            {
                Assert.That(FindSimilar(result.FileName, expectedListOfFiles), Is.Not.Null, $"Not found {result} in results");
                StringAssert.StartsWith(outputDirectory, result.OuputDirectory);
            }
            CollectionAssert.AllItemsAreUnique(results);
            Assert.That(results, Has.Count.EqualTo(expectedListOfFiles.Count), $"\nElements: \n{string.Join("\n", results)}");
        }

        private string FindSimilar(string path, List<string> list) => list.First(s => s.StartsWith(mockFileSystem.Path.GetDirectoryName(path) ?? string.Empty));
    }
}