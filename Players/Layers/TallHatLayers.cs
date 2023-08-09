using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Verdant.Players.Layers;

/// <summary>
/// Draws any given tall hat I may need.
/// </summary>
internal class TallHatLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);
    public virtual Texture2D ChosenTexture(ITallHat hat) => hat.HatTexture();

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player player = drawInfo.drawPlayer;

        if ((!player.active && !Main.gameMenu) || player.outOfRange || player.dead || player.armor[0].ModItem is not ITallHat hat)
            return;

        var tex = ChosenTexture(hat);
        var hatPos = hat.HatPosition(player, drawInfo);
        var position = this.GetRealDrawPosition(hatPos - Main.screenPosition + new Vector2(player.width / 2f, 0));
        var col = Main.gameMenu ? Color.White : Lighting.GetColor(hatPos.ToTileCoordinates());
        var frame = player.bodyFrame;

        if (!hat.HatModifyFraming(frame, out Rectangle newFrame))
            frame = newFrame;

        var effect = player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        var data = new DrawData(tex, position, frame, col, 0f, frame.Size() / 2f, Vector2.One, effect, 0);
        drawInfo.DrawDataCache.Add(data);
    }
}

internal class TallHatLayerBack : TallHatLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeadBack);
    public override Texture2D ChosenTexture(ITallHat hat) => hat.HatBackTexture();
}

internal interface ITallHat
{
    Vector2 HatPosition(Player player, PlayerDrawSet info);
    Texture2D HatTexture();
    Texture2D HatBackTexture();

    /// <summary>
    /// Whether the current hat should or should not use normal hat framing. Return false to use the modified frame.
    /// </summary>
    bool HatModifyFraming(Rectangle baseFrame, out Rectangle frame);
}