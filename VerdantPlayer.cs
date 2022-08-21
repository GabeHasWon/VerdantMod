using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Verdant.Backgrounds.BGItem;
using Verdant.Backgrounds.BGItem.Verdant;
using Verdant.Effects;
using Verdant.Foreground;
using Verdant.Foreground.Parallax;
using Verdant.Items.Verdant.Fishing;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.World;

namespace Verdant
{
    class VerdantPlayer : ModPlayer
    {
        public bool ZoneVerdant;
        public bool ZoneApotheosis;

        public bool heartOfGrowth = false;

        public float lastSlotsMinion = 0;

        private float _steamIntensity = 1f;
        private float _steamProgress = 0f;

        public delegate void FloorVisual(Player p, int type);
        public static event FloorVisual FloorVisualEvent;

        public delegate void ItemDrawLayer(PlayerDrawSet info);
        public static event ItemDrawLayer ItemDrawLayerEvent;

        public delegate void DoOnRespawn(Player p);
        public static event DoOnRespawn OnRespawnEvent;

        public delegate void HitByNPC(Player p, NPC npc, int damage, bool crit);
        public static event HitByNPC HitByNPCEvent;

        public delegate void PreUpdateDelegate(Player p);
        public static event PreUpdateDelegate PreUpdateEvent;

        public override void ResetEffects()
        {
            ZoneApotheosis = false;

            if (heartOfGrowth) //perm bonus
                Player.maxMinions++;
        }

        public override void SaveData(TagCompound tag) => tag.Add("heartOfGrowth", heartOfGrowth);

        public override void LoadData(TagCompound tag) => heartOfGrowth = tag.GetBool("heartOfGrowth");

        public override void Unload()
        {
            FloorVisualEvent = null;
            ItemDrawLayerEvent = null;
            HitByNPCEvent = null;
            OnRespawnEvent = null;
            PreUpdateEvent = null;
        }

        public override void OnRespawn(Player player) => OnRespawnEvent?.Invoke(player);

        public override void PreUpdate()
        {
            PreUpdateEvent?.Invoke(Player);

            //bootleg floor effects
            Point left = Player.TileCoordsBottomLeft();
            Point right = Player.TileCoordsBottomRight();

            if (Player.whoAmI == Main.myPlayer && Player.velocity.Y >= 0 && (TileHelper.SolidTopTile(left.X, left.Y) || TileHelper.SolidTopTile(right.X, right.Y)))
                TileFloor(left, right, Framing.GetTileSafely(left.X, left.Y).TileType, Framing.GetTileSafely(right.X, right.Y).TileType);

            if (Player.HeldItem.type == ModContent.ItemType<Items.Verdant.Tools.BouncebloomItem>())
            {
                if (!Player.controlDown)
                    Player.maxFallSpeed *= 0.4f;
            }

            if (Main.hasFocus)
                AddForegroundOrBackground();
        }

        public override void PostUpdateMiscEffects() => lastSlotsMinion = Player.slotsMinions;

        private void TileFloor(Point left, Point right, int lType, int rType)
        {
            bool lValid = lType == ModContent.TileType<Bouncebloom>() && TileHelper.SolidTopTile(left.X, left.Y);
            bool rValid = rType == ModContent.TileType<Bouncebloom>() && TileHelper.SolidTopTile(right.X, right.Y);
            if (lValid || rValid)
            {
                float newVel = -10f;
                if (Player.controlJump) //Bigger jump if jumping
                    newVel = -13.5f;
                if (Player.controlDown) //NO jump if holding down
                    return;
                Player.velocity.Y = newVel;

                int offsetX = 0;
                int offsetY = 0;
                if (lValid)
                {
                    offsetX = left.X - Framing.GetTileSafely(left.X, left.Y).TileFrameX / 18;
                    offsetY = left.Y - Framing.GetTileSafely(left.X, left.Y).TileFrameY / 18;
                }
                else if (rValid)
                {
                    offsetX = right.X - Framing.GetTileSafely(right.X, right.Y).TileFrameX / 18;
                    offsetY = right.Y - Framing.GetTileSafely(right.X, right.Y).TileFrameY / 18;
                }

                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 2; ++j)
                    {
                        Framing.GetTileSafely(offsetX + i, offsetY + j).TileFrameY = (short)(38 + (18 * j));

                        if (Main.rand.Next(5) <= 3 && j == 0)
                        {
                            int type = Main.rand.NextBool(2)? Mod.Find<ModGore>("RedPetalFalling").Type : Mod.Find<ModGore>("LushLeaf").Type;
                            Gore.NewGore(Player.GetSource_TileInteraction(offsetX + i, offsetY + j), (new Vector2(offsetX + i, offsetY + j) * 16) + new Vector2(Main.rand.Next(18), Main.rand.Next(18)), Vector2.Zero, type);
                        }
                    }
                }
            }
        }

        private void AddForegroundOrBackground()
        {
            if (Player.GetModPlayer<VerdantPlayer>().ZoneVerdant) //Spawn BG items only when in the Verdant and above ground
            {
                if ((Player.Center.Y + Main.screenHeight / 2f) / 16f < Main.worldSurface) 
                {
                    if (Main.rand.NextBool(FlotieBG.SpawnChance))
                    {
                        Vector2 pos = Player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 0.75f), (int)(Main.screenWidth * 0.75f)),
                            Main.rand.Next(-(int)(Main.screenWidth * 0.75f), (int)(Main.screenWidth * 0.75f)));
                        BackgroundItemManager.AddItem(new FlotieBG(pos));
                    }
                    if (Main.raining && Main.rand.NextBool(LushLeafBG.SpawnChance))
                    {
                        Vector2 pos = Player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 1.1f), (int)(Main.screenWidth * 1.1f)), Main.screenHeight * 0.9f);
                        BackgroundItemManager.AddItem(new LushLeafBG(pos));
                    }
                }

                int leafFGChance = LushLeafFG.SpawnChance(Player);
                if (leafFGChance != -1 && Main.rand.NextBool(leafFGChance))
                {
                    Vector2 pos = Player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 2f), (int)(Main.screenWidth * 2f)), Main.screenHeight * 0.52f);
                    ForegroundManager.AddItem(new LushLeafFG(pos));
                }
            }
        }

        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && attempt.questFish == ModContent.ItemType<Shellfish>() && Main.rand.NextBool(3))
                itemDrop = ModContent.ItemType<Shellfish>();
        }

        public void FloorVisuals(Player p, int t) => FloorVisualEvent?.Invoke(p, t);
        public override void OnHitByNPC(NPC npc, int damage, bool crit) => HitByNPCEvent?.Invoke(Player, npc, damage, crit);

        public void InvokeDrawLayer(PlayerDrawSet set) => ItemDrawLayerEvent?.Invoke(set);

        public void UpdateBiomeVisuals() //NEEDSUPDATING
        {
            if (!Filters.Scene[EffectIDs.BiomeSteam].Active && ModContent.GetInstance<VerdantClientConfig>().EnableSteam)
            {
                if (ZoneVerdant && Player.position.Y / 16f > Main.worldSurface)
                {
                    Filters.Scene.Activate(EffectIDs.BiomeSteam, Vector2.Zero); //idk why I need to use UseImage twice but it works so I aint gonna complain
                    Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImage(Mod.Assets.Request<Texture2D>("Effects/Screen/Steam", ReLogic.Content.AssetRequestMode.AsyncLoad).Value, 0);
                    Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImage(Mod.Assets.Request<Texture2D>("Effects/Screen/Steam", ReLogic.Content.AssetRequestMode.AsyncLoad).Value, 1);
                    _steamIntensity = 1f;
                }
            }
            else
            {
                bool validArea = ZoneVerdant && Player.position.Y / 16f > Main.worldSurface && ModContent.GetInstance<VerdantClientConfig>().EnableSteam;
                float baseIntensity = validArea ? 0.94f : 1f;
                _steamIntensity = MathHelper.Lerp(_steamIntensity, baseIntensity, 0.02f);

                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseTargetPosition(Player.Center + (Vector2.UnitY * Player.gfxOffY));
                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseIntensity(_steamIntensity);
                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseProgress(_steamProgress += 0.004f);
                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImageScale(new Vector2(Main.screenWidth, Main.screenHeight), 0);
                Filters.Scene[EffectIDs.BiomeSteam].GetShader().UseImageScale(new Vector2(512, 512), 1);

                if (!validArea && _steamIntensity > 0.99f)
                {
                    Filters.Scene[EffectIDs.BiomeSteam].Deactivate();

                    _steamProgress = 0;
                    _steamIntensity = 1f;
                }
            }
        }
    }
}