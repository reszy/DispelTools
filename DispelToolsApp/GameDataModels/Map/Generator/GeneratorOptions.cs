using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DispelTools.GameDataModels.Map.Generator
{
    public class GeneratorOptions
    {
        public GeneratorOptions() { }
        public GeneratorOptions(
            bool occlusion,
            bool gtl,
            bool sprites,
            bool collisions,
            bool tiledObjects,
            bool roofs,
            bool events,
            bool externalExtra,
            bool externalMonster,
            bool externalNpc,
            bool debugDots)
        {
            Occlusion = occlusion;
            GTL = gtl;
            Sprites = sprites;
            Collisions = collisions;
            TiledObjects = tiledObjects;
            Roofs = roofs;
            Events = events;
            ExternalExtra = externalExtra;
            ExternalMonster = externalMonster;
            ExternalNpc = externalNpc;
            DebugDots = debugDots;
        }

        public bool Occlusion { get; } = true;
        public bool GTL { get; } = true;
        public bool Sprites { get; } = false;
        public bool Collisions { get; } = false;
        public bool TiledObjects { get; } = false;
        public bool Roofs { get; } = false;
        public bool Events { get; } = false;
        public bool ExternalExtra { get; } = false;
        public bool ExternalMonster { get; } = false;
        public bool ExternalNpc { get; } = false;
        public bool DebugDots { get; } = false;

        public string ToSetting()
        {
            var options = new bool[] { Occlusion, GTL, Sprites, Collisions, TiledObjects, Roofs, Events, ExternalExtra, ExternalMonster, ExternalNpc, DebugDots };
            StringBuilder sb = new StringBuilder();
            foreach (var option in options) sb.Append(option ? '1' : '0');
            return sb.ToString();
        }

        public static GeneratorOptions LoadSetting(string setting)
        {
            var options = setting.Select(@char => @char == '1').ToArray();
            if (options.Length != 11)
            {
                return new GeneratorOptions();
            }
            else
            {
                return new GeneratorOptions(
                    options[0],
                    options[1],
                    options[2],
                    options[3],
                    options[4],
                    options[5],
                    options[6],
                    options[7],
                    options[8],
                    options[9],
                    options[10]
                );
            }
        }

        public override bool Equals(object obj)
        {
            return obj is GeneratorOptions options &&
                   Occlusion == options.Occlusion &&
                   GTL == options.GTL &&
                   Sprites == options.Sprites &&
                   Collisions == options.Collisions &&
                   TiledObjects == options.TiledObjects &&
                   Roofs == options.Roofs &&
                   Events == options.Events &&
                   ExternalExtra == options.ExternalExtra &&
                   ExternalMonster == options.ExternalMonster &&
                   ExternalNpc == options.ExternalNpc &&
                   DebugDots == options.DebugDots;
        }

        public override int GetHashCode()
        {
            int hashCode = -1938136125;
            hashCode = hashCode * -1521134295 + Occlusion.GetHashCode();
            hashCode = hashCode * -1521134295 + GTL.GetHashCode();
            hashCode = hashCode * -1521134295 + Sprites.GetHashCode();
            hashCode = hashCode * -1521134295 + Collisions.GetHashCode();
            hashCode = hashCode * -1521134295 + TiledObjects.GetHashCode();
            hashCode = hashCode * -1521134295 + Roofs.GetHashCode();
            hashCode = hashCode * -1521134295 + Events.GetHashCode();
            hashCode = hashCode * -1521134295 + ExternalExtra.GetHashCode();
            hashCode = hashCode * -1521134295 + ExternalMonster.GetHashCode();
            hashCode = hashCode * -1521134295 + ExternalNpc.GetHashCode();
            hashCode = hashCode * -1521134295 + DebugDots.GetHashCode();
            return hashCode;
        }
    }
}
