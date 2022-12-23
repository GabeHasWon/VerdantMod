using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Verdant.Projectiles.Misc;
using System;

namespace Verdant.Tiles.Verdant.Basic;

internal class MudBoulderTile : ModTile
{
    protected virtual int ProjectileType => ModContent.ProjectileType<MudBoulder>();

    public override void SetStaticDefaults() => QuickTile.SetMulti(this, 1, 1, DustID.Mud, SoundID.Dig, false, new Color(73, 32, 18));

    public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        var spawnPos = new Vector2(i + 0.5f, j + 0.4f) * 16;
        Player nearest = Main.player[Player.FindClosest(spawnPos, 1, 1)];

        if (nearest.active && !nearest.dead)
            Projectile.NewProjectile(new EntitySource_TileBreak(i, j), spawnPos, new Vector2(-Math.Sign(nearest.Center.X - spawnPos.X) * 0.001f, 0), ProjectileType, 10, 3f);
    }
}