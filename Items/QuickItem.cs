using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.UI;

namespace Verdant.Items
{
    public class QuickItem
    {
        public static void SetStatic(ModItem i, string name, string tooltip = "", bool isStaff = false)
        {
            i.DisplayName.SetDefault(name);
            i.Tooltip.SetDefault(tooltip);

            Item.staff[i.Item.type] = true;
        }

        /// <summary>Sets the defaults of an item to a basic item that places a block.</summary>
        /// <param name="i">Item to set the defaults of.</param>
        /// <param name="w">Width of the item.</param>
        /// <param name="h">Height of the item.</param>
        /// <param name="tile">Tile ID to place.</param>
        /// <param name="consumable">If the item is consumable. Defaults to true.</param>
        /// <param name="placeStyle">The placeStyle of the item. Defaults to -1, which skips setting it.</param>
        /// <param name="rarity">Rarity of the item. Defaults to <see cref="ItemRarityID.White"/>.</param>
        public static void SetBlock(ModItem i, int w, int h, int tile, bool consumable = true, int placeStyle = -1, int rarity = ItemRarityID.White, int maxStack = 999, int createStyle = 0, bool autoReuse = true)
        {
            i.Item.width = w;
            i.Item.height = h;
            i.Item.createTile = tile;
            i.Item.placeStyle = createStyle;
            i.Item.useTurn = true;
            i.Item.autoReuse = autoReuse;
            i.Item.useAnimation = 15;
            i.Item.useTime = 10;
            i.Item.maxStack = maxStack;
            i.Item.useStyle = ItemUseStyleID.Swing;
            i.Item.consumable = consumable;
            i.Item.rare = rarity;

            if (placeStyle != -1)
                i.Item.placeStyle = placeStyle;
        }

        /// <summary>Sets the defaults of an item to a basic item that places a wall.</summary>
        /// <param name="i">Item to set the defaults of.</param>
        /// <param name="w">Width of the item.</param>
        /// <param name="h">Height of the item.</param>
        /// <param name="wall">Wall ID to place.</param>
        /// <param name="consumable">If the item is consumable. Defaults to true.</param>
        public static void SetWall(ModItem i, int w, int h, int wall, bool consumable = true)
        {
            i.Item.width = w;
            i.Item.height = h;
            i.Item.createWall = wall;

            i.Item.useTurn = true;
            i.Item.autoReuse = true;
            i.Item.useAnimation = 15;
            i.Item.useTime = 7;
            i.Item.maxStack = 999;
            i.Item.useStyle = ItemUseStyleID.Swing;
            i.Item.consumable = consumable;
        }

        public static void SetStaff(ModItem i, int w, int h, int shoot, int use, int damage, int mana, float speed = 5f, float kB = 2f, int rarity = 1)
        {
            i.Item.width = w;
            i.Item.height = h;
            i.Item.useAnimation = use;
            i.Item.useTime = use;
            i.Item.shootSpeed = speed;
            i.Item.knockBack = kB;
            i.Item.damage = damage;
            i.Item.shoot = shoot;
            i.Item.rare = rarity;
            i.Item.mana = mana;
            i.Item.noMelee = true;
            i.Item.useStyle = ItemUseStyleID.Swing;
            i.Item.DamageType = DamageClass.Magic;
        }

        public static void SetMinion(ModItem i, int w, int h, int shoot, int damage, int mana, int rarity = 1)
        {
            i.Item.width = w;
            i.Item.height = h;
            i.Item.useAnimation = 16;
            i.Item.useTime = 16;
            i.Item.shootSpeed = 0f;
            i.Item.knockBack = 1f;
            i.Item.damage = damage;
            i.Item.shoot = shoot;
            i.Item.rare = rarity;
            i.Item.mana = mana;
            i.Item.noMelee = true;
            i.Item.useStyle = ItemUseStyleID.Swing;
            i.Item.DamageType = DamageClass.Summon;
            i.Item.UseSound = SoundID.Item44;
        }

        public static void SetCritter(ModItem i, int w, int h, int npcType, int rarity = 0, int b = 5)
        {
            i.Item.width = w;
            i.Item.height = h;
            i.Item.useAnimation = 16;
            i.Item.useTime = 16;
            i.Item.damage = 0;
            i.Item.rare = rarity;
            i.Item.maxStack = 999;
            i.Item.noUseGraphic = true;
            i.Item.noMelee = false;
            i.Item.useStyle = ItemUseStyleID.Swing;
            i.Item.bait = b;
            i.Item.makeNPC = (short)npcType;
            i.Item.autoReuse = true;
            i.Item.consumable = true;
        }

        public static void SetMaterial(ModItem i, int w, int h, int rarity = 0, int maxStack = 999, bool consumable = false, int value = 0, bool noUseGraphic = false)
        {
            i.Item.width = w;
            i.Item.height = h;
            i.Item.useAnimation = 16;
            i.Item.useTime = 16;
            i.Item.damage = 0;
            i.Item.rare = rarity;
            i.Item.maxStack = maxStack;
            i.Item.value = value;
            i.Item.noMelee = true;
            i.Item.noUseGraphic = noUseGraphic;
            i.Item.useStyle = ItemUseStyleID.Swing;
            i.Item.autoReuse = true;
            i.Item.consumable = consumable;
        }

        public static void SetFishingRod(ModItem i, int w, int h, int fish, int shoot, float shootSpeed, int rarity = 0)
        {
            i.Item.width = w;
            i.Item.height = h;
            i.Item.useAnimation = 16;
            i.Item.useTime = 16;
            i.Item.damage = 0;
            i.Item.knockBack = 0f;
            i.Item.rare = rarity;
            i.Item.maxStack = 1;
            i.Item.shootSpeed = shootSpeed;
            i.Item.shoot = shoot;
            i.Item.noMelee = true;
            i.Item.fishingPole = fish;
            i.Item.autoReuse = true;
            i.Item.consumable = false;
        }

        public static void SetLightPet(ModItem i, int w, int h, int rarity = 0)
        {
            i.Item.CloneDefaults(ItemID.ShadowOrb);
            i.Item.width = w;
            i.Item.height = h;
            i.Item.rare = rarity;
        }

        /// <summary>Adds a recipe to the mod given the following items and tiles.</summary>
        /// <param name="item">Item to use as a result.</param>
        /// <param name="mod">Mod to use to add to.</param>
        /// <param name="tile">Tile to use for crafting. -1 means no tile is used. Defaults to -1.</param>
        /// <param name="resultStack">Stack created with this crafting recipe. Defaults to 1.</param>
        /// <param name="ingredients">Ingredients to use. Formatted as a (int, int) pair.</param>
        public static void AddRecipe(ModItem item, int tile = -1, int resultStack = 1, params (int, int)[] ingredients)
        {
            if (ingredients.Length <= 0)
                throw new ArgumentException("Ingredents array is empty.", nameof(ingredients));

            Recipe r = Recipe.Create(item.Type, resultStack);
            for (int i = 0; i < ingredients.Length; ++i)
                r.AddIngredient(ingredients[i].Item1, ingredients[i].Item2);

            if (tile != -1) 
                r.AddTile(tile);
            r.Register();
        }

        /// <summary>Adds a recipe to the mod given the following items and tiles.</summary>
        /// <param name="id">Item ID to use as a result.</param>
        /// <param name="mod">Mod to use to add to.</param>
        /// <param name="tile">Tile to use for crafting. -1 means no tile is used. Defaults to -1.</param>
        /// <param name="resultStack">Stack created with this crafting recipe. Defaults to 1.</param>
        /// <param name="ingredients">Ingredients to use. Formatted as a (int, int) pair.</param>
        public static void AddRecipe(int id, Mod mod, int tile = -1, int resultStack = 1, params (int, int)[] ingredients)
        {
            if (ingredients.Length <= 0)
                throw new ArgumentException("Ingredents array is empty.", "ingredients");

            Recipe r = Recipe.Create(id, resultStack);
            for (int i = 0; i < ingredients.Length; ++i)
                r.AddIngredient(ingredients[i].Item1, ingredients[i].Item2);
            if (tile != -1) r.AddTile(tile);
            r.Register();
        }

        public static bool CanCritterSpawnCheck() => !Framing.GetTileSafely(Main.MouseWorld).HasTile || !Main.tileSolid[Framing.GetTileSafely(Main.MouseWorld).TileType];

        public static void ToggleBookUI(string title, float titleScale, object[] body)
        {
            if (ModContent.GetInstance<UISystem>().BookInterface.CurrentState is not null)
			{
                SoundEngine.PlaySound(SoundID.MenuClose);
                ModContent.GetInstance<UISystem>().BookInterface.SetState(null);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuOpen);
                ModContent.GetInstance<UISystem>().BookInterface.SetState(new BookState(title, titleScale, body));
            }
        }
    }
}
