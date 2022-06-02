using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items
{
    public class QuickItem
    {
        public static void SetStatic(ModItem i, string name, string tooltip = "", bool isStaff = false)
        {
            i.DisplayName.SetDefault(name);
            i.Tooltip.SetDefault(tooltip);

            Item.staff[i.item.type] = true;
        }

        /// <summary>Sets the defaults of an item to a basic item that places a block.</summary>
        /// <param name="i">Item to set the defaults of.</param>
        /// <param name="w">Width of the item.</param>
        /// <param name="h">Height of the item.</param>
        /// <param name="tile">Tile ID to place.</param>
        /// <param name="consumable">If the item is consumable. Defaults to true.</param>
        /// <param name="placeStyle">The placeStyle of the item. Defaults to -1, which skips setting it.</param>
        /// <param name="rarity">Rarity of the item. Defaults to <see cref="ItemRarityID.White"/>.</param>
        public static void SetBlock(ModItem i, int w, int h, int tile, bool consumable = true, int placeStyle = -1, int rarity = ItemRarityID.White)
        {
            i.item.width = w;
            i.item.height = h;
            i.item.createTile = tile;

            i.item.useTurn = true;
            i.item.autoReuse = true;
            i.item.useAnimation = 15;
            i.item.useTime = 10;
            i.item.maxStack = 999;
            i.item.useStyle = ItemUseStyleID.SwingThrow;
            i.item.consumable = consumable;
            i.item.rare = rarity;

            if (placeStyle != -1)
                i.item.placeStyle = placeStyle;
        }

        /// <summary>Sets the defaults of an item to a basic item that places a wall.</summary>
        /// <param name="i">Item to set the defaults of.</param>
        /// <param name="w">Width of the item.</param>
        /// <param name="h">Height of the item.</param>
        /// <param name="wall">Wall ID to place.</param>
        /// <param name="consumable">If the item is consumable. Defaults to true.</param>
        public static void SetWall(ModItem i, int w, int h, int wall, bool consumable = true)
        {
            i.item.width = w;
            i.item.height = h;
            i.item.createWall = wall;

            i.item.useTurn = true;
            i.item.autoReuse = true;
            i.item.useAnimation = 15;
            i.item.useTime = 10;
            i.item.maxStack = 999;
            i.item.useStyle = ItemUseStyleID.SwingThrow;
            i.item.consumable = consumable;
        }

        public static void SetStaff(ModItem i, int w, int h, int shoot, int use, int damage, int mana, float speed = 5f, float kB = 2f, int rarity = 1)
        {
            i.item.width = w;
            i.item.height = h;
            i.item.useAnimation = use;
            i.item.useTime = use;
            i.item.shootSpeed = speed;
            i.item.knockBack = kB;
            i.item.damage = damage;
            i.item.shoot = shoot;
            i.item.rare = rarity;
            i.item.mana = mana;

            i.item.noMelee = true;
            i.item.useStyle = ItemUseStyleID.SwingThrow;
            i.item.magic = true;
        }

        public static void SetMinion(ModItem i, int w, int h, int shoot, int damage, int mana, int rarity = 1)
        {
            i.item.width = w;
            i.item.height = h;
            i.item.useAnimation = 16;
            i.item.useTime = 16;
            i.item.shootSpeed = 0f;
            i.item.knockBack = 1f;
            i.item.damage = damage;
            i.item.shoot = shoot;
            i.item.rare = rarity;
            i.item.mana = mana;
            i.item.UseSound = SoundID.Item44;

            i.item.noMelee = true;
            i.item.useStyle = ItemUseStyleID.SwingThrow;
            i.item.summon = true;
        }

        public static void SetCritter(ModItem i, int w, int h, int npcType, int rarity = 0, int b = 5)
        {
            i.item.width = w;
            i.item.height = h;
            i.item.useAnimation = 16;
            i.item.useTime = 16;
            i.item.damage = 0;
            i.item.rare = rarity;
            i.item.maxStack = 999;
            i.item.noUseGraphic = true;

            i.item.noMelee = false;
            i.item.useStyle = ItemUseStyleID.SwingThrow;

            i.item.bait = b;
            i.item.makeNPC = (short)npcType;
            i.item.autoReuse = true;
            i.item.consumable = true;
        }

        public static void SetMaterial(ModItem i, int w, int h, int rarity = 0, int maxStack = 999, bool consumable = false, int value = 0)
        {
            i.item.width = w;
            i.item.height = h;
            i.item.useAnimation = 16;
            i.item.useTime = 16;
            i.item.damage = 0;
            i.item.rare = rarity;
            i.item.maxStack = maxStack;
            i.item.value = value;
            i.item.noMelee = false;
            i.item.useStyle = ItemUseStyleID.SwingThrow;
            i.item.autoReuse = true;
            i.item.consumable = consumable;
        }

        public static void SetFishingRod(ModItem i, int w, int h, int fish, int shoot, float shootSpeed, int rarity = 0)
        {
            i.item.width = w;
            i.item.height = h;
            i.item.useAnimation = 16;
            i.item.useTime = 16;
            i.item.damage = 0;
            i.item.knockBack = 0f;
            i.item.rare = rarity;
            i.item.maxStack = 1;
            i.item.shootSpeed = shootSpeed;
            i.item.shoot = shoot;

            i.item.noMelee = true;
            i.item.fishingPole = fish;
            i.item.autoReuse = true;
            i.item.consumable = false;
        }

        public static void SetLightPet(ModItem i, int w, int h, int rarity = 0)
        {
            i.item.CloneDefaults(ItemID.ShadowOrb);
            i.item.width = w;
            i.item.height = h;
            i.item.rare = rarity;
        }

        /// <summary>Adds a recipe to the mod given the following items and tiles.</summary>
        /// <param name="item">Item to use as a result.</param>
        /// <param name="mod">Mod to use to add to.</param>
        /// <param name="tile">Tile to use for crafting. -1 means no tile is used. Defaults to -1.</param>
        /// <param name="resultStack">Stack created with this crafting recipe. Defaults to 1.</param>
        /// <param name="ingredients">Ingredients to use. Formatted as a (int, int) pair.</param>
        public static void AddRecipe(ModItem item, Mod mod, int tile = -1, int resultStack = 1, params (int, int)[] ingredients)
        {
            if (ingredients.Length <= 0)
                throw new ArgumentException("Ingredents array is empty.", "ingredients");

            ModRecipe r = new ModRecipe(mod);
            for (int i = 0; i < ingredients.Length; ++i)
                r.AddIngredient(ingredients[i].Item1, ingredients[i].Item2);
            if (tile != -1) r.AddTile(tile);
            r.SetResult(item, resultStack);
            r.AddRecipe();
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

            ModRecipe r = new ModRecipe(mod);
            for (int i = 0; i < ingredients.Length; ++i)
                r.AddIngredient(ingredients[i].Item1, ingredients[i].Item2);
            if (tile != -1) r.AddTile(tile);
            r.SetResult(id, resultStack);
            r.AddRecipe();
        }

        public static bool CanCritterSpawnCheck() => !Framing.GetTileSafely(Main.MouseWorld).active() || !Main.tileSolid[Framing.GetTileSafely(Main.MouseWorld).type];
    }
}
