using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Verdant.Tiles.Verdant.Decor;

internal class HardmodeApotheosis : ModTile
{
    private int _timer = 0;
    private Asset<Texture2D> glowTex;

    public override void SetStaticDefaults()
    {
        QuickTile.SetMulti(this, 16, 12, DustID.Stone, SoundID.Dig, false, new Color(142, 120, 124), false, false, false, "Apotheosis");
        glowTex = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;
    public override bool CanExplode(int i, int j) => false;
    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public override void NearbyEffects(int i, int j, bool closer)
    {
        _timer++;

        if (Framing.GetTileSafely(i, j).TileFrameX == 126 && Framing.GetTileSafely(i, j).TileFrameY == 36)
        {
            Vector2 p = new Vector2(i, j) * 16;
            float LightMult = (float)((Math.Sin(Main.time * 0.03f) * 0.6) + 0.7) * 2f + 0.5f;

            Lighting.AddLight(p, new Vector3(0.44f, 0.17f, 0.28f) * 2f * LightMult);
            Lighting.AddLight(p, new Vector3(0.2f, 0.06f, 0.12f));
        }
    }

    public override bool RightClick(int i, int j)
    {
        if (_timer > 3000)
        {
            if (ScreenTextManager.CurrentText is not null)
                return false;

            if (NPC.downedBoss1 && !ModContent.GetInstance<VerdantSystem>().apotheosisEyeDown) //EoC text
            {
                DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Eye");
                return true;
            }

            if (NPC.downedBoss2 && !ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown) //BoC/EoW text
            {
                DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Evil");
                return true;
            }

            if (NPC.downedBoss3 && !ModContent.GetInstance<VerdantSystem>().apotheosisSkelDown) //Skeleton boss text
            {
                DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Skeletron");
                return true;
            }

            if (Main.hardMode && !ModContent.GetInstance<VerdantSystem>().apotheosisWallDown) //WoF boss text
            {
                DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".WoF");
                return true;
            }

            if (!ModContent.GetInstance<VerdantSystem>().apotheosisGreeting) //Greeting
                DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Greeting");
            else
                DialogueCacheAutoloader.Play(nameof(ApotheosisDialogueCache) + ".Idle", false);
        }
        return true;
    }

    public override void MouseOver(int i, int j)
    {
        Main.LocalPlayer.cursorItemIconText = "Speak";
        Main.LocalPlayer.cursorItemIconEnabled = false;
        Main.LocalPlayer.cursorItemIconID = -1;
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile t = Main.tile[i, j];

        spriteBatch.Draw(glowTex.Value, TileHelper.TileCustomPosition(i, j), new Rectangle(t.TileFrameX, t.TileFrameY, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
    }
}