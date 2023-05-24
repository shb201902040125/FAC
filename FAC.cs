using System;
using Terraria.ModLoader;

namespace FAC
{
	public class FAC : Mod
	{
		public FAC()
		{
			Instance = this;
		}
		public static FAC Instance { get; private set; }
		public static Action OnModUnload;
        public override void Unload()
        {
            if(OnModUnload is not null)
			{
				OnModUnload();
				OnModUnload = null;
			}
        }
    }
}