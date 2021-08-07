using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Misc
{
    class HeartOfGrowth : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Heart of Growth");
            Tooltip.SetDefault("Permenantly increases max minions by one.\n'A heart that beats everliving.'");
        }

        public override void SetDefaults()
        {
            item.accessory = false;
            item.rare = ItemRarityID.Green;
            item.value = Item.sellPrice(gold: 50);
            item.consumable = true;
            item.width = 32;
            item.height = 28;

            item.useAnimation = item.useTime = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
        }

        public override bool UseItem(Player player)
        {
            player.GetModPlayer<VerdantPlayer>().heartOfGrowth = true;
            Vector2 top = new Vector2(player.Center.X, player.Bottom.Y + 30);

            for (int i = 0; i < 14; ++i)
            {
                float offset = (float)Math.Sin(i * 0.4f) * 40;

                float height = top.Y - (i * 15);
                Vector2 pos = top - new Vector2(offset, i * 15);
                Vector2 vel = Vector2.Normalize(pos - new Vector2(player.Center.X, height)) * (2.5f + (i / 24f));

                Dust d = Dust.NewDustPerfect(pos, 59, vel, 0, default, 1.25f - (i / 24f));

                pos = top + new Vector2(offset, -i * 15);
                vel = Vector2.Normalize(pos - new Vector2(player.Center.X, height)) * (2.5f + (i / 24f));
                Dust d2 = Dust.NewDustPerfect(pos, 59, vel, 0, default, 1.25f - (i / 24f));

                d.fadeIn = i / 12f * 3;
                d2.fadeIn = i / 12f * 3;
                d.noGravity = true;
                d2.noGravity = true;
            }
            return true;
        }
    }
}