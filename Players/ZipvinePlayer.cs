using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;

namespace Verdant.Players;

internal class ZipvinePlayer : ModPlayer
{
    ZipvineEntity zipvine = null;
    float progress = 0;

    public override void PreUpdateMovement()
    {
        if (zipvine is null)
            TryGrabAnyVine();
        else
        {
            Player.velocity = Vector2.Zero;
            Player.fallStart = (int)(zipvine.position.Y / 16f);
            Player.pulley = true;
            Player.pulleyDir = 0;

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

        if (Player.controlUp) // Climb vine
            progress += zipvine.ClimbSpeed;
        else if (Player.controlDown)
            progress -= zipvine.ClimbSpeed;

        if (progress < 0) // Adjust progress so we skip to next vine(s) if needed
        {
            if (zipvine.priorVine is null)
                progress = 0;

            while (progress < 0)
            {
                if (zipvine.priorVine is null)
                {
                    progress = 0;
                    break;
                }

                zipvine = zipvine.priorVine;
                progress += 1f;
            }
        }
        else if (progress > 1)
        {
            if (zipvine.nextVine is null)
                progress = 1;

            while (progress > 1)
            {
                if (zipvine.nextVine is null)
                {
                    progress = 1;
                    break;
                }

                zipvine = zipvine.nextVine;
                progress -= 1f;
            }
        }

        Vector2 nextPosition = zipvine.nextVine is null ? zipvine.position : zipvine.nextVine.position;
        Player.position = Vector2.Lerp(zipvine.position, nextPosition, progress) + new Vector2(Player.width, 30);
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
