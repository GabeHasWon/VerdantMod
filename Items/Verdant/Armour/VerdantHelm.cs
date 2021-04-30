using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Gores.Verdant;
using Verdant.Items.Verdant.Blocks;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class VerdantHelm : ModItem
	{
        public override bool Autoload(ref string name)
        {
            VerdantPlayer.OnRespawnEvent += OnRespawn;
            VerdantPlayer.HitByNPCEvent += OnHit;
            return base.Autoload(ref name);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Growth Headdress");
            Tooltip.SetDefault("+5% increased minion damage\nGives off a very small amount of light");
        }

        public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.defense = 5;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<VerdantChestplate>() && legs.type == ModContent.ItemType<VerdantLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = $"+4% minion damage\n+1 max minion\nReduces fall speed - Hold DOWN to fall faster\nUpon respawning, heals you for 50 health";

            player.maxMinions++;
            player.minionDamage *= 1.04f;
            if (!player.controlDown)
                player.maxFallSpeed *= 0.70f;
            if (Math.Abs(player.velocity.X) > 0.5f && Main.rand.Next(68) == 0)
            {
                int random = Main.rand.Next(3);
                int gore = mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling");
                if (random == 0) gore = mod.GetGoreSlot("Gores/Verdant/RedPetalFalling");
                if (random == 1) gore = mod.GetGoreSlot("Gores/Verdant/LushLeaf");
                Gore.NewGore(player.Center + new Vector2(0), new Vector2(0), gore, 1f);
            }
        }

        private void OnRespawn(Player p)
        {
            if (p.ArmourSetEquipped(ModContent.ItemType<VerdantHelm>(), ModContent.ItemType<VerdantChestplate>(), ModContent.ItemType<VerdantLeggings>()))
            {
                if (p.statLifeMax2 < p.statLife + 50)
                {
                    p.HealEffect(p.statLifeMax2 - p.statLife);
                    p.statLife = p.statLifeMax2;
                }
                else
                {
                    p.HealEffect(50);
                    p.statLife += 50;
                }
            }
        }

        private void OnHit(Player p, NPC npc, int damage, bool crit)
        {
            if (p.ArmourSetEquipped(ModContent.ItemType<VerdantHelm>(), ModContent.ItemType<VerdantChestplate>(), ModContent.ItemType<VerdantLeggings>()))
            {
                int r = Main.rand.Next(1, 3);
                for (int i = 0; i < r; ++i)
                {
                    int d = Main.rand.Next(2) == 0 ? mod.GetGoreSlot("Gores/Verdant/PinkPetalFalling") : mod.GetGoreSlot("Gores/Verdant/RedPetalFalling");
                    Gore.NewGore(p.position + new Vector2(Main.rand.Next(p.width), Main.rand.Next(p.height)), Vector2.Zero, d, 1f);
                }
            }
        }

        public override void UpdateEquip(Player player)
		{
            player.minionDamage *= 1.05f;
            Lighting.AddLight(player.position + new Vector2(player.width / 2), new Vector3(0.1f, 0.03f, 0.06f) * 6f);
        }

        public override void AddRecipes()
        {
            ModRecipe m = new ModRecipe(mod);
            m.AddIngredient(ModContent.ItemType<RedPetal>(), 12);
            m.AddIngredient(ModContent.ItemType<VerdantStrongVineMaterial>(), 2);
            m.AddIngredient(ModContent.ItemType<Materials.LushLeaf>(), 10);
            m.AddIngredient(ModContent.ItemType<Lightbulb>(), 1);
            m.AddIngredient(ModContent.ItemType<YellowBulb>(), 2);
            m.AddTile(TileID.Anvils);
            m.SetResult(this);
            m.AddRecipe();
        }
    }
}