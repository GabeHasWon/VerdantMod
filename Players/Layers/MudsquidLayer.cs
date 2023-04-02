using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Verdant.Players.Layers;

public class MudsquidLayer : PlayerDrawLayer
{
    private Asset<Texture2D> _squidTexture = null;

    public override void Unload() => _squidTexture = null;

    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player drawPlayer = drawInfo.drawPlayer;

        if (_squidTexture is null)
            ModContent.Request<Texture2D>("Verdant/Items/Verdant/Equipables/Mudsquid");
    }
}
