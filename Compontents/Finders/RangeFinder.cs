using FAC.Compontents.Workers;
using Terraria.ModLoader.IO;

namespace FAC.Compontents.Finders
{
    public abstract class RangeFinder<T> : Worker<T>
    {
        public OverrideValue<float> MaxRange { get; protected set; }
        public OverrideValue<float> MinRange { get; protected set; }
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
