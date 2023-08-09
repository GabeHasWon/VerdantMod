using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Particles;

namespace Verdant.Items.Verdant.Equipables;

class SproutInAGlove : ModItem
{
    bool offsetFlag = false;

    public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
    {
        const int Offset = 4;

        if (line.Name == "Verdant:OrLine")
        {
            line.BaseScale *= 0.9f;
            yOffset -= Offset;
            offsetFlag = true;
        }
        else if (offsetFlag)
        {
            yOffset += Offset;
            offsetFlag = false;
        }

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
                bool chance = Main.rand.NextDouble() < 0.333333f && !contained;

                if (chance || contained)
                {
                    if (chance)
                    {
                        Player.immune = true;
                        Player.immuneTime = 45;
                        Player.Heal(20);

                        var source = Player.GetSource_Accessory(equippedGlove);
                        Projectile.NewProjectile(source, Player.Center, new Vector2(0, -4), ModContent.ProjectileType<ApotheosisHand>(), 0, 0, Player.whoAmI);

                        for (int i = 0; i < 3; ++i)
                        {
                            var vel = new Vector2(Main.rand.NextFloat(4, 12), 0).RotatedByRandom(MathHelper.TwoPi);
                            Projectile.NewProjectile(source, Player.Center, vel, ModContent.ProjectileType<HealingParticle>(), 0, 0, Player.whoAmI);
                        }

                        SoundEngine.PlaySound(new SoundStyle("Verdant/Sounds/Blessing") with { PitchRange = (0f, 0.66f), Volume = 3f }, Player.Center);
                    }

                    return false;
                }
            }

            protectedWhoAmIs.Add(proj.whoAmI);
            return true;
        }
    }
}