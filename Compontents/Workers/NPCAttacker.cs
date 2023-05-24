using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace FAC.Compontents.Workers
{
    public abstract class NPCAttacker : Worker<NPC>
    {
        public OverrideValue<int> Damage { get; protected set; }
        public OverrideValue<float> Knockback { get; protected set; }
        public OverrideValue<int> Crit { get; protected set; }
        public OverrideClass<DamageClass> DamageClass { get; protected set; }
        public override void Update()
        {
            if (!IsActive)
            {
                return;
            }
            if (ColdTimer > 0)
            {
                ColdTimer--;
            }
            Foundation.ApplyAuxiliaryTo(this);
            if (Foundation.TryGetTarget(out NPC target) && AttackNPC(target))
            {
                ColdTimer = MaxCold;
            }
        }
        public abstract bool AttackNPC(NPC target);
    }
}
