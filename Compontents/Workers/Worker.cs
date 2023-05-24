﻿using FAC.Compontents.Finders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FAC.Compontents.Workers
{
    public abstract class Worker<T> : Compontent
    {
        public int ColdTimer;
        public OverrideValue<int> MaxCold { get; protected set; }
    }
}
