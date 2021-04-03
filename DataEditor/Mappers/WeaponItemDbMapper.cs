﻿using System.Collections.Generic;

namespace DispelTools.DataEditor.Mappers
{
    internal class WeaponItemDbMapper : Mapper
    {
        private const byte FILLER = 0x0;
        private const int NAME_STRING_MAX_LENGTH = 30;
        private const int DESCRIPTION_STRING_MAX_LENGTH = 202;
        private static List<ItemFieldDescriptor> descriptorList;
        protected override int PropertyItemSize => 71 * 4;

        protected override List<ItemFieldDescriptor> FileDescriptor { get { if (descriptorList == null) { descriptorList = CreateMap(); } return descriptorList; } }

        private List<ItemFieldDescriptor> CreateMap()
        {
            var list = new List<ItemFieldDescriptor>
            {
                createDescriptor("name", ItemFieldDescriptor.AsFixedString(NAME_STRING_MAX_LENGTH, FILLER)),
                createDescriptor("description", ItemFieldDescriptor.AsFixedString(DESCRIPTION_STRING_MAX_LENGTH, FILLER)),

                createDescriptor("Base price", ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor("PZ", ItemFieldDescriptor.AsInt16()),
                createDescriptor("PM", ItemFieldDescriptor.AsInt16()),
                createDescriptor("SIŁ",ItemFieldDescriptor.AsInt16()),
                createDescriptor("ZW", ItemFieldDescriptor.AsInt16()),
                createDescriptor("MM", ItemFieldDescriptor.AsInt16()),
                createDescriptor("TF", ItemFieldDescriptor.AsInt16()),
                createDescriptor("UNK", ItemFieldDescriptor.AsInt16()),
                createDescriptor("TRF", ItemFieldDescriptor.AsInt16()),
                createDescriptor("ATK", ItemFieldDescriptor.AsInt16()),
                createDescriptor("OBR", ItemFieldDescriptor.AsInt16()),
                createDescriptor("MAG", ItemFieldDescriptor.AsInt16()),
                createDescriptor("WYT",ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor("REQ SIŁ", ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor("REQ ZW", ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor("REQ MM", ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),
                createDescriptor(ItemFieldDescriptor.AsInt16()),

            };

            return list;
        }
    }
}
