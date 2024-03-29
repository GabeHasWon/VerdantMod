﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Verdant.World;

namespace Verdant.Systems.PestControl;

internal class PestInvasionUI
{
	public static void DrawEventUI(SpriteBatch spriteBatch)
	{
		if (Main.LocalPlayer.GetModPlayer<PestPlayer>().InPestControl)
		{
			const float Scale = 0.875f;
			const float Alpha = 0.5f;
			const int InternalOffset = 6;
			const int OffsetX = 20;
			const int OffsetY = 20;

			var system = ModContent.GetInstance<PestSystem>();
			Texture2D EventIcon = ModContent.Request<Texture2D>("Verdant/Systems/PestControl/Textures/EventIcon").Value;
			Color descColor = new Color(77, 39, 135);
			Color waveColor = new Color(255, 241, 51);

			int width = (int)(200f * Scale);
			int height = (int)(46f * Scale);

			Rectangle waveBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - OffsetX - 100f, Main.screenHeight - OffsetY - 23f), new Vector2(width, height));
			Utils.DrawInvBG(spriteBatch, waveBackground, new Color(63, 65, 151, 255) * 0.785f);

			string waveText = GetWaveText(system);
			Utils.DrawBorderString(spriteBatch, waveText, new Vector2(waveBackground.Center.X, waveBackground.Y + 5), Color.White, Scale, 0.5f, -0.1f);
			Rectangle waveProgressBar = Utils.CenteredRectangle(new Vector2(waveBackground.Center.X, waveBackground.Y + waveBackground.Height * 0.75f), TextureAssets.ColorBar.Size());

			var waveSourceRectangle = new Rectangle(0, 0, (int)(TextureAssets.ColorBar.Width() * 0.01f * system.pestControlProgress), TextureAssets.ColorBar.Height());
			var offset = new Vector2((waveProgressBar.Width - (int)(waveProgressBar.Width * Scale)) * 0.5f, (waveProgressBar.Height - (int)(waveProgressBar.Height * Scale)) * 0.5f);
			spriteBatch.Draw(TextureAssets.ColorBar.Value, waveProgressBar.Location.ToVector2() + offset, null, Color.White * Alpha, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureAssets.ColorBar.Value, waveProgressBar.Location.ToVector2() + offset, waveSourceRectangle, waveColor, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);

			Vector2 descSize = new Vector2(154, 40) * Scale;
			Rectangle barrierBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - OffsetX - 100f, Main.screenHeight - OffsetY - 19f), new Vector2(width, height));
			Rectangle descBackground = Utils.CenteredRectangle(new Vector2(barrierBackground.Center.X, barrierBackground.Y - InternalOffset - descSize.Y * 0.5f), descSize * 0.9f);
			Utils.DrawInvBG(spriteBatch, descBackground, descColor * Alpha);

			int descOffset = (descBackground.Height - (int)(32f * Scale)) / 2;
			var icon = new Rectangle(descBackground.X + descOffset + 7, descBackground.Y + descOffset, (int)(32 * Scale), (int)(32 * Scale));
			spriteBatch.Draw(EventIcon, icon, Color.White);
			Utils.DrawBorderString(spriteBatch, "Pest Control", new Vector2(barrierBackground.Center.X, barrierBackground.Y - InternalOffset - descSize.Y * 0.5f), Color.White, 0.8f, 0.3f, 0.4f);
		}
	}

	private static string GetWaveText(PestSystem system)
	{
		string str = (system.pestControlProgress * 100).ToString();

		if (system.pestControlProgress < 0.1f)
			return (str.Length < 4 ? str : str[..4]) + "%";

		return (str.Length < 5 ? str : str[..5]) + "%";
	}
}
