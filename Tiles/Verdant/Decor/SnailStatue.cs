﻿using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Verdant.NPCs.Passive;

namespace Verdant.Tiles.Verdant.Decor;

internal class SnailStatue : ModTile
{
    public override void Load() => On_Main.DrawMouseOver += Main_DrawMouseOver;

    private void Main_DrawMouseOver(On_Main.orig_DrawMouseOver orig, Main self)
    {
        orig(self);

		Point target = new(Player.tileTargetX, Player.tileTargetY);

		if (Main.tile[target].TileType == Type)
		{
			Tile tile = Main.tile[target];

			if (tile.TileFrameY != 90 || (tile.TileFrameX != 18 && tile.TileFrameX != 36))
				return;

			string[] lines = Utils.WordwrapString(SnailText(), FontAssets.MouseText.Value, 460, 10, out int lineAmount);
			lineAmount++;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_UI();

			float textWidth = 0f;

			for (int l = 0; l < lineAmount; l++)
			{
				float x = FontAssets.MouseText.Value.MeasureString(lines[l]).X;

				if (textWidth < x)
					textWidth = x;
			}

			if (textWidth > 460f)
				textWidth = 460f;

			bool opaque = Main.SettingsEnabled_OpaqueBoxBehindTooltips;
			Vector2 position = Main.MouseScreen + new Vector2(16f);

			if (opaque)
				position += new Vector2(8f, 2f);

			if (position.Y > (Main.screenHeight - 30 * lineAmount))
				position.Y = Main.screenHeight - 30 * lineAmount;

			if (position.X > Main.screenWidth - textWidth)
				position.X = Main.screenWidth - textWidth;

			position.X -= textWidth / 2;

			Color color = new(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);

			if (opaque)
			{
				color = Color.Lerp(color, Color.White, 1f);
				Utils.DrawInvBG(Main.spriteBatch, new Rectangle((int)position.X - 10, (int)position.Y - 5, (int)textWidth + 10 * 2, 30 * lineAmount + 5 + 5 / 2), new Color(23, 25, 81, 255) * 0.925f * 0.85f);
			}

			for (int m = 0; m < lineAmount; m++)
				Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, lines[m], position.X, position.Y + (m * 30), color, Color.Black, Vector2.Zero);
			
			Main.mouseText = true;
		}
	}

	public override void SetStaticDefaults()
	{
		Main.tileNoAttach[Type] = true;
		Main.tileLavaDeath[Type] = false;
		Main.tileFrameImportant[Type] = true;
		Main.tileSolidTop[Type] = false;
		Main.tileSolid[Type] = false;

		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.Width = 4;
        TileObjectData.newTile.Height = 6;
        TileObjectData.newTile.CoordinateHeights = new int[6] { 16, 16, 16, 16, 16, 16 };
		TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -2);
		TileObjectData.newTile.Direction = Terraria.Enums.TileObjectDirection.None;
		TileObjectData.newTile.LinkedAlternates = false;
		TileObjectData.newTile.Origin = new(2, 3);
        TileObjectData.addTile(Type);

		LocalizedText n = CreateMapEntryName();
		// n.SetDefault("Snail Statue");
		AddMapEntry(Color.Gray, n);
	}

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

	public static string SnailText()
    {
		string key = "Prehardmode";

		if (NPC.downedGolemBoss || NPC.downedTowers || NPC.downedAncientCultist)
			key = "Endgame";
		else if (Main.hardMode)
			key = "Hardmode";
		else if (NPC.downedBoss2 || NPC.downedBoss3)
			key = "LatePrehardmode";

		return Language.GetTextValue("Mods.Verdant.SnailDialogue." + key + "." + (int)(Main.ActivePlayerFileData.GetPlayTime().TotalMinutes * 0.25f) % 3);
    }

    public override void HitWire(int i, int j)
    {
		Tile tile = Main.tile[i, j];
		(int frameX, int frameY) = (tile.TileFrameX, tile.TileFrameY);

		int[] types = new int[] { ModContent.NPCType<VerdantRedGrassSnail>(), ModContent.NPCType<VerdantBulbSnail>() };
		int npc = NPC.NewNPC(new EntitySource_Wiring(i, j), (i - (frameX / 18 % 4) + 2) * 16, (j - (frameY / 18 % 6) + 3) * 16, Main.rand.Next(types));

		Main.npc[npc].GivenName = SnailText();

		for (int x = i; x < i + 4; ++x)
			for (int y = j; y < j + 6; ++y)
				Wiring.SkipWire(x, y);

		if (Main.netMode != NetmodeID.SinglePlayer)
			NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc);
	}
}