using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant;
using Verdant.Items.Verdant.Blocks;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Armour
{
    [AutoloadEquip(EquipType.Legs)]
    public class VerdantLeggings : ModItem
	{
        public override bool Autoload(ref string name)
        {
            VerdantPlayer.FloorVisualEvent += FloorVisuals;
            return base.Autoload(ref name);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Growth Leggings");
            Tooltip.SetDefault("Increased regen\n+1 max minion");
        }

        public override void SetDefaults()
		{
			item.width = 18;
            item.height = 12;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.defense = 4;
            item.lifeRegen = 2;
        }

		public override void UpdateEquip(Player player)
		{
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            ModRecipe m = new ModRecipe(mod);
            m.AddIngredient(ModContent.ItemType<VerdantStrongVineMaterial>(), 4);
            m.AddIngredient(ModContent.ItemType<RedPetal>(), 4);
            m.AddIngredient(ModContent.ItemType<PinkPetal>(), 4);
            m.AddIngredient(ModContent.ItemType<LushLeaf>(), 16);
            m.AddIngredient(ModContent.ItemType<YellowBulb>(), 2);
            m.AddTile(TileID.Anvils);
            m.SetResult(this);
            m.AddRecipe();
        }

        public void FloorVisuals(Player p, int type)
        {
            //if (p.ArmourEquipped(item))
            //{
            //    Point t = p.TileCoordsBottomCentred(); //oh god why does this not work

            //    if (!Framing.GetTileSafely(t.X, t.Y - 1).active() && Framing.GetTileSafely(t.X, t.Y - 1).type == ModContent.TileType<VerdantSoilGrass>())
            //    {
            //        WorldGen.PlaceTile(t.X, t.Y - 1, ModContent.TileType<VerdantDecor1x1>(), true, true);
            //        if (Main.netMode == NetmodeID.Server)
            //            NetMessage.SendTileSquare(-1, t.X, t.Y - 1, 3, TileChangeType.None);
            //    }
            //    if (!Framing.GetTileSafely(t.X + 1, t.Y - 1).active() && Framing.GetTileSafely(t.X + 1, t.Y - 1).type == ModContent.TileType<VerdantSoilGrass>())
            //    {
            //        WorldGen.PlaceTile(t.X + 1, t.Y - 1, ModContent.TileType<VerdantDecor1x1>(), true, true);
            //        if (Main.netMode == NetmodeID.Server)
            //            NetMessage.SendTileSquare(-1, t.X + 1, t.Y - 1, 3, TileChangeType.None);
            //    }
            //}
        }
	}
}