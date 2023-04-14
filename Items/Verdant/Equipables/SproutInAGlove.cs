using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Equipables;

//[AutoloadEquip(EquipType.HandsOn)]
class SproutInAGlove : ModItem
{
    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Sprout in Gloves");
        Tooltip.SetDefault("Dodges hostile projectiles randomly\nDodging a projectile gives you a small amount of i-frames and heals you slightly" +
            "\nProjectiles can only hit you once, even if they pierce or bounce");
    }

    public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
    {
        const int Offset = 4;

        if (line.Name == "Verdant:OrLine")
        {
            line.BaseScale *= 0.8f;
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
        Item.value = Item.sellPrice(silver: 3);
        Item.accessory = true;
    }

    public override void UpdateEquip(Player player) => player.GetModPlayer<SproutInAGlobePlayer>().active = true;
}

class SproutInAGlobePlayer : ModPlayer
{
    public bool active = false;

    private List<int> protectedWhoAmIs = new();

    public override void ResetEffects()
    {
        active = false;
        protectedWhoAmIs.RemoveAll(x => !Main.projectile[x].active || Main.projectile[x].friendly);
    }

    public override bool CanBeHitByProjectile(Projectile proj)
    {
        bool contained = protectedWhoAmIs.Contains(proj.whoAmI);
        bool chance = Main.rand.NextDouble() < 0.2f && !contained;

        if (chance || contained)
        {
            if (chance)
            {
                Player.immune = true;
                Player.immuneTime = 30;
                CombatText.NewText(Player.getRect(), Color.White, "eg", true);
            }

            return false;
        }

        protectedWhoAmIs.Add(proj.whoAmI);
        return true;
    }
}