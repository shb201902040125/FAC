using FAC.Compontents.Finders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace FAC.Compontents.Auxiliaries
{
    internal class NPCRangeIncrease : Auxiliary<RangeFinder<NPC>>
    {
        public override void Apply(Compontent compontent)
        {
            ((RangeFinder<NPC>)compontent).MaxRange.Value += 80;
        }
    }
}