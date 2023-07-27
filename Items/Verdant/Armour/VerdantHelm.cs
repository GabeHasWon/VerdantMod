using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;

namespace Verdant.Items.Verdant.Armour
{
    [AutoloadEquip(EquipType.Head)]
    public class VerdantHelm : ModItem
	{
        public override void Load()
        {
            VerdantPlayer.OnRespawnEvent += OnRespawn;
            VerdantPlayer.HitByNPCEvent += VerdantPlayer_HitByNPCEvent;
        }

        private void VerdantPlayer_HitByNPCEvent(Player p, NPC npc, Player.HurtInfo hurtInfo)
        {
            if (p.ArmourSetEquipped(ModContent.ItemType<VerdantHelm>(), ModContent.ItemType<VerdantChestplate>(), ModContent.ItemType<VerdantLeggings>()))
            {
                int r = Main.rand.Next(1, 3);
                for (int i = 0; i < r; ++i)
                {
                    int d = Main.rand.NextBool(2) ? Mod.Find<ModGore>("PinkPetalFalling").Type : Mod.Find<ModGore>("RedPetalFalling").Type;
                    Gore.NewGore(p.GetSource_OnHurt(npc), p.position + new Vector2(Main.rand.Next(p.width), Main.rand.Next(p.height)), Vector2.Zero, d, 1f);
                }
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Growth Headdress");
            // Tooltip.SetDefault("+5% increased minion damage\nGives off a very small amount of light");
        }

        public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<VerdantChestplate>() && legs.type == ModContent.ItemType<VerdantLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = $"+4% minion damage\n+1 max minion\nReduces fall speed - Hold DOWN to fall faster\nUpon respawning, heals you for 50 health";

            player.maxMinions++;
            player.GetDamage(DamageClass.Summon) *= 1.04f;

            if (!player.controlDown)
                player.maxFallSpeed *= 0.69f; //LMAO

            if (Math.Abs(player.velocity.X) > 0.5f && Main.rand.NextBool(140)) //Spawn gores
            {
                int random = Main.rand.Next(3);
                int gore = Mod.Find<ModGore>("PinkPetalFalling").Type;

                if (random == 0) 
                    gore = Mod.Find<ModGore>("RedPetalFalling").Type;
                if (random == 1) 
                    gore = Mod.Find<ModGore>("LushLeaf").Type;

                Gore.NewGore(player.GetSource_Accessory(Item), player.Center + new Vector2(0), new Vector2(0), gore, 1f);
            }
        }

        private void OnRespawn(Player p)
        {
            if (p.ArmourSetEquipped(ModContent.ItemType<VerdantHelm>(), ModContent.ItemType<VerdantChestplate>(), ModContent.ItemType<VerdantLeggings>())) //Heals when 
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

        public override void UpdateEquip(Player player)
		{
            player.GetDamage(DamageClass.Summon) *= 1.05f;
            Lighting.AddLight(player.Center - new Vector2(0, 10), new Vector3(0.1f, 0.03f, 0.06f) * 7.5f);
        }

        public override void AddRecipes()
        {
            Recipe m = CreateRecipe();
            m.AddIngredient(ModContent.ItemType<RedPetal>(), 12);
            m.AddIngredient(ModContent.ItemType<VerdantStrongVineMaterial>(), 2);
            m.AddIngredient(ModContent.ItemType<Materials.LushLeaf>(), 10);
            m.AddIngredient(ModContent.ItemType<Lightbulb>(), 1);
            m.AddIngredient(ModContent.ItemType<YellowBulb>(), 2);
            m.AddTile(TileID.Anvils);
            m.Register();
        }
    }
}