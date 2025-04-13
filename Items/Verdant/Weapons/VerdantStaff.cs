using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Projectiles.Minion;
using Verdant.Tiles;

namespace Verdant.Items.Verdant.Weapons
{
    class VerdantStaff : ModItem
    {
        public override void SetStaticDefaults() => Item.staff[Type] = true;

        public override void SetDefaults()
        {
            QuickItem.SetStaff(this, 48, 48, ModContent.ProjectileType<VerdantHealingMinion>(), 9, 0, 24, 0, 0, ItemRarityID.Green);
            Item.value = Item.buyPrice(0, 0, 5, 0);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.dedServ || Main.netMode != NetmodeID.SinglePlayer)
            {
                TooltipLine line = new(Mod, "Verdant Staff", Language.GetTextValue("Mods.Verdant.Items.VerdantStaff.MultiplayerTooltip"));
                tooltips.Add(line);
            }
        }

        public override void ModifyShootStats(Player p, ref Vector2 position, ref Vector2 v, ref int t, ref int d, ref float k) => position = Main.MouseWorld;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(ModContent.BuffType<Buffs.Minion.HealingFlowerBuff>(), 2000);
            return true;
        }

        public override bool CanShoot(Player player)
        {
            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (projectile.type == ModContent.ProjectileType<VerdantHealingMinion>() && projectile.owner == player.whoAmI)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                for (int i = -2; i < 2; ++i)
                    for (int j = -3; j < 0; ++j)
                        if (TileHelper.SolidTile(Helper.MouseTile().X + i, Helper.MouseTile().Y + j))
                            return false;

                Projectile proj = null;

                foreach (var projectile in Main.ActiveProjectiles)
                {
                    if (projectile.type == ModContent.ProjectileType<VerdantHealingMinion>() && projectile.owner == player.whoAmI)
                    {
                        proj = projectile;
                        break;
                    }
                }

                if (proj is not null)
                {
                    if (player.HasBuff(ModContent.BuffType<Buffs.Minion.HealingFlowerBuff>()))
                        (proj.ModProjectile as VerdantHealingMinion).goPosition = Main.MouseWorld - new Vector2(24, 24);

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
                    }

                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
