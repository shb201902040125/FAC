using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace FAC
{
    public abstract class Compontent : ModItem
    {
        public Foundation Foundation { get; internal set; }
        public virtual bool IsVisible => true;
        public virtual bool IsActive => true;
        public virtual void Update() { }
        public virtual bool CompatibleWith(Compontent other) => true;
        public virtual void OnEquip() { }
        public virtual void Draw() { }
    }
}
