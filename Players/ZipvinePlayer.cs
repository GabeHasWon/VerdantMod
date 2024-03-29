﻿using Ionic.Zip;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Systems.Syncing;

namespace Verdant.Players;

/// <summary>
/// Handles Zipvine controls and movement.
/// </summary>
internal class ZipvinePlayer : ModPlayer
{
    internal ZipvineEntity zipvine = null;
    
    float _progress = 0;

    public override void PreUpdateMovement()
    {
        if (zipvine is not null && zipvine.killMe)
            zipvine = null;

        if (zipvine is null)
            TryGrabAnyVine();
        else
        {
            Player.velocity = Vector2.Zero;
            Player.fallStart = (int)(zipvine.position.Y / 16f);
            Player.pulley = true;
            Player.pulleyDir = 0;
            Player.FindPulley();

            VineMovement();
        }
    }

    private void VineMovement()
    {
        if (Player.controlJump) // Jump off vine
        {
            zipvine = null;
            return;
        }

        float oldProgress = _progress;

        if (Player.controlUp) // Climb vine
            _progress += zipvine.ClimbSpeed;
        else if (Player.controlDown)
            _progress -= zipvine.ClimbSpeed;

        AdjustVineProgress();

        Vector2 nextPosition = zipvine.nextVine is null ? zipvine.position : zipvine.nextVine.position;
        Vector2 realNextPos = Vector2.Lerp(zipvine.position, nextPosition, _progress) + zipvine.HoldOffset;

        bool mockLineCheck = true;

        for (float i = 0; i < 1; i += 0.1f) // Mock a tile collision line check so you can't pass through thin walls
        {
            if (Collision.SolidCollision(Vector2.Lerp(Player.position, realNextPos, i), Player.width, Player.height))
            {
                mockLineCheck = false;
                break;
            }
        }

        if (mockLineCheck)
            Player.position = realNextPos;
        else
        {
            if (Player.controlUp) // Climb vine
                _progress -= zipvine.ClimbSpeed;
            else if (Player.controlDown)
                _progress += zipvine.ClimbSpeed;

            AdjustVineProgress();
        }

        if (_progress != oldProgress)
            UpdatePulleyFrame();
    }

    private void UpdatePulleyFrame()
    {
        if (Player.controlUp || Player.controlDown)
            Player.pulleyFrameCounter += zipvine.ClimbSpeed * 3;

        if (Player.pulleyFrameCounter > 10f)
        {
            Player.pulleyFrame++;
            Player.pulleyFrameCounter = 0f;
        }

        if (Player.pulleyFrame > 1)
            Player.pulleyFrame = 0;
    }

    private void AdjustVineProgress()
    {
        if (_progress < 0) // Adjust progress so we skip to next vine(s) if needed
        {
            if (zipvine.priorVine is null)
                _progress = 0;

            while (_progress < 0)
            {
                if (zipvine.priorVine is null)
                {
                    _progress = 0;
                    break;
                }

                zipvine = zipvine.priorVine;
                _progress += 1f;
            }
        }
        else if (_progress > 1)
        {
            if (zipvine.nextVine is null)
                _progress = 1;

            while (_progress > 1)
            {
                if (zipvine.nextVine is null)
                {
                    _progress = 1;
                    break;
                }

                zipvine = zipvine.nextVine;
                _progress -= 1f;
            }
        }
    }

    private void TryGrabAnyVine()
    {
        bool validDir = (Player.controlUp || Player.controlDown) && !Player.controlJump;
        bool openToGrab = !Player.mount.Active && !Player.pulley && Player.grappling[0] < 0;
        bool collision = !Collision.SolidCollision(Player.position, Player.width, Player.height);

        if (!validDir || !openToGrab || !collision)
            return; // Return if obstructed or grappled/mounted

        Rectangle playerTop = new((int)Player.position.X, (int)Player.position.Y, Player.width, 2);
        var allVines = ForegroundManager.PlayerLayerItems.Where(x => x is ZipvineEntity && x.DistanceSQ(Player.Center) < 80 * 80);

        foreach (var vine in allVines)
        {
            if (playerTop.Intersects((vine as ZipvineEntity).Hitbox))
            {
                Player.pulley = true;
                Player.pulleyDir = 1;
                Player.position = vine.Center;
                Player.fallStart = (int)(vine.position.Y / 16f);

                zipvine = vine as ZipvineEntity;
                return;
            }
        }
    }

    public override void Load()
    {
        On_Player.QuickMount += Player_QuickMount;
        On_Player.Teleport += Player_Teleport;
    }

    public static void Player_QuickMount(On_Player.orig_QuickMount orig, Player self)
    {
        if (self.GetModPlayer<ZipvinePlayer>().zipvine != null)
            self.GetModPlayer<ZipvinePlayer>().zipvine = null;

        orig(self);
    }

    public static void Player_Teleport(On_Player.orig_Teleport orig, Player self, Vector2 newPos, int Style, int extraInfo)
    {
        if (self.GetModPlayer<ZipvinePlayer>().zipvine != null)
            self.GetModPlayer<ZipvinePlayer>().zipvine = null;

        orig(self, newPos, Style, extraInfo);
    }
}
