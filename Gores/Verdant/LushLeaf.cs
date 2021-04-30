using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Verdant.Gores.Verdant
{
	public class LushLeaf : ModGore
	{
		public override void OnSpawn(Gore gore) {
			gore.velocity = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() * MathHelper.TwoPi);
			gore.numFrames = 8;
			gore.frame = (byte)Main.rand.Next(8);
			gore.frameCounter = (byte)Main.rand.Next(8);
            gore.timeLeft = 805;
            updateType = 910;
        }
    }
}