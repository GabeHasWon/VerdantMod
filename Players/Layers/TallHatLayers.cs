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

        if (!player.active && !Main.gameMenu)
            return;

        bool isArmorTallHatAndNotHidden = player.armor[0].ModItem is ITallHat && player.armor[10].IsAir;
        bool isVanityTallHat = player.armor[10].ModItem is ITallHat;

        if (player.outOfRange || player.dead || (!isArmorTallHatAndNotHidden && !isVanityTallHat))
            return;

        ITallHat hat;

        if (isArmorTallHatAndNotHidden)
            hat = player.armor[0].ModItem as ITallHat;
        else if (isVanityTallHat)
            hat = player.armor[10].ModItem as ITallHat;
        else
            return;

        var tex = ChosenTexture(hat);
        var hatPos = hat.HatOffset(player, drawInfo);
        var position = drawInfo.GetRealDrawPosition(hatPos + new Vector2(player.width / 2f, 0));
        var col = Main.gameMenu ? Color.White : Lighting.GetColor((position + Main.screenPosition).ToTileCoordinates());
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
    Vector2 HatOffset(Player player, PlayerDrawSet info);
    Texture2D HatTexture();
    Texture2D HatBackTexture();

    /// <summary>
    /// Whether the current hat should or should not use normal hat framing. Return false to use the modified frame.
    /// </summary>
    bool HatModifyFraming(Rectangle baseFrame, out Rectangle frame);
}