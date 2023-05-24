using Terraria.ModLoader.IO;

namespace FAC.Compontents.Workers
{
    public abstract class Worker<T> : Compontent
    {
        public int ColdTimer;
        public OverrideValue<int> MaxCold { get; protected set; }
        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag[nameof(MaxCold)] = MaxCold.OrigValue;
            tag[nameof(ColdTimer)] = ColdTimer;
        }
        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if (tag.TryGet(nameof(MaxCold), out int maxcold))
            {
                MaxCold = new(maxcold);
            }
            tag.TryGet(nameof(ColdTimer), out ColdTimer);
        }
    }
}
