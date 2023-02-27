using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Puff;

namespace Verdant.Items.Verdant.Armour;

[AutoloadEquip(EquipType.Legs)]
public class VerdantLeggings : ModItem
	{
    public override bool IsLoadingEnabled(Mod mod)
    {
        VerdantPlayer.FloorVisualEvent += FloorVisuals;
        return true;
    }

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Growth Leggings");
        Tooltip.SetDefault("Increased regen\n+1 max minion\nGrows plants under you as you walk");
    }

    public override void SetDefaults()
		{
			Item.width = 18;
        Item.height = 12;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
        Item.lifeRegen = 2;
    }

		public override void UpdateEquip(Player player) => player.maxMinions++;

    public override void AddRecipes()
    {
        Recipe m = CreateRecipe();
        m.AddIngredient(ModContent.ItemType<VerdantStrongVineMaterial>(), 4);
        m.AddIngredient(ModContent.ItemType<RedPetal>(), 4);
        m.AddIngredient(ModContent.ItemType<PinkPetal>(), 4);
        m.AddIngredient(ModContent.ItemType<LushLeaf>(), 16);
        m.AddIngredient(ModContent.ItemType<YellowBulb>(), 2);
        m.AddTile(TileID.Anvils);
        m.Register();
    }

    public void FloorVisuals(Player p, int type) //grows plants beneath the player
    {
        if (p.ArmourEquipped(Item) && System.Math.Abs(p.velocity.X) > 0.01f)
        {
            Point t = p.TileCoordsBottomCentred();
            Tile ground = Framing.GetTileSafely(t.X, t.Y);

            if (!Framing.GetTileSafely(t.X, t.Y - 1).HasTile && !ground.TopSlope && !ground.IsHalfBlock)
            {
                if (VerdantGrassLeaves.VerdantGrassList().Contains(ground.TileType))
                {
                    int choice = Main.rand.Next(3);
                    bool[] valids = new bool[] { true, !Framing.GetTileSafely(t.X, t.Y - 2).HasTile, !Framing.GetTileSafely(t.X, t.Y - 1).HasTile && !Framing.GetTileSafely(t.X, t.Y - 2).HasTile };

                    while (!valids[choice])
                        choice = Main.rand.Next(3);

                    if (VerdantGrassLeaves.CheckPuff(t.X, t.Y)) //Force puff decor
                        choice = -1;

                    if (choice == -1)
                    {
                        TileHelper.SyncedPlace(t.X, t.Y - 1, ModContent.TileType<PuffDecor1x1>(), style: Main.rand.Next(7));
                        return;
                    }

                    if (choice == 0)
                        TileHelper.SyncedPlace(t.X, t.Y - 1, Main.hardMode ? ModContent.TileType<HardmodeDecor1x1>() : ModContent.TileType<VerdantDecor1x1>(), true, true, -1, Main.rand.Next(7));
                    else if (choice == 1)
                        TileHelper.SyncedPlace(t.X, t.Y - 2, ModContent.TileType<VerdantDecor1x2>(), true, true, -1, Main.rand.Next(6));
                    else if (choice == 2)
                        TileHelper.SyncedPlace(t.X, t.Y - 3, ModContent.TileType<VerdantDecor1x3>(), true, true, -1, Main.rand.Next(6));
                }
                else if (ground.TileType == TileID.Grass)
                {
                    int choice = Main.rand.Next(2);
                    bool[] valids = new bool[] { true, !Framing.GetTileSafely(t.X, t.Y - 2).HasTile };

                    while (!valids[choice])
                        choice = Main.rand.Next(2);

                    if (choice == 0)
                        WorldGen.PlaceTile(t.X, t.Y - 1, TileID.Plants, true, true, -1, Main.rand.Next(45));
                    else if (choice == 1)
                        WorldGen.PlaceTile(t.X, t.Y - 1, TileID.Plants2, true, true, -1, Main.rand.Next(45));

                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendTileSquare(-1, t.X, t.Y - 1, 3, TileChangeType.None);
                }
                else if (ground.TileType == TileID.JungleGrass)
                {
                    int choice = Main.rand.Next(2);
                    bool[] valids = new bool[] { true, !Framing.GetTileSafely(t.X, t.Y - 2).HasTile };

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