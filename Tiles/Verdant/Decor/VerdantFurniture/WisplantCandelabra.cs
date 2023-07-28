using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Systems;

namespace Verdant.Tiles.Verdant.Decor.VerdantFurniture
{
    internal class WisplantCandelabra : ModTile
    {
        public override void SetStaticDefaults() => CandelabraHelper.Defaults(this, new Color(253, 221, 3), false);
        public override void HitWire(int i, int j) => CandelabraHelper.WireHit(i, j);

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Vector3 light = new Vector3(0.5f, 0.16f, 0.30f) * 3f;
            if (Framing.GetTileSafely(i, j).TileFrameX == 0)
            {
                r = light.X;
                g = light.Y;
                b = light.Z;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (Main.gamePaused)
                return;

            RandomUpdating.CircularUpdate(i, j, 10, 1600, (i, j) =>
            {
                Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.TerraBlade, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            });
        }

        public override void RandomUpdate(int i, int j)
        {
            RandomUpdating.CircularUpdate(i, j, 10, 80, (i, j) =>
            {
                Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.TerraBlade, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            });
        }
    }
}
