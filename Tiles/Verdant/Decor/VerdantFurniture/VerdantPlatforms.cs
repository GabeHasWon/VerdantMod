using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
	public class VerdantPlatforms : ModTile
	{
		public override void SetStaticDefaults() 
		{
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileID.Sets.Platforms[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;

            TileObjectData.newTile.CoordinateHeights = new[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.StyleMultiplier = 27;
            TileObjectData.newTile.StyleWrapLimit = 27;
            TileObjectData.newTile.UsesCustomCanPlace = false;
            TileObjectData.newTile.LavaDeath = true;
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			AddMapEntry(new Color(33, 142, 22));

			HitSound = SoundID.Grass;
			DustType = DustID.Grass;
			ItemDrop = ModContent.ItemType<Items.Verdant.Blocks.VerdantFurniture.VerdantPlatformItem>();
			AdjTiles = new int[] { TileID.Platforms };
		}

		public override void PostSetDefaults() => Main.tileNoSunLight[Type] = false;
		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
	}

	public class VerdantPlatformsDropLeaves : VerdantPlatforms
	{
        public override string Texture => base.Texture.Substring(0, base.Texture.Length - "DropLeaves".Length);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemDrop = ModContent.ItemType<Items.Verdant.Materials.LushLeaf>();
		}
	}
}