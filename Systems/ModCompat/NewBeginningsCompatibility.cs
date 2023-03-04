using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Armour;
using Verdant.Items.Verdant.Equipables;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Weapons;

namespace Verdant.Systems.ModCompat;

internal class NewBeginningsCompatibility
{
    internal static void AddOrigin()
    {
        if (ModLoader.TryGetMod("NewBeginnings", out Mod beginnings))
        {
            beginnings.Call("AddOrigin", ModContent.Request<Texture2D>("Verdant/Systems/ModCompat/Textures/Botanist"), "Botanist", "Botanist",
                "The druid of the science world; understands what makes life life, what grows and dies, and most importantly, what fungi is the tastiest.",
                "Starts with 500 lush shivs, 10 wisplants, 10 pink puff, 80 max mana, and an expert guide to plant fiber cordage",
                new (int, int)[] { (ModContent.ItemType<LushDagger>(), 500), (ModContent.ItemType<WisplantItem>(), 10), (ModContent.ItemType<PuffMaterial>(), 10) },
                ItemID.ArchaeologistsHat, ItemID.None, ItemID.None, new int[] { }, 100, 80);

            object equip = beginnings.Call("EquipData", ModContent.ItemType<LushWoodHead>(), ModContent.ItemType<LushWoodBody>(), ModContent.ItemType<LushWoodLegs>(),
                new int[] { ModContent.ItemType<HealingFlowerItem>(), ModContent.ItemType<Lightbloom>() });
            object misc = beginnings.Call("MiscData", 60, 20, -1, ModContent.ItemType<LushWoodSword>());
            object result = beginnings.Call("ShortAddOrigin", ModContent.Request<Texture2D>("Verdant/Systems/ModCompat/Textures/MinorSummoner"), "MinorSummoner", "Lushman",
                "Slightly in tune with druidic powers, but the signal's a bit weak so just a little bit.", 
                "Starts with a full Lush Wood set, a Yellow Sprout, a Lightbloom and lowered max health.", Array.Empty<(int, int)>(), equip, misc);
        }
    }
}
