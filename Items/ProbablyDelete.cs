using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Walls;
using Verdant.World;
using static SpiritReforged.Common.WorldGeneration.GenTypes;

namespace Verdant.Items;

public class ProbablyDelete : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) =>
#if DEBUG
        true;
#else
        false;
#endif

    public override void SetStaticDefaults() => ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true;

    public override void SetDefaults()
	{
		Item.DamageType = DamageClass.Melee;
		Item.width = 40;
		Item.height = 40;
		Item.useTime = 2;
		Item.useAnimation = 2;
		Item.maxStack = 50;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.knockBack = 6;
		Item.value = 10000;
		Item.rare = ItemRarityID.Green;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = false;
        Item.placeStyle = 0;
        Item.createWall = ModContent.WallType<BluescreenWall>();
        //Item.createTile = ModContent.TileType<LilyPad>();
        //Item.shoot = ModContent.ProjectileType<HealPlants>();
    }

    public override bool? UseItem(Player player)
    {
        Point pos = Main.MouseWorld.ToTileCoordinates();
        Point end = Main.MouseWorld.ToTileCoordinates() + new Point(40, Main.rand.Next(-10, 10));
        Vector2[] control = VerdantGenSystem.BuildVineControlPoints(pos, end, (int)((pos.Y + end.Y) / 2f + Main.rand.Next(15, 30)), 0);

        GenHelper.ActOnBezier(0.8f, (x, y) =>
        {
            float factor = Math.Abs(Utils.GetLerpValue(pos.X, end.X, x, true) - 0.5f) + 0.25f;
            float off = Math.Max(1, 3 * factor);
            
            for (int k = 0; k < off; ++k)
            {
                Tile tile = Main.tile[x, y + k + (int)(3 - off)];
                tile.WallType = (ushort)ModContent.WallType<VerdantVineWall_Unsafe>();
            }
        }, control);

        //Point16 pos = Main.MouseWorld.ToTileCoordinates16();
        //WorldGen.PlaceObject(pos.X, pos.Y, ModContent.TileType<Graves>(), true, 3);
        //DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Eye");
        return true;

        Tile tile = Main.tile[Main.MouseWorld.ToTileCoordinates()];
        tile.HasTile = false;
        return true;
    }
}