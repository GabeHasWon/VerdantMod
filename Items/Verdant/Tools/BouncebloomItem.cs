using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Tools;

class BouncebloomItem : ApotheoticItem
{
    public override bool IsLoadingEnabled(Mod mod)
    {
        VerdantPlayer.ItemDrawLayerEvent += PlayerDraw;
        return true;
    }

    public override void SetDefaults() => QuickItem.SetBlock(this, 38, 26, ModContent.TileType<Tiles.Verdant.Basic.Plants.Bouncebloom>(), true);
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Bouncebloom", "Slows fall\nHold DOWN to fall faster\n" +
        "Light enemies above you will bounce on the flower");

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

    private static bool CheckNPCConditions(NPC n, Player p) => n.Hitbox.Intersects(new Rectangle((int)p.Center.X - 24, (int)p.Center.Y - 30, 48, 24)) && !n.boss && 
        n.width + n.height < 300 && n.knockBackResist > 0.05f && n.velocity.Y > 0 && n.life > 5;

    public override void HoldItemFrame(Player player) => player.bodyFrame.Y = 56;

    public void PlayerDraw(PlayerDrawSet info)
    {
        if (info.drawPlayer.HeldItem.type == Item.type && !info.drawPlayer.dead)
        {
            Texture2D t = Mod.Assets.Request<Texture2D>("Items/Verdant/Tools/BouncebloomEquip").Value;
            Vector2 pos = PlayerHelper.PlayerDrawPositionOffset(info.drawPlayer, new Vector2(0, -52));
            Color col = Lighting.GetColor((int)(info.drawPlayer.position.X / 16f), (int)(info.drawPlayer.position.Y / 16f));
            SpriteEffects effect = info.drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            DrawData data = new DrawData(t, pos.Floor(), null, col, 0f, new Vector2(t.Width / 2f, t.Height / 2f), 1f, effect, 0);
            info.DrawDataCache.Add(data);
        }
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(BouncebloomItem))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Bouncebloom.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Bouncebloom.0", 80).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Bouncebloom.1", 50, 1f));
    }
}
