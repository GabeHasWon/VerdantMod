using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Tools
{
    class BouncebloomItem : ModItem
    {
        public override bool Autoload(ref string name)
        {
            VerdantPlayer.ItemDrawLayerEvent += PlayerDraw;
            return mod.Properties.Autoload;
        }

        public override void SetDefaults() => QuickItem.SetBlock(this, 38, 26, ModContent.TileType<Tiles.Verdant.Basic.Plants.Bouncebloom>(), true);
        public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Bouncebloom", "Slow fall + protection from above");

        public override void HoldItem(Player player)
        {
            player.noFallDmg = true;
            player.fallStart = (int)player.position.Y / 16;

            for (int i = 0; i < Main.maxNPCs; ++i)
            {
                NPC npc = Main.npc[i];
                if (npc.active && CheckNPCConditions(npc, player))
                    npc.velocity.Y = -16 * npc.knockBackResist;
            }

            for (int i = 0; i < Main.maxPlayers; ++i)
            {
                Player p = Main.player[i];
                if (p != player && p.active && !p.dead && p.Hitbox.Intersects(new Rectangle((int)player.Center.X - 24, (int)player.Center.Y - 30, 48, 24)))
                    p.velocity.Y = -16;
            }
        }

        private bool CheckNPCConditions(NPC n, Player p) => n.Hitbox.Intersects(new Rectangle((int)p.Center.X - 24, (int)p.Center.Y - 30, 48, 24)) && !n.boss && n.width + n.height < 300 && n.knockBackResist > 0.05f;

        public override bool HoldItemFrame(Player player)
        {
            player.bodyFrame.Y = 56;
            return true;
        }

        public void PlayerDraw(PlayerDrawInfo info)
        {
            if (info.drawPlayer.HeldItem.type == item.type)
            {
                Texture2D t = ModContent.ModContent.GetTexture("Verdant/Items/Verdant/Tools/BouncebloomEquip");
                Vector2 pos = PlayerHelper.PlayerDrawPositionOffset(info.drawPlayer, new Vector2(0, -52));
                Color col = Lighting.GetColor((int)(info.drawPlayer.position.X / 16f), (int)(info.drawPlayer.position.Y / 16f));
                SpriteEffects effect = info.drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                DrawData data = new DrawData(t, pos.Floor(), null, col, 0f, new Vector2(t.Width / 2f, t.Height / 2f), 1f, effect, 0);
                Main.playerDrawData.Add(data);
            }
        }
    }
}
