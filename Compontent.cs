using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FAC
{
    public abstract class Compontent : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => false;
        public Foundation Foundation { get; internal set; }
        public virtual bool IsVisible => true;
        public virtual bool IsActive => true;
        public virtual void Update() { }
        public virtual bool CompatibleWith(Compontent other) => true;
        public virtual void OnEquip() { }
        public virtual void Draw() { }
        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            if (Foundation is not null)
            {
                tag[nameof(Foundation)] = Foundation.UID.ToString();
            }
        }
        public override void LoadData(TagCompound tag)
        {
            if (tag.TryGet(nameof(Foundation), out string uid))
            {
                foreach (TileEntity te in TileEntity.ByID.Values)
                {
                    if (te is Foundation f && f.UID.ToString() == uid)
                    {
                        Foundation = f;
                        break;
                    }
                }
            }
        }
    }
}
