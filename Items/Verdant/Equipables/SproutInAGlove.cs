using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Particles;

namespace Verdant.Items.Verdant.Equipables;

//[AutoloadEquip(EquipType.HandsOn)]
class SproutInAGlove : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Sprout in Gloves");
        Tooltip.SetDefault("Dodges hostile projectiles randomly (33% chance)\nDodging a projectile gives you a small amount of i-frames and heals you slightly" +
            "\nProjectiles can only hit you once, even if they pierce or bounce");
    }

    public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
    {
        const int Offset = 4;

        if (line.Name == "Verdant:OrLine")
        {
            line.BaseScale *= 0.9f;
            yOffset -= Offset;
        }

        if (line.Name == "Equipable")
            yOffset += Offset;
        return true;
    }

    public override void ModifyTooltips(List<TooltipLine> lines) => lines.Insert(1, new TooltipLine(Mod, "Verdant:OrLine", "[c/969696:Or: the Apotheosis's Blessing]"));

    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(gold: 50);
        Item.accessory = true;
    }

    public override void UpdateEquip(Player player) => player.GetModPlayer<SproutInAGlovePlayer>().equippedGlove = Item;

    private class SproutInAGlovePlayer : ModPlayer
    {
        public bool Active => equippedGlove is not null;

        public Item equippedGlove = null;

        private List<int> protectedWhoAmIs = new();

        public override void ResetEffects()
        {
            equippedGlove = null;
            protectedWhoAmIs.RemoveAll(x => !Main.projectile[x].active || Main.projectile[x].friendly);
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (Active)
            {
                bool contained = protectedWhoAmIs.Contains(proj.whoAmI);
                bool chance = Main.rand.NextDouble() < 0.33333f && !contained;

                if (chance || contained)
                {
                    if (chance)
                    {
                        Player.immune = true;
                        Player.immuneTime = 60;
                        Player.Heal(20);

                        var source = Player.GetSource_Accessory(equippedGlove);
                        Projectile.NewProjectile(source, Player.Center, new Vector2(0, -4), ModContent.ProjectileType<ApotheosisHand>(), 0, 0, Player.whoAmI);

                        for (int i = 0; i < 3; ++i)
                        {
                            var vel = new Vector2(Main.rand.NextFloat(4, 12), 0).RotatedByRandom(MathHelper.TwoPi);
                            Projectile.NewProjectile(source, Player.Center, vel, ModContent.ProjectileType<HealingParticle>(), 0, 0, Player.whoAmI);
                        }
                    }

                    return false;
                }
            }

            protectedWhoAmIs.Add(proj.whoAmI);
            return true;
        }
    }
}