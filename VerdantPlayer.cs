using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Backgrounds.BGItem;
using Verdant.Backgrounds.BGItem.Verdant;
using Verdant.Items.Verdant.Fishing;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.World;

namespace Verdant
{
    class VerdantPlayer : ModPlayer
    {
        public bool ZoneVerdant;
        public bool ZoneApotheosis;

        public delegate void FloorVisual(Player p, int type);
        public static event FloorVisual FloorVisualEvent;

        public delegate void ItemDrawLayer(PlayerDrawInfo info);
        public static event ItemDrawLayer ItemDrawLayerEvent;

        public delegate void DoOnRespawn(Player p);
        public static event DoOnRespawn OnRespawnEvent;

        public delegate void HitByNPC(Player p, NPC npc, int damage, bool crit);
        public static event HitByNPC HitByNPCEvent;

        public delegate void PreUpdateDelegate(Player p);
        public static event PreUpdateDelegate PreUpdateEvent;

        private bool _showCatchIcon = false;

        public override void ResetEffects()
        {
        }

        public override void UpdateBiomes()
        {
            ZoneVerdant = VerdantWorld.VerdantTiles > 50;
            ZoneApotheosis = VerdantWorld.ApotheosisTiles > 2;
        }

        public override Texture2D GetMapBackgroundImage()
        {
            if (ZoneVerdant)
                return mod.GetTexture("Backgrounds/VerdantMap");
            return null;
        }

        public override void UpdateBiomeVisuals()
        {
            VerdantPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<VerdantPlayer>();
            player.ManageSpecialBiomeVisuals("Verdant:Verdant", modPlayer.ZoneVerdant && !Main.dayTime);
        }

        public static void Unload()
        {
            FloorVisualEvent = null;
            ItemDrawLayerEvent = null;
            HitByNPCEvent = null;
            OnRespawnEvent = null;
            PreUpdateEvent = null;
        }

        public override void OnRespawn(Player player)
        {
            OnRespawnEvent?.Invoke(player);
            //player.hitTile.AddDamage(player.hitTile.HitObject(x, y, 1), 0, false); USEFUL?
        }

        public override void PreUpdate()
        {
            PreUpdateEvent?.Invoke(player);

            //bootleg floor effects
            Point left = player.TileCoordsBottomLeft();
            Point right = player.TileCoordsBottomRight();

            if (player.velocity.Y >= 0 && (Helper.SolidTopTile(left.X, left.Y) || Helper.SolidTopTile(right.X, right.Y)))
                TileFloor(left, right, Framing.GetTileSafely(left.X, left.Y).type, Framing.GetTileSafely(right.X, right.Y).type);

            if (player.HeldItem.type == ModContent.ItemType<Items.Verdant.Tools.BouncebloomItem>())
            {
                if (!player.controlDown)
                    player.maxFallSpeed *= 0.4f;
            }

            if (Main.hasFocus)
                UpdateBGItems();
        }

        private void TileFloor(Point left, Point right, int lType, int rType)
        {
            bool lValid = lType == ModContent.TileType<Bouncebloom>() && Helper.SolidTopTile(left.X, left.Y);
            bool rValid = rType == ModContent.TileType<Bouncebloom>() && Helper.SolidTopTile(right.X, right.Y);
            if (lValid || rValid)
            {
                float newVel = -10f;
                if (player.controlJump) //Bigger jump if jumping
                    newVel = -13.5f;
                if (player.controlDown) //NO jump if holding down
                    return;
                player.velocity.Y = newVel;

                int offsetX = 0;
                int offsetY = 0;
                if (lValid)
                {
                    offsetX = left.X - Framing.GetTileSafely(left.X, left.Y).frameX / 18;
                    offsetY = left.Y - Framing.GetTileSafely(left.X, left.Y).frameY / 18;
                }
                else if (rValid)
                {
                    offsetX = right.X - Framing.GetTileSafely(right.X, right.Y).frameX / 18;
                    offsetY = right.Y - Framing.GetTileSafely(right.X, right.Y).frameY / 18;
                }

                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 2; ++j)
                    {
                        Framing.GetTileSafely(offsetX + i, offsetY + j).frameY = (short)(38 + (18 * j));

                        if (Main.rand.Next(5) <= 3 && j == 0)
                        {
                            int type = Main.rand.Next(2) == 0 ? mod.GetGoreSlot("Gores/Verdant/RedPetalFalling") : mod.GetGoreSlot("Gores/Verdant/LushLeaf");
                            Gore.NewGore((new Vector2(offsetX + i, offsetY + j) * 16) + new Vector2(Main.rand.Next(18), Main.rand.Next(18)), Vector2.Zero, type);
                        }
                    }
                }
            }
        }

        private void UpdateBGItems()
        {
            if (player.GetModPlayer<VerdantPlayer>().ZoneVerdant && (player.Center.Y + (Main.screenHeight / 2f)) / 16f < Main.worldSurface) //Spawn BG items only when in the Verdant and above ground
            {
                if (Main.rand.NextBool(FlotieBG.SpawnChance))
                {
                    Vector2 pos = player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 0.75f), (int)(Main.screenWidth * 0.75f)), 
                        Main.rand.Next(-(int)(Main.screenWidth * 0.75f), (int)(Main.screenWidth * 0.75f)));
                    BackgroundItemManager.AddItem(new FlotieBG(pos));
                }
                if (Main.raining && Main.rand.NextBool(LushLeafBG.SpawnChance))
                {
                    Vector2 pos = player.Center - new Vector2(Main.rand.Next(-(int)(Main.screenWidth * 1.1f), (int)(Main.screenWidth * 1.1f)), Main.screenHeight * 0.52f);
                    BackgroundItemManager.AddItem(new LushLeafBG(pos));
                }
            }
        }

        public override void PostUpdate()
        {
            //catch icon info
            if (_showCatchIcon)
            {
                player.showItemIconText = "Catch";
                player.showItemIcon = false;
            }
        }

        public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
        {
            if (junk && player.GetModPlayer<VerdantPlayer>().ZoneVerdant)
                junk = Main.rand.Next(7) < 2; //verdant is not as trashy
            if (junk) return;

            if (player.GetModPlayer<VerdantPlayer>().ZoneVerdant && questFish == ModContent.ItemType<Shellfish>() && Main.rand.NextBool(3))
                caughtType = ModContent.ItemType<Shellfish>();
        }

        public void FloorVisuals(Player p, int t) => FloorVisualEvent?.Invoke(p, t);

        public override void OnHitByNPC(NPC npc, int damage, bool crit) => HitByNPCEvent?.Invoke(player, npc, damage, crit);

        public static readonly PlayerLayer ItemEffects = new PlayerLayer("Verdant", "ItemEffects", PlayerLayer.MountFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
                return;
            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.active && !drawPlayer.outOfRange)
                ItemDrawLayerEvent?.Invoke(drawInfo);
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            ItemEffects.visible = true;
            layers.Insert(0, ItemEffects);
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            VerdantPlayer modOther = other.GetModPlayer<VerdantPlayer>();
            modOther.ZoneVerdant = ZoneVerdant;
            modOther.ZoneApotheosis = ZoneApotheosis;
        }
    }
}