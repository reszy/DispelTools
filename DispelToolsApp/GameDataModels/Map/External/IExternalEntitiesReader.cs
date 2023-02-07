using DispelTools.Common.DataProcessing;
using System.Collections.Generic;

namespace DispelTools.GameDataModels.Map.External
{
    interface IExternalEntitiesReader
    {
        List<MapExternalObject> GetObjects(string gamePath, string mapFilePath, MapContainer mapContainer, WorkReporter workReporter);
    }
}
