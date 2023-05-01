using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Tools.Paintbrush;

internal class PaintbrushDrawLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => true;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.shadow != 0)
            return;

        if (drawInfo.drawPlayer.HeldItem.ModItem is CrudePaintbrush brush)
            brush.GetLayerDrawing(drawInfo);
    }
}
