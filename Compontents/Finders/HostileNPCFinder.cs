using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace FAC.Compontents.Finders
{
    internal class HostileNPCFinder : RangeFinder<NPC>
    {
        public override void SetDefaults()
        {
            Item.width = Item.height = 24;
            MaxRange = new(480);
            MinRange = new(0);
        }
        public override bool TryFindTarget(out NPC target)
        {
            target = null;
            if(Foundation is null)
            {
                return false;
            }
            float dis = MaxRange;
            foreach(NPC  npc in Main.npc)
            {
                if (!npc.active || npc.friendly || npc.dontTakeDamage || npc.dontTakeDamageFromHostiles)
                {
                    continue;
                }
                float d = Vector2.Distance(Foundation.Center, npc.Center);
                if (d < dis && d > MinRange)
                {
                    target = npc;
                    dis = d;
                }
            }
            return target is not null;
        }
    }
}
