using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Verdant.Players.Layers;

public class MudsquidLayer : PlayerDrawLayer
{
    private static Asset<Texture2D> _squidTexture = null;

    private float _rotation = 0;

    public override void Unload() => _squidTexture = null;

    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player drawPlayer = drawInfo.drawPlayer;
        MudsquidPlayer squidPlayer = drawPlayer.GetModPlayer<MudsquidPlayer>();

        if (squidPlayer.squidAlpha < 1)
        {
            _squidTexture ??= ModContent.Request<Texture2D>("Verdant/Items/Verdant/Equipables/Mudsquid");

            if (drawPlayer.velocity.LengthSquared() > 0.6f)
                _rotation = drawPlayer.velocity.ToRotation() + MathHelper.PiOver2;

            var tex = _squidTexture.Value;
            var col = Lighting.GetColor(drawPlayer.Center.ToTileCoordinates()) * (1 - squidPlayer.squidAlpha);
            var scale = new Vector2(1 - drawPlayer.velocity.Length() * 0.01f, 1 + drawPlayer.velocity.Length() * 0.01f);
            var data = new DrawData(tex, drawPlayer.Center - Main.screenPosition, null, col, _rotation, tex.Size() / 2f, scale, SpriteEffects.None, 0);
            drawInfo.DrawDataCache.Add(data);
        }
    }
}
