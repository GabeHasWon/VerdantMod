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

    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);
    public override bool IsHeadLayer => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player drawPlayer = drawInfo.drawPlayer;
        MudsquidPlayer squidPlayer = drawPlayer.GetModPlayer<MudsquidPlayer>();
        float alpha = squidPlayer.squidAlpha;
        Vector2 offset = Vector2.Zero;

        if (drawInfo.headOnlyRender)
        {
            alpha = squidPlayer.IsSquid && alpha < 0.2f ? 0 : 1;
            offset.X += 20;
        }

        if (alpha < 1)
        {
            _squidTexture ??= ModContent.Request<Texture2D>("Verdant/Items/Verdant/Equipables/Mudsquid");

            if (drawPlayer.velocity.LengthSquared() > 0.6f)
                _rotation = drawPlayer.velocity.ToRotation() + MathHelper.PiOver2;

            var tex = _squidTexture.Value;
            var col = drawInfo.headOnlyRender ? Color.White : Lighting.GetColor(drawPlayer.Center.ToTileCoordinates()) * (1 - alpha);
            var scale = new Vector2(1 - drawPlayer.velocity.Length() * 0.01f, 1 + drawPlayer.velocity.Length() * 0.01f);
            Vector2 pos = drawInfo.GetRealDrawPosition(offset);
            var data = new DrawData(tex, pos, null, col, _rotation, tex.Size() / 2f, scale, SpriteEffects.None, 0);
            drawInfo.DrawDataCache.Add(data);
        }
    }
}
