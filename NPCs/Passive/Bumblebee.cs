using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;
using Verdant.Tiles;
using Verdant.Tiles.Verdant;
using Verdant.Tiles.Verdant.Basic;
using Verdant.World;

namespace Verdant.NPCs.Passive
{
    public class Bumblebee : ModNPC
    {
        const int MaxHoney = 5;

        public ref float State => ref NPC.ai[0];
        public bool Clockwise { get => NPC.ai[1] == 0; set => NPC.ai[1] = value ? 0 : 1; }
        public ref float Timer => ref NPC.ai[2];
        public ref float WaitTimer => ref NPC.ai[3];

        private Vector2 flowerOffset = Vector2.Zero;
        private int honeyCount = 0;

        private List<Point> visitedFlowers = new List<Point>();

        public override void SetStaticDefaults()
        {
            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 2;

            NPCID.Sets.CountsAsCritter[Type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.dontCountMe = true;
            NPC.catchItem = (short)ModContent.ItemType<BumblebeeItem>();
            NPC.DeathSound = SoundID.NPCHit45;

            SpawnModBiomes = [ModContent.GetInstance<Scenes.VerdantBiome>().Type];
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.AddInfo(this, "");

        public override void AI()
        {
            if (State == 0)
            {
                Clockwise = Main.rand.NextBool();
                State = 1;
            }
            else if (State == 1)
            {
                var nearestTile = NearestTile(out Point tile, out Point? flower);

                if (honeyCount >= MaxHoney && flower is not null) //Beehive check
                {
                    Point hive = flower.Value;
                    Vector2 destination = hive.ToWorldCoordinates() + new Vector2(16);

                    NPC.velocity = NPC.DirectionTo(destination) * 2f;
                    NPC.spriteDirection = Math.Sign(NPC.velocity.X);

                    if (Vector2.DistanceSquared(destination, NPC.Center) < 9f)
                    {
                        Beehive.IncreaseFrame(hive);
                        BeehiveSystem.Remove(hive);
                        NPC.active = false;

                        for (int i = 0; i < 3; ++i)
                        {
                            Vector2 vel = new Vector2(Main.rand.NextFloat(1, 3), 0).RotatedByRandom(MathHelper.TwoPi);
                            Dust.NewDust(NPC.Center, 1, 1, DustID.Honey2, vel.X, vel.Y);
                        }
                    }
                    return;
                }

                if (nearestTile < 25 * 25 && flower is null)
                    NPC.velocity = NPC.DirectionFrom(tile.ToWorldCoordinates());
                else
                    FreeMovement(flower);
            }
            else if (State == 2)
                PauseOnFlower();
        }

        private void PauseOnFlower()
        {
            Timer++;

            NPC.velocity *= 0.6f;

            if (Timer > 200)
            {
                State = 1;
                Timer = 0;
                honeyCount++;

                var pos = visitedFlowers.Last();

                if (ModContent.GetModTile(Main.tile[pos].TileType) is not IFlowerTile flower)
                    return;

                flower.OnPollenate(pos.X, pos.Y);
            }
        }

        private void FreeMovement(Point? flower)
        {
            if (flower is not null)
            {
                Vector2 destination = flower.Value.ToWorldCoordinates() + flowerOffset / 2f;

                NPC.velocity = NPC.DirectionTo(destination) * 3;

                if (NPC.DistanceSQ(destination) < 4 * 4)
                {
                    State = 2;
                    visitedFlowers.Add(flower.Value);
                    Timer = 0;
                }
            }
            else
            {
                if (NPC.velocity.LengthSquared() < 1)
                    NPC.velocity = new Vector2(1.2f, 0).RotatedByRandom(MathHelper.TwoPi);
                else
                    NPC.velocity = NPC.velocity.RotatedBy(Main.rand.Next(-1, 2) * 0.2f);
            }

            NPC.spriteDirection = Math.Sign(NPC.velocity.X);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += State != 2 ? 1f : 0.6f;
            NPC.frame.Y = frameHeight * (NPC.frameCounter < 4 ? 0 : 1);

            if (NPC.frameCounter >= 8)
                NPC.frameCounter = 0;
        }

        public float NearestTile(out Point tile, out Point? flower)
        {
            const int MaxDist = 30;

            flower = null;
            tile = Point.Zero;
            Point center = NPC.Center.ToTileCoordinates();
            bool foundTile = false;

            for (int i = center.X - MaxDist; i < center.X + MaxDist; ++i)
            {
                for (int j = center.Y - MaxDist; j < center.Y + MaxDist; ++j)
                {
                    if (Main.tile[i, j].HasTile)
                    {
                        Tile t = Main.tile[i, j];
                        bool closer = Vector2.DistanceSquared(tile.ToVector2(), center.ToVector2()) > Vector2.DistanceSquared(new Vector2(i, j), center.ToVector2());
                        if (WorldGen.SolidTile(t) && closer)
                        {
                            tile = new Point(i, j);
                            foundTile = true;
                        }

                        Point tL = TileHelper.GetTopLeft(new Point(i, j));
                        bool validTile;
                        if (honeyCount >= MaxHoney)
                            validTile = t.TileType == ModContent.TileType<Beehive>() && t.TileFrameX < Beehive.FrameHeight * 2 && Vector2.DistanceSquared((flower ?? Point.Zero).ToWorldCoordinates(), NPC.Center) > Vector2.DistanceSquared(new Vector2(i, j) * 16, NPC.Center);
                        else
                            validTile = Flowers.FlowerIDs.ContainsKey(t.TileType) && IsFlowerValid(tL.X, tL.Y, flower ?? Point.Zero);

                        if (validTile)
                        {
                            flower = tL;

                            if (honeyCount < MaxHoney)
                            {
                                var flowerInfo = Flowers.FlowerIDs[t.TileType];
                                Vector2[] offsets = flowerInfo.OffsetsAt(i, j);

                                if (honeyCount < MaxHoney)
                                    flowerOffset = offsets[NPC.whoAmI % offsets.Length];
                            }
                        }
                    }
                }
            }

            if (!foundTile)
                return 8000 * 8000;
            return Vector2.DistanceSquared(tile.ToWorldCoordinates(), center.ToWorldCoordinates());
        }

        private bool IsFlowerValid(int i, int j, Point oldFlower)
        {
            bool inList = visitedFlowers.Contains(new Point(i, j));
            bool closer = Vector2.DistanceSquared(oldFlower.ToWorldCoordinates(), NPC.Center) > Vector2.DistanceSquared(new Vector2(i, j) * 16, NPC.Center);
            bool isFlower = Flowers.FlowerIDs.ContainsKey(Main.tile[i, j].TileType) && Flowers.FlowerIDs[Main.tile[i, j].TileType].IsValid(i, j);
            return !inList && closer && isFlower;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Vector2 vel = new Vector2(Main.rand.NextFloat(1, 3), 0).RotatedByRandom(MathHelper.TwoPi);
                Dust.NewDust(NPC.Center, 1, 1, DustID.Honey2, vel.X, vel.Y);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => ((spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.raining) ? 2f : 0f) * (spawnInfo.PlayerInTown ? 1.75f : 0f);
    }
}