using DispelTools.Components.CustomPropertyGridControl;
using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class MonsterDbMapper : Mapper
    {
        private const byte FILLER = 0x0;
        protected override int PropertyItemSize => 40 * 4;

        protected override bool HaveCounterOnBeginning => false;

        protected override List<ItemFieldDescriptor> CreateDescriptors()
        {
            var builder = new FileDescriptorBuilder();
            builder.Add("name", ItemFieldDescriptor.AsFixedString(24, FILLER, Field.DisplayType.TEXT_KOR));

            builder.Add("PZ MAX", ItemFieldDescriptor.AsInt32());
            builder.Add("PZ MIN", ItemFieldDescriptor.AsInt32());
            builder.Add("PM MAX", ItemFieldDescriptor.AsInt32());
            builder.Add("PM MIN", ItemFieldDescriptor.AsInt32());

            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());

            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add("ATK", ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());

            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());
            builder.Add("isUndead", ItemFieldDescriptor.AsInt32(), "0 or 1");

            builder.Add("isAlive", ItemFieldDescriptor.AsInt32(), "0 or 1, golem is not alive and not undead");
            builder.Add("Attack type?", ItemFieldDescriptor.AsInt32(), "goblin and chicken = 1,archers = 2, worm bot no zombie =3, deer and dog = 5");
            builder.Add("EXP MAX", ItemFieldDescriptor.AsInt32());
            builder.Add("EXP MIN", ItemFieldDescriptor.AsInt32());

            builder.Add("GOLD MAX", ItemFieldDescriptor.AsInt32());
            builder.Add("GOLD MIN", ItemFieldDescriptor.AsInt32());
            builder.Add("9 or 10", ItemFieldDescriptor.AsInt32(), "only goblin king have 10");
            builder.Add("1 or 6 if archer", ItemFieldDescriptor.AsInt32());

            builder.Add("magic stat1", ItemFieldDescriptor.AsInt32());
            builder.Add("magic stat2", ItemFieldDescriptor.AsInt32());
            builder.Add("magic stat3", ItemFieldDescriptor.AsInt32());
            builder.Add("redDragon, balrog, beholder, = 1", ItemFieldDescriptor.AsInt32());

            builder.Add("always 1", ItemFieldDescriptor.AsInt32());
            builder.Add("not magic effect stat1", ItemFieldDescriptor.AsInt32());
            builder.Add("not magic effect stat2", ItemFieldDescriptor.AsInt32());
            builder.Add("not magic effect stat3", ItemFieldDescriptor.AsInt32());

            builder.Add("always 10", ItemFieldDescriptor.AsInt32());
            builder.Add(ItemFieldDescriptor.AsInt32());

            return builder.Build();
        }
    }
}
