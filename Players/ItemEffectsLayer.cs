using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Verdant.Players
{
    internal class ItemEffectsLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.ArmOverItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;
            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.active && !drawPlayer.outOfRange)
                drawPlayer.GetModPlayer<VerdantPlayer>().InvokeDrawLayer(drawInfo);
        }
    }
}
