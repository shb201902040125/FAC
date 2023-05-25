using FAC.Compontents.Finders;
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