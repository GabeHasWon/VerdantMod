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
            return mod.Properties.Autoload;
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

        public void FloorVisuals(Player p, int type) //grows plants beneath the player
        {
            if (p.ArmourEquipped(item) && System.Math.Abs(p.velocity.X) > 0.01f)
            {
                Point t = p.TileCoordsBottomCentred();
                Tile ground = Framing.GetTileSafely(t.X, t.Y);

                if (!Framing.GetTileSafely(t.X, t.Y - 1).active() && !ground.topSlope() && !ground.halfBrick())
                {
                    if (ground.type == ModContent.TileType<VerdantGrassLeaves>())
                    {
                        int choice = Main.rand.Next(3);
                        bool[] valids = new bool[] { true, !Framing.GetTileSafely(t.X, t.Y - 2).active(), !Framing.GetTileSafely(t.X, t.Y - 1).active() && !Framing.GetTileSafely(t.X, t.Y - 2).active() };

                        while (!valids[choice])
                            choice = Main.rand.Next(3);

                        if (choice == 0)
                            WorldGen.PlaceTile(t.X, t.Y - 1, ModContent.TileType<VerdantDecor1x1>(), true, true, -1, Main.rand.Next(7));
                        else if (choice == 1)
                            WorldGen.PlaceTile(t.X, t.Y - 2, ModContent.TileType<VerdantDecor1x2>(), true, true, -1, Main.rand.Next(6));
                        else if (choice == 2)
                            WorldGen.PlaceTile(t.X, t.Y - 3, ModContent.TileType<VerdantDecor1x3>(), true, true, -1, Main.rand.Next(6));

                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendTileSquare(-1, t.X, t.Y - 1, 3, TileChangeType.None);
                    }
                    else if (ground.type == TileID.Grass)
                    {
                        int choice = Main.rand.Next(2);
                        bool[] valids = new bool[] { true, !Framing.GetTileSafely(t.X, t.Y - 2).active() };

                        while (!valids[choice])
                            choice = Main.rand.Next(2);

                        if (choice == 0)
                            WorldGen.PlaceTile(t.X, t.Y - 1, TileID.Plants, true, true, -1, Main.rand.Next(45));
                        else if (choice == 1)
                            WorldGen.PlaceTile(t.X, t.Y - 1, TileID.Plants2, true, true, -1, Main.rand.Next(45));

                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendTileSquare(-1, t.X, t.Y - 1, 3, TileChangeType.None);
                    }
                    else if (ground.type == TileID.JungleGrass)
                    {
                        int choice = Main.rand.Next(2);
                        bool[] valids = new bool[] { true, !Framing.GetTileSafely(t.X, t.Y - 2).active() };

                        while (!valids[choice])
                            choice = Main.rand.Next(2);

                        if (choice == 0)
                            WorldGen.PlaceTile(t.X, t.Y - 1, TileID.JunglePlants, true, true, -1, Main.rand.Next(23));
                        else if (choice == 1)
                            WorldGen.PlaceTile(t.X, t.Y - 1, TileID.JunglePlants2, true, true, -1, Main.rand.Next(17));

                        if (Main.netMode == NetmodeID.Server)
                            NetMessage.SendTileSquare(-1, t.X, t.Y - 1, 3, TileChangeType.None);
                    }
                }
            }
        }
	}
}