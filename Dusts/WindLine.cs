using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Verdant.Dusts
{
    public class WindLine : ModDust
    {
		public override void OnSpawn(Dust dust)
		{
			dust.velocity = Vector2.Zero;
			dust.noGravity = true;
			dust.noLight = true;
			dust.scale *= Main.rand.NextFloat(0.9f, 1.25f);
			dust.alpha = 150;
		}

		public override bool Update(Dust dust)
		{
            if (Collision.SolidCollision(dust.position, 2, 6))
            {
                dust.active = false;
                return false;
            }

            dust.position += dust.velocity;
            dust.velocity *= 0.94f;
            dust.rotation = 0;
			dust.alpha += 5;

			if (dust.alpha > 254)
				dust.active = false;
            return false;
		}
	}
}