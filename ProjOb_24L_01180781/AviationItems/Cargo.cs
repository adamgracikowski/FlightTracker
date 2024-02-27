﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public class Cargo
        : IAviationItem
    {
        public static string Acronym => _acronym;
        public UInt64 Id { get; private set; }
        public Single Weight { get; private set; }
        public string Code { get; private set; }
        public string Description { get; private set; }

        public Cargo(UInt64 id, Single weight, string code, string description)
        {
            Id = id;
            Weight = weight;
            Code = code;
            Description = description;
        }

        private static readonly string _acronym = "CA";
    }
}