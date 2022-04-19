using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Tools
{
    class VineWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Vine");
            Tooltip.SetDefault("Allows the user to build a vine");
        }

        public override void SetDefaults()
        {
            item.Size = new Vector2(36, 28);
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 2;
            item.useTime = 2;
            item.autoReuse = false;
            item.rare = ItemRarityID.Green;
            item.channel = true;
            item.shoot = ModContent.ProjectileType<VineWandProjectile>();
            item.noUseGraphic = true;
            item.noMelee = true;
        }
    }

    public class VineWandProjectile : ModProjectile
    {
        public override string Texture => "Verdant/Items/Verdant/Tools/VineWand";

        public ref float Timer => ref projectile.ai[0];
        public ref float LastVineIndex => ref projectile.ai[1];

        public Projectile LastVine => Main.projectile[(int)LastVineIndex];

        private bool _init = false;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Enchanted Vine");

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.ranged = true;
            projectile.aiStyle = -1;

            drawHeldProjInFrontOfHeldItemAndArms = true;
        }

        public override bool CanDamage() => false;

        public override void AI()
        {
            if (!_init)
            {
                Timer = 1;
                LastVineIndex = -1;
                _init = true;
            }

            Player p = Main.player[projectile.owner];
            p.heldProj = projectile.whoAmI;

            if (p.whoAmI != Main.myPlayer)
                return; //mp check (hopefully)

            projectile.timeLeft++;
            projectile.rotation = Vector2.Normalize(p.MountedCenter - Main.MouseWorld).ToRotation() - MathHelper.Pi; //So it looks like the player is holding it properly
            projectile.Center = p.Center - new Vector2(16, 0).RotatedBy(-projectile.rotation);
            Helper.ArmsTowardsMouse(p);

            if (!p.channel)
                projectile.Kill();
            else
            {
                p.itemTime = 2;
                p.itemAnimation = 2;

                Timer--;

                if (Timer <= 0)
                {
                    int lastInd = (int)LastVineIndex;

                    if (LastVineIndex == -1)
                        LastVineIndex = Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<VineWandVine>(), 0, 0, p.whoAmI, 0, 0);
                    else
                    {
                        Vector2 pos = LastVine.Center + (LastVine.DirectionTo(Main.MouseWorld) * 14);
                        LastVineIndex = Projectile.NewProjectile(pos, Vector2.Zero, ModContent.ProjectileType<VineWandVine>(), 0, 0, p.whoAmI, 0, 0);
                    }

                    if (lastInd != -1)
                    {
                        (Main.projectile[lastInd].modProjectile as VineWandVine).nextVine = (int)LastVineIndex;
                        (LastVine.modProjectile as VineWandVine).priorVine = lastInd;
                    }

                    Timer = 3;
                }
            }
        }
    }
}
