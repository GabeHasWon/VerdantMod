using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Foreground.Parallax;

namespace Verdant.Items.Verdant.Tools;

class VineWand : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Zipvine");
        Tooltip.SetDefault("Allows the user to build a vine\nThe vine works like a rope and can be used in any open space\nThese vines last 10 seconds");
    }

    public override void SetDefaults()
    {
        Item.Size = new Vector2(36, 28);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 2;
        Item.useTime = 2;
        Item.autoReuse = false;
        Item.rare = ItemRarityID.Green;
        Item.channel = true;
        Item.shoot = ModContent.ProjectileType<VineWandProjectile>();
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.maxStack = 1;
    }

    public override bool CanUseItem(Player player) => player.GetModPlayer<VinePulleyPlayer>().vineResource > 1; //2 or more because a vine of length 1 is bad

    public override void UpdateInventory(Player player)
    {
        Item.stack = player.GetModPlayer<VinePulleyPlayer>().vineResource;

        if (Item.stack <= 1)
            Item.stack = 1;
    }
}

public class VineWandProjectile : ModProjectile
{
    public override string Texture => "Verdant/Items/Verdant/Tools/VineWand";

    public ref float Timer => ref Projectile.ai[0];

    public EnchantedVine LastVine = null;

    private bool _init = false;

    public override void SetStaticDefaults() => DisplayName.SetDefault("Enchanted Vine");

    public override void SetDefaults()
    {
        Projectile.width = 42;
        Projectile.height = 24;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.aiStyle = -1;

        DrawHeldProjInFrontOfHeldItemAndArms = true;
    }

    public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */ => false;

    public override void AI()
    {
        if (!_init)
        {
            Timer = 1;
            LastVine = null;
            _init = true;
        }

        Player p = Main.player[Projectile.owner];
        p.heldProj = Projectile.whoAmI;
        p.direction = System.Math.Sign(Main.MouseWorld.X - p.Center.X);

        if (p.whoAmI != Main.myPlayer)
            return; //mp check (hopefully)

        Projectile.timeLeft++;
        Projectile.rotation = p.AngleFrom(Main.MouseWorld); //So it looks like the player is holding it properly
        Projectile.position = p.position - new Vector2(14, 0).RotatedBy(Projectile.rotation);
        Helper.ArmsTowardsMouse(p);

        if (!p.channel)
            Projectile.Kill();
        else
        {
            p.itemTime = 2;
            p.itemAnimation = 2;

            Timer--;

            if (Timer <= 0 && p.GetModPlayer<VinePulleyPlayer>().vineResource > 0)
            {
                LastVine = VineWandCommon.BuildVine(Projectile.owner, LastVine, false);

                Timer = 3;

                p.GetModPlayer<VinePulleyPlayer>().vineResource--;
                p.GetModPlayer<VinePulleyPlayer>().vineRegenCooldown = (15 * 60) + 240;
            }
        }
    }
}
