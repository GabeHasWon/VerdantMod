﻿using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.World;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Verdant.Items;

namespace Verdant.Tiles.Verdant.Decor;

internal class Apotheosis : ModTile
{
    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.newTile.LavaPlacement = Terraria.Enums.LiquidPlacement.Allowed;
        TileObjectData.newTile.WaterDeath = false;
        TileObjectData.newTile.WaterPlacement = Terraria.Enums.LiquidPlacement.Allowed;
        QuickTile.SetMulti(this, 16, 12, DustID.Stone, SoundID.Dig, false, new Color(142, 120, 124), false, false, false, "Apotheosis");
    }

    public override bool CanKillTile(int i, int j, ref bool blockDamaged) => false;
    public override bool CanExplode(int i, int j) => false;
    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

    public override void NearbyEffects(int i, int j, bool closer)
    {
        if (Framing.GetTileSafely(i, j).TileFrameX == 126 && Framing.GetTileSafely(i, j).TileFrameY == 36)
        {
            Vector2 p = new Vector2(i, j) * 16;
            float LightMult = (float)((Math.Sin(Main.time * 0.03f) * 0.6) + 0.7);

            if (ModContent.GetInstance<VerdantSystem>().apotheosisEvilDown)
                LightMult *= 1.3f;

            if (ModContent.GetInstance<VerdantSystem>().apotheosisSkelDown)
                LightMult *= 1.6f;

            Lighting.AddLight(p, new Vector3(0.44f, 0.17f, 0.28f) * 2f * LightMult);
            Lighting.AddLight(p, new Vector3(0.1f, 0.03f, 0.06f));
        }
    }

    public override bool RightClick(int i, int j)
    {
        TrySetLocation(i, j);

        if (ScreenTextManager.CurrentText is not null)
            return false;

        if (CheckApotheoticHeldItem())
            return true;

        if (!CommonDialogue())
            DialogueCacheAutoloader.Play(nameof(ApotheosisDialogueCache) + ".Idle", false);
        return true;
    }

    public static bool CheckApotheoticHeldItem()
    {
        if (!Main.LocalPlayer.HeldItem.IsAir && Main.LocalPlayer.HeldItem.ModItem is ApotheoticItem apoth)
        {
            DialogueCacheAutoloader.Play(nameof(ApotheoticItem) + "." + apoth.Name, false);
            return true;
        }
        return false;
    }

    public static bool CommonDialogue()
    {
        var system = ModContent.GetInstance<VerdantSystem>();

        if (!system.apotheosisGreeting) //Greeting
        {
            DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Greeting");
            return true;
        }

        if (NPC.downedBoss1 && !system.apotheosisEyeDown) //EoC text
        {
            DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Eye");
            return true;
        }

        if (NPC.downedBoss2 && !system.apotheosisEvilDown) //BoC/EoW text
        {
            DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Evil");
            return true;
        }

        if (NPC.downedBoss3 && !system.apotheosisSkelDown) //Skeleton boss text
        {
            DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".Skeletron");
            return true;
        }

        if (Main.hardMode && !system.apotheosisWallDown) //WoF boss text
        {
            DialogueCacheAutoloader.SyncPlay(nameof(ApotheosisDialogueCache) + ".WoF");
            return true;
        }
        return false;
    }

    public override void MouseOver(int i, int j)
    {
        Main.LocalPlayer.cursorItemIconText = "Speak";
        Main.LocalPlayer.cursorItemIconEnabled = false;
        Main.LocalPlayer.cursorItemIconID = -1;
    }

    /// <summary>
    /// Sets the location (<see cref="VerdantGenSystem.apotheosisLocation"/>) to new(i, j) if successful or null if not.
    /// </summary>
    internal static void TrySetLocation(int i, int j)
    {
        var system = ModContent.GetInstance<VerdantGenSystem>();
        Tile checkTile = Main.tile[i, j];

        if (!checkTile.HasTile || (checkTile.TileType != ModContent.TileType<Apotheosis>() && checkTile.TileType != ModContent.TileType<HardmodeApotheosis>()))
        {
            system.apotheosisLocation = null;
            return;
        }

        if (system.apotheosisLocation is null)
        {
            Tile tile = Main.tile[i, j];
            int x = i - (tile.TileFrameX % 18 / 18) + 8;
            int y = j - (tile.TileFrameY % 18 / 18) + 6;

            system.apotheosisLocation = new Point16(x, y);
        }
        else
        {
            Tile orig = Main.tile[system.apotheosisLocation.Value.ToPoint()];

            if (!orig.HasTile || (orig.TileType != ModContent.TileType<Apotheosis>() && orig.TileType != ModContent.TileType<HardmodeApotheosis>()))
            {
                Tile tile = Main.tile[i, j];
                int x = i - (tile.TileFrameX % 18 / 18) + 8;
                int y = j - (tile.TileFrameY % 18 / 18) + 6;

                system.apotheosisLocation = new Point16(x, y);
            }
        }
    }
}