using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Verdant.Dusts
{
    public class PuffDust : ModDust
    {
		public override void OnSpawn(Dust dust)
		{
			dust.velocity = Microsoft.Xna.Framework.Vector2.Zero;
			dust.noGravity = true;
			dust.noLight = true;
			dust.scale *= 1.5f;
			dust.alpha = 250;
			dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += 0.04f;
			dust.velocity.X = (float)Math.Sin(dust.rotation) * 0.8f;
			dust.velocity.Y += 0.005f;

			if (dust.rotation > 8)
				dust.alpha += 2;
			else if (dust.alpha > 2)
				dust.alpha -= 2;

			if (dust.alpha > 254)
				dust.active = false;
            return false;
		}
	}
}