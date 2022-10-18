using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.Verdant.Decor.VerdantFurniture;

namespace Verdant.Items.Verdant.Equipables
{
    [AutoloadEquip(EquipType.Legs)]
    class SproutInABoot : ModItem
    {
        internal const int MaxWater = 80;

        private readonly Texture2D _dryTex;

        private int _water = MaxWater;

        public SproutInABoot()
        {
            _dryTex = ModContent.Request<Texture2D>(Texture + "Dry").Value;
        }

        public override void Load()
        {
            EquipLoader.AddEquipTexture(Mod, Texture + "_Legs_2", EquipType.Legs, this, nameof(SproutInABoot) + "_Legs_2");
            EquipLoader.AddEquipTexture(Mod, Texture + "_Legs_3", EquipType.Legs, this, nameof(SproutInABoot) + "_Legs_3");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sprout in Boots");
            Tooltip.SetDefault("Spreads leafy platforms automatically under the player when worn\nUses a single Lush Leaf per platform placed\nNeeds water to continue working\n'Surprisingly comfortable & durable'");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (_water <= 0)
                tooltips.Insert(1, new TooltipLine(Mod, "DrySprout", "[c/BC775C:It needs water!]"));
        }

        public override void SetDefaults()
        {
            Item.defense = 1;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 3);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<VerdantPlayer>().sproutBoots = true;

            if (player.wet)
                _water = MaxWater;

            if (_water <= 0 || player.velocity.Y > player.gravity || player.velocity.Y < 0 || Math.Abs(player.velocity.X) < 0.01f)
                return;

            Point t = player.TileCoordsBottomCentred();
            Tile ground = Framing.GetTileSafely(t.X, t.Y);

            if (ground.HasTile && (!ground.TopSlope || (player.velocity.X < 0 && ground.RightSlope) || (player.velocity.X > 0 && ground.LeftSlope)))
            {
                if (player.velocity.X > 0)
                    TryPlacePlatform(t.X + 1, t.Y, player);
                else
                    TryPlacePlatform(t.X - 1, t.Y, player);
            }
        }

        private void TryPlacePlatform(int x, int y, Player player)
        {
            int ind = -1;

            for (int i = 0; i < player.inventory.Length; ++i)
            {
                Item item = player.inventory[i];

                if (!item.IsAir && item.type == ModContent.ItemType<LushLeaf>() && item.stack > 0)
                {
                    ind = i;
                    break;
                }
            }

            if (ind == -1)
                return;

            Tile tile = Main.tile[x, y];
            if (tile.HasTile && !Main.tileCut[tile.TileType])
                return;

            WorldGen.PlaceTile(x, y, ModContent.TileType<VerdantPlatformsDropLeaves>(), false, true);

            for (int i = 0; i < 3; ++i)
                Dust.NewDust(new Vector2(x, y) * 16, 16, 16, DustID.Grass, 0, 0);

            if (--player.inventory[ind].stack <= 0)
                player.inventory[ind].TurnToAir();

            _water--;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (_water <= 0)
                spriteBatch.Draw(_dryTex, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return _water > 0;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (_water > 0)
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, _water.ToString(), position + new Vector2(-4, 16), Color.White, Color.Black * 0.5f, 0f, origin, Vector2.One * 1.15f * scale);
            else
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, "X", position + new Vector2(-4, 16), Color.Red, Color.Black * 0.5f, 0f, origin, Vector2.One * 1.15f * scale);
        }

        public int GetWater() => _water;
    }
}