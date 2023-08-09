using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
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

        Main.instance.LoadProjectile(ModContent.ProjectileType<FruitProjectile>());
        var tex = TextureAssets.Projectile[ModContent.ProjectileType<FruitProjectile>()].Value;
        var fruits = player.GetModPlayer<TreeHelmetPlayer>().fruits;
        var hatPos = hat.HatOffset(player, drawInfo) - new Vector2(8 + (3 * player.direction), player.GetBobble() + 11);
        var effect = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        for (int i = 0; i < fruits.Length; ++i)
        {
            int index = player.direction == -1 ? i : fruits.Length - 1 - i;

            if (fruits[index] == FruitType.None)
                continue;

            DrawFruit(drawInfo, tex, hatPos, effect, i, index);
        }
    }

    private static void DrawFruit(PlayerDrawSet drawInfo, Texture2D tex, Vector2 hatPos, SpriteEffects effect, int index, int fruitIndex)
    {
        Player player = drawInfo.drawPlayer;
        var treePlayer = player.GetModPlayer<TreeHelmetPlayer>();
        var basePosition = hatPos + new Vector2(index * 8 + 10, 0);

        if (index == 1)
            basePosition.Y -= 8;
        else if ((player.direction == 1 && index == 2) || (index == 0 && player.direction != 1))
            basePosition.Y -= 4;

        var position = drawInfo.GetRealDrawPosition(basePosition);
        var col = Main.gameMenu ? Color.White : Lighting.GetColor((position + Main.screenPosition).ToTileCoordinates());
        var frame = new Rectangle(12 * (int)(treePlayer.fruits[fruitIndex] - 1), 0, 10, 14);
        var rotation = MathF.Sin(Main.GameUpdateCount * 0.05f + index) * MathHelper.PiOver4 * 0.5f;
        var data = new DrawData(tex, position, frame, col, rotation, new Vector2(frame.Width / 2f, 0), Vector2.One, effect, 0);
        drawInfo.DrawDataCache.Add(data);
    }
}