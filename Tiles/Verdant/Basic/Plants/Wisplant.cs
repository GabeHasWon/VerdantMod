using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants
{
	public class Wisplant : ModTile
	{
		private enum PlantStage : byte
		{
			Planted,
			Growing,
			Grown
		}

		private const int FrameWidth = 18;

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;

			TileID.Sets.ReplaceTileBreakUp[Type] = true;
			TileID.Sets.IgnoredInHouseScore[Type] = true;
			TileID.Sets.IgnoredByGrowingSaplings[Type] = true;
			TileID.Sets.SwaysInWindBasic[Type] = true;
			TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Wisplant");
			AddMapEntry(new Color(255, 174, 183), name);

			TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.AnchorValidTiles = new int[] { ModContent.TileType<LushSoil>() };
			TileObjectData.newTile.ExpandValidAnchors(VerdantGrassLeaves.VerdantGrassList());
			TileObjectData.newTile.AnchorAlternateTiles = new int[] { TileID.ClayPot, TileID.PlanterBox };
			TileObjectData.addTile(Type);

			HitSound = SoundID.Grass;
			DustType = DustID.PinkStarfish;
		}

		public override bool CanPlace(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j); // Safe way of getting a tile instance

			if (tile.HasTile)
			{
				int tileType = tile.TileType;
				if (tileType == Type)
					return GetStage(i, j) == PlantStage.Grown;
				else
				{
					if (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType] || tileType == TileID.WaterDrip || tileType == TileID.LavaDrip || tileType == TileID.HoneyDrip || tileType == TileID.SandDrip)
					{
						bool foliageGrass = tileType == TileID.Plants || tileType == TileID.Plants2;
						bool moddedFoliage = tileType >= TileID.Count && (Main.tileCut[tileType] || TileID.Sets.BreakableWhenPlacing[tileType]);
						bool harvestableVanillaHerb = Main.tileAlch[tileType] && WorldGen.IsHarvestableHerbWithSeed(tileType, tile.TileFrameX / 18);

						if (foliageGrass || moddedFoliage || harvestableVanillaHerb)
						{
							WorldGen.KillTile(i, j);

							if (!tile.HasTile && Main.netMode == NetmodeID.MultiplayerClient)
								NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j);
							return true;
						}
					}
					return false;
				}
			}
			return true;
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects) => spriteEffects = i % 2 == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = -2;
		public override bool IsTileSpelunkable(int i, int j) => GetStage(i, j) == PlantStage.Grown;
		private static PlantStage GetStage(int i, int j) => (PlantStage)(Framing.GetTileSafely(i, j).TileFrameX / FrameWidth);

		public override bool Drop(int i, int j)
		{
			PlantStage stage = GetStage(i, j);

			if (stage == PlantStage.Planted)
				return false;

			Vector2 worldPosition = new Vector2(i, j).ToWorldCoordinates();
			Player nearestPlayer = Main.player[Player.FindClosest(worldPosition, 16, 16)];

			int herbItemStack = 0;
			int seedItemStack = 0;

			if (stage == PlantStage.Grown)
			{
				herbItemStack = 1;
				seedItemStack = Main.rand.Next(2, 4);
			}
			else if (stage == PlantStage.Growing)
				seedItemStack = Main.rand.Next(1, 3);

			if (nearestPlayer.active && nearestPlayer.HeldItem.type == ItemID.StaffofRegrowth)
			{
				herbItemStack += Main.rand.Next(2);
				seedItemStack += Main.rand.Next(1, 4);
			}

			var source = new EntitySource_TileBreak(i, j);

			if (herbItemStack > 0)
				Item.NewItem(source, worldPosition, ModContent.ItemType<WisplantItem>(), herbItemStack);

			if (seedItemStack > 0)
				Item.NewItem(source, worldPosition, ModContent.ItemType<WisplantSeeds>(), seedItemStack);
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			PlantStage stage = GetStage(i, j);

			if (stage != PlantStage.Grown)
			{
				tile.TileFrameX += FrameWidth;

				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendTileSquare(-1, i, j, 1);
			}
		}
	}
}