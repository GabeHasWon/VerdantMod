using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Equipables
{
    class Lightbloom : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightbloom");
            Tooltip.SetDefault("Increases life regeneration when in light\nThe stronger the light, the larger the increase");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 3);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Color col = Lighting.GetColor((int)(player.MountedCenter.X / 16f), (int)(player.MountedCenter.Y / 16f));
            int val = 2;
            int total = col.R + col.G + col.B;

            if (total < 200) 
                val = 0;
            else if (total < 400) 
                val = 1;

            if (!hideVisual && Main.rand.Next(80) < val * 2)
            {
                for (int i = 0; i < val; ++i)
                {
                    float magnitude = Main.rand.NextFloat(0.25f, 4);

                    Vector2 pos = player.Center + new Vector2(50 * magnitude, 0).RotatedByRandom(MathHelper.TwoPi);
                    Dust d = Dust.NewDustPerfect(pos, 59, Vector2.Normalize(player.Center - pos) * 2.5f * magnitude, 0, default, 0.12f);
                    d.fadeIn = 1.14f;
                    d.noLight = true;
                    d.noGravity = true;
                }
            }

            player.lifeRegen += val;
        }
    }
}