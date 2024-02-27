﻿using ProjOb_24L_01180781.AviationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjOb_24L_01180781.Factories
{
    /// <summary>
    /// Represents common characteristics of all the factories, 
    /// which are used in the process of parsing .ftr files.
    /// </summary>
    public interface IAviationFactory
    {
        IAviationItem Create(string[] itemDetails);
    }
}
