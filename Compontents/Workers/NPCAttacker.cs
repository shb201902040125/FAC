using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FAC.Compontents.Workers
{
    public abstract class NPCAttacker : Worker<NPC>
    {
        public OverrideValue<int> Damage { get; protected set; }
        public OverrideValue<float> Knockback { get; protected set; }
        public OverrideValue<int> Crit { get; protected set; }
        public OverrideClass<DamageClass> DamageType { get; protected set; }
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
        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag[nameof(Damage)] = Damage.OrigValue;
            tag[nameof(Knockback)] = Knockback.OrigValue;
            tag[nameof(Crit)] = Crit.OrigValue;
            tag[nameof(DamageType)] = DamageType.OrigValue.FullName;
        }
        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if (tag.TryGet(nameof(Damage), out int damage))
            {
                Damage = new(damage);
            }
            if (tag.TryGet(nameof(Knockback), out float knockback))
            {
                Knockback = new(knockback);
            }
            if (tag.TryGet(nameof(Crit), out int crit))
            {
                Crit = new(crit);
            }
            if (tag.TryGet(nameof(DamageType), out string dcname))
            {
                var ds = (List<DamageClass>)typeof(DamageClassLoader).GetField("DamageClasses", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                DamageType = new(ds.FirstOrDefault(d => d.FullName == dcname, DamageClass.Default));
            }
        }
    }
}
