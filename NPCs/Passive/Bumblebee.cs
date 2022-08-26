using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Critter;
using Verdant.Tiles.Verdant;

namespace Verdant.NPCs.Passive
{
    public class Bumblebee : ModNPC
    {
        public ref float State => ref NPC.ai[0];
        public bool Clockwise { get => NPC.ai[1] == 0; set => NPC.ai[1] = value ? 0 : 1; }
        public ref float Timer => ref NPC.ai[2];

        private List<Point> visitedFlowers = new List<Point>();

        public override void SetStaticDefaults()
        {
            Main.npcCatchable[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 18;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.dontCountMe = true;
            NPC.catchItem = (short)ModContent.ItemType<FlotieItem>();
            SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("A bee with extra bumble. Bumble with a little bit more bee added. Speck with wings."),
            });
        }

        public override void AI()
        {
            if (State == 0)
            {
                Clockwise = Main.rand.NextBool();
                State = 1;
            }
            else if (State == 1)
            {
                var nearestTile = NearestTile(out Point? flower);

                if (nearestTile > 80 * 80)
                {

                }
                else
                    FreeMovement(flower);
            }
            else if (State == 2)
                PauseOnFlower();
        }

        private void PauseOnFlower()
        {
            Timer++;

            NPC.velocity *= 0.8f;

            if (Timer > 180)
            {
                State = 1;
                Timer = 0;
            }
        }

        private void FreeMovement(Point? flower)
        {
            if (flower is not null)
            {
                NPC.velocity = NPC.DirectionTo(flower.Value.ToWorldCoordinates()) * 4;

                if (NPC.DistanceSQ(flower.Value.ToWorldCoordinates()) < 8 * 8)
                {
                    State = 2;
                    visitedFlowers.Add(flower.Value);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            NPC.frame.Y = frameHeight * (NPC.frameCounter < 4 ? 0 : 1);

            if (NPC.frameCounter >= 8)
                NPC.frameCounter = 0;
        }

        public float NearestTile(out Point? flower)
        {
            const int MaxDist = 20;

            flower = null;
            Point center = NPC.Center.ToTileCoordinates();
            Point tile = Point.Zero;
            bool foundTile = false;

            for (int i = center.X - MaxDist; i < center.X + MaxDist; ++i)
            {
                for (int j = center.Y - MaxDist; j < center.Y + MaxDist; ++j)
                {
                    if (Vector2.DistanceSquared(tile.ToVector2(), center.ToVector2()) > Vector2.DistanceSquared(new Vector2(i, j), center.ToVector2()))
                    {
                        Tile t = Main.tile[i, j];
                        if (WorldGen.SolidTile(t))
                        {
                            tile = new Point(i, j);
                            foundTile = true;
                        }
                        if (Flowers.FlowerIDs.Contains(t.TileType) && (flower is null || IsFlowerValid(i, j, flower.Value)))
                        {
                            TileObjectData data = TileObjectData.GetTileData(i, j);

                            (int x, int y) = (i, j);
                            WorldGen.GetTopLeftAndStyles(ref x, ref y, data.Width, data.Height, 0, 0);

                            flower = new Point(x, y);
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
            return !inList && Vector2.DistanceSquared(oldFlower.ToWorldCoordinates(), NPC.Center) > Vector2.DistanceSquared(new Vector2(i, j) * 16, NPC.Center);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
                for (int i = 0; i < 2; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) => ((spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && Main.raining) ? 2f : 0f) * (spawnInfo.PlayerInTown ? 1.75f : 0f);
    }
}