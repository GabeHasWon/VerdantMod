using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Armour.ApotheoticArmor;

namespace Verdant.Players.Layers;

/// <summary>
/// Draws the fruit of the Apotheotic Treeband.
/// </summary>
internal class ApotheoticTreeHatLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(ModContent.GetInstance<TallHatLayer>());

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player player = drawInfo.drawPlayer;

        if ((!player.active && !Main.gameMenu) || player.outOfRange || player.dead || player.armor[0].ModItem is not ApotheoticTreeHelmet hat)
            return;

        var tex = ApotheoticTreeHelmet.fruitTex.Value;
        var fruits = player.GetModPlayer<TreeHelmetPlayer>().fruits;

        int playerFrame = player.bodyFrame.Y / player.bodyFrame.Height;
        bool highFrame = (playerFrame >= 7 && playerFrame <= 9) || (playerFrame >= 14 && playerFrame <= 16);
        float offset = highFrame ? 0 : -2;

        var hatPos = hat.HatPosition(player, drawInfo) - new Vector2(8 + (3 * player.direction), offset + 11);
        var col = Main.gameMenu ? Color.White : Lighting.GetColor(hatPos.ToTileCoordinates());
        var effect = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        for (int i = 0; i < fruits.Length; ++i)
        {
            int index = player.direction == -1 ? i : fruits.Length - 1 - i;

            if (fruits[index] == FruitType.None)
                continue;

            DrawFruit(drawInfo, tex, hatPos, col, effect, i, index);
        }
    }

    private void DrawFruit(PlayerDrawSet drawInfo, Texture2D tex, Vector2 hatPos, Color col, SpriteEffects effect, int index, int fruitIndex)
    {
        Player player = drawInfo.drawPlayer;
        var treePlayer = player.GetModPlayer<TreeHelmetPlayer>();
        var basePosition = hatPos - Main.screenPosition + new Vector2(index * 8 + 10, 0);

        if (index == 1)
            basePosition.Y -= 8;
        else if ((player.direction == 1 && index == 2) || (index == 0 && player.direction != 1))
            basePosition.Y -= 4;

        var position = this.GetRealDrawPosition(basePosition);

        treePlayer.fruitLocations[fruitIndex] = position + Main.screenPosition;

        var frame = new Rectangle(12 * (int)(treePlayer.fruits[fruitIndex] - 1), 0, 10, 14);
        var rotation = MathF.Sin(Main.GameUpdateCount * 0.05f + index) * MathHelper.PiOver4 * 0.5f;
        var data = new DrawData(tex, position, frame, col, rotation, new Vector2(frame.Width / 2f, 0), Vector2.One, effect, 0);
        drawInfo.DrawDataCache.Add(data);
    }
}