using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Equipables
{
	internal class FlotieOfWrathItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curious Skull");
			Tooltip.SetDefault("Summons a Flotie of Wrath\nGives off a tiny amount of light");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Fish);
			Item.shoot = ModContent.ProjectileType<FlotieOfWrath>();
			Item.buffType = ModContent.BuffType<Buffs.Pet.FlotiePetBuff>();
			Item.UseSound = SoundID.NPCDeath6; 
			Item.rare = ItemRarityID.Yellow;
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
				player.AddBuff(Item.buffType, 3600, true);
		}

		public override bool CanUseItem(Player player) => player.miscEquips[1].IsAir;
	}
}
