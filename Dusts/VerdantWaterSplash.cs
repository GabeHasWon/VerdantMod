using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Dusts
{
    public class VerdantWaterSplash : ModDust
    {
        public override void SetStaticDefaults() => UpdateType = DustID.Water;

        public override void OnSpawn(Dust dust)
        {
            dust.alpha = 170;
            dust.velocity *= 0.5f;
            dust.velocity.Y += 1f;
        }
    }
}