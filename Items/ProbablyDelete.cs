using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Effects;
using Verdant.Projectiles.Magic;
using Verdant.Systems.RealtimeGeneration;
using Verdant.Systems.RealtimeGeneration.CaptureRendering;
using Verdant.Systems.RealtimeGeneration.Old;
using Verdant.Tiles.Verdant.Basic;
using Verdant.Tiles.Verdant.Basic.Blocks;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Items;

public class ProbablyDelete : ModItem
{
    public override bool IsLoadingEnabled(Mod mod) => true;

    public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Zegg");
		Tooltip.SetDefault("@ me if you see this");
	}

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
		Item.autoReuse = true;
        Item.placeStyle = 0;
        //Item.shoot = ModContent.ProjectileType<HealPlants>();
        Item.createTile = TileID.Bamboo;
    }

    public override bool? UseItem(Player player)
    {
		//ScreenTextManager.CurrentText = ApotheosisDialogueCache.IntroDialogue(false);
		//var pos = Main.MouseWorld.ToTileCoordinates();

		//Tile tile = Main.tile[pos];
		//tile.TileFrameX = 0;
		//tile.TileFrameY = 0;
		//Main.NewText(tile.TileFrameX + " " + tile.TileFrameY);

		//return true;
		//if (!RealtimeGen.HasStructure("Testing"))
		//    Spawn(pos);

		return true;
    }

    private void Spawn(Point pos)
    {
		Queue<RealtimeStep> steps = new();

		const int Size = 13;

		for (int i = pos.X - Size; i <= pos.X + Size; ++i)
		{
			for (int j = pos.Y - Size; j <= pos.Y + Size; ++j)
			{
				RealtimeStep step = new(new(i, j), TileAction.PlaceTile(TileID.SilverBrick, false, true, true));
				steps.Enqueue(step);
			}
		}
		
		ModContent.GetInstance<RealtimeGen>().CurrentActions.Add(new RealtimeAction(steps, 5, true, "Testing", new ApotheosisCapture("Testing", 400)));
	}
}