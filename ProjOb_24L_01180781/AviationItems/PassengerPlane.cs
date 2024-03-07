﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems
{
    public class PassengerPlane
        : Plane, IAviationItem
    {
        public ClassSize ClassSize { get; private set; }
        public PassengerPlane(UInt64 id, string serial, string country, string model, ClassSize classSize)
            : base(id, serial, country, model)
        {
            ClassSize = classSize;
        }
    }
}
