﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.AviationItems.Interfaces
{
    public interface IHasId
    {
        ulong Id { get; set; }
        void UpdateId(ulong id)
        {
            Id = id;
        }
    }
}