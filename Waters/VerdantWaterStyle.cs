using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Verdant.World;

namespace Verdant.Waters
{
	public class VerdantWaterStyle : ModWaterStyle
	{
		public override int ChooseWaterfallStyle() => ModContent.Find<ModWaterfallStyle>("SpiritMod/SpiritWaterfallStyle").Slot;
		public override int GetSplashDust() => Mod.Find<ModDust>("VerdantWaterSplash").Type;
		public override int GetDropletGore() => ModContent.Find<ModGore>("VerdantDroplet").Type;
		public override void LightColorMultiplier(ref float r, ref float g, ref float b)
        {
			r = 0.675f;
			g = 0.783f;
			b = 0.9f;
		}

		public override Color BiomeHairColor() => new Color(33, 124, 22);
	}
}