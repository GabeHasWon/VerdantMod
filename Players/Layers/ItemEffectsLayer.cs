using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Verdant.Players.Layers
{
    internal class ItemEffectsLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.active && !drawPlayer.outOfRange)
                drawPlayer.GetModPlayer<VerdantPlayer>().InvokeDrawLayer(drawInfo);
        }
    }
}
