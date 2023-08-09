using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Equipables;

internal class FlotieOfWrathItem : ModItem
{
	public override void SetDefaults()
	{
		Item.CloneDefaults(ItemID.Fish);
		Item.shoot = ModContent.ProjectileType<FlotieOfWrath>();
		Item.buffType = ModContent.BuffType<Buffs.Pet.FlotiePetBuff>();
		Item.UseSound = SoundID.NPCDeath6; 
		Item.rare = ItemRarityID.Yellow;
		Item.Size = new Vector2(32, 30);
	}

	public override void UseStyle(Player player, Rectangle heldItemFrame)
	{
		if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			player.AddBuff(Item.buffType, 3600, true);
	}

	public override bool CanUseItem(Player player) => player.miscEquips[1].IsAir;
	public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.WorkBenches, 1, (ModContent.ItemType<LushLeaf>(), 8), (ModContent.ItemType<Lightbulb>(), 4), (ItemID.Bone, 6), (ModContent.ItemType<RedPetal>(), 1));
}