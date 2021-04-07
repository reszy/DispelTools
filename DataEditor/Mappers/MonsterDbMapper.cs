using System.Collections.Generic;
using DispelTools.Components.CustomPropertyGridControl;

namespace DispelTools.DataEditor.Mappers
{
    internal class MonsterDbMapper : Mapper
    {
        private const byte FILLER = 0x0;
        private static List<ItemFieldDescriptor> descriptorList;
        protected override int PropertyItemSize => 40 * 4;

        protected override List<ItemFieldDescriptor> FileDescriptor { get { if (descriptorList == null) { descriptorList = CreateMap(); } return descriptorList; } }

        protected override bool HaveCounterOnBeginning => false;

        private List<ItemFieldDescriptor> CreateMap()
        {
            var list = new List<ItemFieldDescriptor>
            {
                createDescriptor("name", ItemFieldDescriptor.AsFixedString(24, FILLER, Field.DisplayType.TEXT_KOR)),

                createDescriptor("PZ MAX", ItemFieldDescriptor.AsInt32()),
                createDescriptor("PZ MIN", ItemFieldDescriptor.AsInt32()),
                createDescriptor("PM MAX", ItemFieldDescriptor.AsInt32()),
                createDescriptor("PM MIN", ItemFieldDescriptor.AsInt32()),

                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),

                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor("ATK", ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),

                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),
                createDescriptor("isUndead", ItemFieldDescriptor.AsInt32(), "0 or 1"),

                createDescriptor("isAlive", ItemFieldDescriptor.AsInt32(), "0 or 1, golem is not alive and not undead"),
                createDescriptor("atack type?", ItemFieldDescriptor.AsInt32(), "goblin and chicken = 1,archers = 2, worm bot no zombie =3, deer and dog = 5" ),
                createDescriptor("EXP MAX", ItemFieldDescriptor.AsInt32()),
                createDescriptor("EXP MIN", ItemFieldDescriptor.AsInt32()),

                createDescriptor("GOLD MAX",ItemFieldDescriptor.AsInt32()),
                createDescriptor("GOLD MIN",ItemFieldDescriptor.AsInt32()),
                createDescriptor("9 or 10", ItemFieldDescriptor.AsInt32(), "only goblin king have 10"),
                createDescriptor("1 or 6 if archer", ItemFieldDescriptor.AsInt32()),

                createDescriptor("magic stat1", ItemFieldDescriptor.AsInt32()),
                createDescriptor("magic stat2", ItemFieldDescriptor.AsInt32()),
                createDescriptor("magic stat3", ItemFieldDescriptor.AsInt32()),
                createDescriptor("redDragon, balrog, beholder, = 1", ItemFieldDescriptor.AsInt32()),

                createDescriptor("always 1", ItemFieldDescriptor.AsInt32()),
                createDescriptor("not magic effect stat1", ItemFieldDescriptor.AsInt32()),
                createDescriptor("not magic effect stat2", ItemFieldDescriptor.AsInt32()),
                createDescriptor("not magic effect stat3", ItemFieldDescriptor.AsInt32()),

                createDescriptor("always 10", ItemFieldDescriptor.AsInt32()),
                createDescriptor(ItemFieldDescriptor.AsInt32()),

            };

            return list;
        }
    }
}
