using Terraria.ModLoader.IO;

namespace FAC.Compontents.Finders
{
    public abstract class RangeFinder<T> : Finder<T>
    {
        public OverrideValue<float> MaxRange { get; protected set; } = new(800);
        public OverrideValue<float> MinRange { get; protected set; } = new(0);
        public override void Update()
        {
            MaxRange.Reset();
            MinRange.Reset();
        }
        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
            tag[nameof(MaxRange)] = MaxRange.OrigValue;
            tag[nameof(MinRange)] = MinRange.OrigValue;
        }
        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
            if (tag.TryGet(nameof(MaxRange), out float max))
            {
                MaxRange = new(max);
            }
            if (tag.TryGet(nameof(MinRange), out float min))
            {
                MinRange = new(min);
            }
        }
    }
}
