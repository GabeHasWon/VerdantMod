﻿using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Verdant.Tiles.Verdant.Decor;

internal class SnailStatue : ModTile
{
    public override void Load() => On.Terraria.Main.DrawMouseOver += Main_DrawMouseOver;

    private void Main_DrawMouseOver(On.Terraria.Main.orig_DrawMouseOver orig, Main self)
    {
        orig(self);

		Point target = new(Player.tileTargetX, Player.tileTargetY);

		if (Main.tile[target].TileType == Type)
		{
			Tile tile = Main.tile[target];

			if (tile.TileFrameY != 90 || (tile.TileFrameX != 18 && tile.TileFrameX != 36))
				return;

			string[] array = Utils.WordwrapString(SnailText, FontAssets.MouseText.Value, 460, 10, out int lineAmount);
			lineAmount++;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);

			PlayerInput.SetZoom_UI();

			//PlayerInput.SetZoom_UI();
			//PlayerInput.SetZoom_Test();

			float textWidth = 0f;

			for (int l = 0; l < lineAmount; l++)
			{
				float x = FontAssets.MouseText.Value.MeasureString(array[l]).X;

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

			Color color = new(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);

			if (opaque)
			{
				color = Color.Lerp(color, Color.White, 1f);
				Utils.DrawInvBG(Main.spriteBatch, new Rectangle((int)position.X - 10, (int)position.Y - 5, (int)textWidth + 10 * 2, 30 * lineAmount + 5 + 5 / 2), new Color(23, 25, 81, 255) * 0.925f * 0.85f);
			}

			for (int m = 0; m < lineAmount; m++)
				Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, array[m], position.X, position.Y + (m * 30), color, Color.Black, Vector2.Zero);
			
			Main.mouseText = true;
		}
	}

    public override void SetStaticDefaults() => QuickTile.SetMulti(this, 4, 6, DustID.Stone, SoundID.Dig, false, new Color(142, 120, 124), false, false, false, "Snail Statue");
    public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;
    public override bool CanExplode(int i, int j) => false;
    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

	public string SnailText()
    {
		return "Statue of a Snail";
    }
}