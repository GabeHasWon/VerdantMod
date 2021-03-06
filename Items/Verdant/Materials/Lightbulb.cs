using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Materials
{
    class Lightbulb : ModItem
    {
        public override bool Autoload(ref string name)
        {
            VerdantPlayer.ItemDrawLayerEvent += PlayerDraw;
            return base.Autoload(ref name);
        }

        public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 24, ItemRarityID.White);
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lightbulb", "'Keeps Charlie away'");
        public override void HoldItem(Player player) => Lighting.AddLight(player.MountedCenter - new Vector2(0, 28), new Vector3(0.1f, 0.03f, 0.06f) * 12);
        public override void PostUpdate() => Lighting.AddLight(item.position, new Vector3(0.1f, 0.03f, 0.06f) * 9);
        public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick) => wetTorch = true;

        public override bool HoldItemFrame(Player player)
        {
            player.bodyFrame.Y = 56;
            return true;
        }

        public void PlayerDraw(PlayerDrawInfo info)
        {
            if (info.drawPlayer.HeldItem.type == item.type && info.drawPlayer.itemAnimation <= 0)
            {
                Texture2D t = ModContent.GetTexture("Verdant/Items/Verdant/Materials/Lightbulb");
                Vector2 pos = PlayerHelper.PlayerDrawPositionOffset(info.drawPlayer, new Vector2(0, -52));
                Color col = Lighting.GetColor((int)(info.drawPlayer.position.X / 16f), (int)(info.drawPlayer.position.Y / 16f));
                DrawData data = new DrawData(t, pos.Floor(), null, col, 0f, new Vector2(t.Width / 2f, t.Height / 2f), 1f, info.drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                Main.playerDrawData.Add(data);
            }
        }
    }
}
