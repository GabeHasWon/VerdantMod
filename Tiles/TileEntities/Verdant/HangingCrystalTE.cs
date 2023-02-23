using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Verdant.Projectiles.Magic;
using Verdant.Tiles.Verdant.Misc;

namespace Verdant.Tiles.TileEntities.Verdant;

internal class HangingCrystalTE : ModTileEntity
{
    const int MaxPlants = 4;

    private List<int> plants = new List<int>(5);
    private int time = 0;
    private int count = 0;

    public override bool IsTileValidForEntity(int x, int y) => Main.tile[x, y].HasTile && Main.tile[x, y].TileType == ModContent.TileType<HangingCrystal>() && Main.tile[x, y].TileFrameX == 18;

    public override void Update()
    {
        time++;

        if (plants.Count >= MaxPlants)
        {
            ClearPlants();

            if (plants.Count >= MaxPlants)
                return;
        }

        if (count >= MaxPlants)
        {
            if (time > 0)
                time = -2000;
            else if (time == 0)
            {
                ClearPlants();

                count = plants.Count;
            }
            return;
        }

        Player nearest = Main.player[Player.FindClosest(Position.ToWorldCoordinates(), 2, 2)];

        if (nearest.DistanceSQ(Position.ToWorldCoordinates()) > 4000 * 4000)
            return;

        if (time % 180 == 0)
            SpawnHealPlant();
    }

    private void ClearPlants()
    {
        plants = plants.Distinct().ToList();
        plants.RemoveAll(x => !Main.projectile[x].active || Main.projectile[x].type != ModContent.ProjectileType<HealPlants>());
    }

    private void SpawnHealPlant()
    {
        const int Distance = 14;

        var tiles = new List<Point16>();

        for (int i = Position.X - Distance; i < Position.X + Distance; ++i)
            for (int j = Position.Y - Distance; j < Position.Y + Distance; ++j)
                if (Main.tile[i, j].HasTile && Main.tileSolid[Main.tile[i, j].TileType])
                    tiles.Add(new Point16(i, j));

        tiles.RemoveAll(x => WorldGen.SolidOrSlopedTile(x.X, x.Y - 1));

        var pos = Main.rand.Next(tiles) - new Point16(0, 1);
        var placePos = pos.ToWorldCoordinates(8, 4 + (Main.tile[pos.X, pos.Y + 1].IsHalfBlock ? 8 : 0));
        int proj = Projectile.NewProjectile(new EntitySource_Wiring(pos.X, pos.Y), placePos, Vector2.Zero, ModContent.ProjectileType<HealPlants>(), 0, 0, Main.myPlayer);

        plants.Add(proj);
        count++;
    }
}
