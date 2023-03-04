using Terraria.ModLoader;
using Verdant.Projectiles.Misc;

namespace Verdant.Tiles.Verdant.Misc;

internal class WisplantInfusedLushBall : MudBoulderTile
{
    protected override int ProjectileType => ModContent.ProjectileType<WisplantInfusedLushBallProjectile>();
}