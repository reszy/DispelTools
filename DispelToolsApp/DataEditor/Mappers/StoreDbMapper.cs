﻿using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class StoreDbMapper : Mapper
    {
        private const byte FILLER = 0x0;
        internal override int PropertyItemSize => 237 * 4;
        protected override List<ItemFieldDescriptor> CreateDescriptors()
        {
            var builder = new FileDescriptorBuilder();
            builder.Add("name", ItemFieldDescriptor.AsFixedString(32, FILLER));
            builder.Add(ItemFieldDescriptor.AsByteArray(148), true);
            builder.Add("text", ItemFieldDescriptor.AsFixedString(512, FILLER));
            builder.Add("haggle success", ItemFieldDescriptor.AsFixedString(128, FILLER));
            builder.Add("haggle fail", ItemFieldDescriptor.AsFixedString(128, FILLER));

            return builder.Build();
        }
    }
}
