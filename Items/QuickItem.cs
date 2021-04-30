using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items
{
    public class QuickItem
    {
        public static void SetStatic(ModItem i, string name, string tooltip = "")
        {
            i.DisplayName.SetDefault(name);
            i.Tooltip.SetDefault(tooltip);
        }

        public static void SetBlock(ModItem i, int w, int h, int tile, bool consumable = true)
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
        }

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

            Item.staff[i.item.type] = true;
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

        public static void SetMaterial(ModItem i, int w, int h, int rarity = 0, int maxStack = 999, bool consumable = false)
        {
            i.item.width = w;
            i.item.height = h;
            i.item.useAnimation = 16;
            i.item.useTime = 16;
            i.item.damage = 0;
            i.item.rare = rarity;
            i.item.maxStack = maxStack;

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

        public static void AddRecipe(ModItem item, Mod mod, int tile = -1, int resultStack = 1, params (int, int)[] ingredients)
        {
            ModRecipe r = new ModRecipe(mod);
            for (int i = 0; i < ingredients.Length; ++i)
                r.AddIngredient(ingredients[i].Item1, ingredients[i].Item2);
            if (tile != -1) r.AddTile(tile);
            r.SetResult(item, resultStack);
            r.AddRecipe();
        }
    }
}
