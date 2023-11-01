using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Particles;

namespace Verdant.Items.Verdant.Misc;

[Sacrifice(1)]
class CrystalHeart : ModItem
{
    public override void SetStaticDefaults() => ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<HeartOfGrowth>();

    public override void SetDefaults()
    {
        Item.accessory = false;
        Item.rare = ItemRarityID.Yellow;
        Item.value = 0;
        Item.consumable = false;
        Item.width = 32;
        Item.height = 28;
        Item.useAnimation = Item.useTime = 20;
        Item.useStyle = ItemUseStyleID.HoldUp;
    }

    public override bool CanUseItem(Player player) => !player.GetModPlayer<VerdantPlayer>().crystalHeart;

    public override bool? UseItem(Player player)
    {
        player.GetModPlayer<VerdantPlayer>().crystalHeart = true;

        for (int i = 0; i < 60; ++i)
        {
            float magnitude = Main.rand.NextBool() ? Main.rand.NextFloat(20, 26) : Main.rand.NextFloat(6, 10);
            Vector2 vel = new Vector2(magnitude, 0).RotatedByRandom(MathHelper.Pi * 0.8f).RotatedBy(MathHelper.PiOver2);
            int type = ModContent.ProjectileType<HealingParticle>();
            int p = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, vel, type, 0, 0, player.whoAmI, 0, 0);
            Color col = new(Main.rand.NextFloat(0.2f, 0.6f), Main.rand.NextFloat(0.2f, 0.6f), Main.rand.NextFloat(0.2f, 0.6f));
            (Main.projectile[p].ModProjectile as HealingParticle).drawCol = col;
            (Main.projectile[p].ModProjectile as HealingParticle).Timer = Main.rand.Next(-80, -10);
        }

        for (int i = 0; i < 2; ++i)
        {
            int type = ModContent.ProjectileType<ApotheosisHand>();
            int p = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, new Vector2(0, -8 * ((i * 0.75f) + 1)), type, 0, 0, player.whoAmI);
            Main.projectile[p].scale = (i * 0.5f) + 1;
        }
        return true;
    }
}