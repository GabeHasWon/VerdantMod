using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Verdant.World;

namespace Verdant.Waters
{
	public class VerdantWaterStyle : ModWaterStyle
	{
        public override bool ChooseWaterStyle() => VerdantWorld.VerdantTiles >= 50;

		public override int ChooseWaterfallStyle() => mod.GetWaterfallStyleSlot("VerdantWaterfallStyle");

		public override int GetSplashDust() => mod.DustType("VerdantWaterSplash");

		public override int GetDropletGore() => mod.GetGoreSlot("Gores/Verdant/VerdantDroplet");

		public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
			r = 0.675f;
			g = 0.783f;
			b = 0.9f;
		}

		public override Color BiomeHairColor() => new Color(33, 124, 22);
	}
}