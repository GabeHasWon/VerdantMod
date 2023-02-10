using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Verdant.Drawing;
using Terraria.DataStructures;
using Verdant.Systems.PestControl;
using Verdant.Items.Verdant.Misc;
using System.Collections.Generic;

namespace Verdant.Tiles.Verdant.Decor;

internal class HardmodeApotheosis : ModTile, IAdditiveTile
{
    private int _effigyTimer = 0;

    private Asset<Texture2D> glowTex;
    private Asset<Texture2D> alphaTex;

    public override void SetStaticDefaults()
    {
        QuickTile.SetMulti(this, 16, 12, DustID.Stone, SoundID.Dig, false, new Color(142, 120, 124), false, false, false, "Apotheosis");
        glowTex = ModContent.Request<Texture2D>(Texture + "_Glow");
        alphaTex = ModContent.Request<Texture2D>(Texture + "_GlowAlpha");
    }

    public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;
    public override bool CanExplode(int i, int j) => false;
    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (Framing.GetTileSafely(i, j).TileFrameX == 126 && Framing.GetTileSafely(i, j).TileFrameY == 36)
        {
            Vector2 p = new Vector2(i, j) * 16;
            float LightMult = (float)((Math.Sin(Main.time * 0.03f) * 0.6) + 0.7) * 2f + 0.5f;

            Lighting.AddLight(p, new Vector3(0.44f, 0.17f, 0.28f) * 2f * LightMult);
            Lighting.AddLight(p, new Vector3(0.2f, 0.06f, 0.12f));
        }

        if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0)
            CheckEffigy(i, j);
    }

    private void CheckEffigy(int i, int j)
    {
        const int CheckDistance = 280 * 280;
        const int MinDistance = 20 * 20;
        const float MaxSpeed = 7;

        if (!Main.hasFocus)
            return;

        int x = i + 8;
        int y = j + 6;

        Vector2 center = new Vector2(x, y).ToWorldCoordinates(-8, -38);
        bool hasEffigy = false;

        for (int k = 0; k < Main.maxItems; ++k)
        {
            Item item = Main.item[k];
            float dist = item.DistanceSQ(center);

            if (item.active && (item.type == ModContent.ItemType<CorruptEffigy>() || item.type == ModContent.ItemType<CrimsonEffigy>()) && dist <= CheckDistance)
            {
                item.velocity += (center - item.Center) * 0.006f;

                if (item.velocity.LengthSquared() > MaxSpeed * MaxSpeed)
                    item.velocity = item.velocity.SafeNormalize(Vector2.Zero) * MaxSpeed;

                if (dist < MinDistance)
                {
                    hasEffigy = true;
                    _effigyTimer++;
                }
                break;
            }
        }

        if (!hasEffigy)
            _effigyTimer--;

        if (_effigyTimer < 0)
            _effigyTimer = 0;

        if (hasEffigy && _effigyTimer > 300)
        {
            for (int k = 0; k < Main.maxItems; ++k)
            {
                Item item = Main.item[k];
                float dist = item.DistanceSQ(center);

                if (item.active && (item.type == ModContent.ItemType<CorruptEffigy>() || item.type == ModContent.ItemType<CrimsonEffigy>()) && dist <= CheckDistance)
                {
                    item.active = false;

                    int[] types = new int[] { DustID.Blood, DustID.CorruptGibs, DustID.Corruption, DustID.CorruptionThorns };

                    if (item.type == ModContent.ItemType<CrimsonEffigy>())
                        types = new int[] { DustID.Blood, DustID.CrimsonPlants, DustID.Crimstone };

                    for (int l = 0; l < 140; ++l)
                    {
                        Vector2 vel = new Vector2(Main.rand.Next(4, 12), 0).RotatedByRandom(MathHelper.TwoPi);
                        Color col = new(Main.rand.NextFloat(0.8f, 1f), Main.rand.NextFloat(0.8f, 1f), Main.rand.NextFloat(0.8f, 1f));
                        Dust.NewDust(item.position, item.width, item.height, Main.rand.Next(types), vel.X, vel.Y, 0, col, Main.rand.NextFloat(0.8f, 1.4f));
                    }

                    Apotheosis.TrySetLocation(i, j);
                    break;
                }
            }

            _effigyTimer = 0;

            DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".PestControlWarning");
        }
    }

    public override bool RightClick(int i, int j)
    {
        Apotheosis.TrySetLocation(i, j);

        if (ScreenTextManager.CurrentText is not null)
            return false;

        if (!ModContent.GetInstance<VerdantSystem>().apotheosisGreeting) //Greeting
        {
            DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Greeting");
            return true;
        }

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

        if (Main.hardMode && !ModContent.GetInstance<VerdantSystem>().apotheosisPestControlNotif)
        {
            DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".PestControlNotif");
            return true;
        }

        DialogueCacheAutoloader.Play(nameof(ApotheosisDialogueCache) + ".Idle", false);
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

    public void DrawAdditive(Point16 position)
    {
        Tile t = Main.tile[position.X, position.Y];

        if (t.TileFrameX != 0 || t.TileFrameY != 0)
            return;

        Main.spriteBatch.Draw(alphaTex.Value, TileHelper.TileCustomPosition(position.X, position.Y, new Vector2(6, -2)), null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
    }
}