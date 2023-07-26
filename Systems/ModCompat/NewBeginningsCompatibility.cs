using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Verdant.Items.Verdant.Armour;
using Verdant.Items.Verdant.Equipables;
using Verdant.Items.Verdant.Materials;
using Verdant.Items.Verdant.Weapons;
using Verdant.World;

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
                ItemID.ArchaeologistsHat, ItemID.None, ItemID.None, Array.Empty<int>(), 100, 80);

            object equip = beginnings.Call("EquipData", ModContent.ItemType<LushWoodHead>(), ModContent.ItemType<LushWoodBody>(), ModContent.ItemType<LushWoodLegs>(),
                new int[] { ModContent.ItemType<HealingFlowerItem>(), ModContent.ItemType<Lightbloom>() });
            object misc = beginnings.Call("MiscData", 60, 20, -1, ModContent.ItemType<LushWoodSword>());
            object dele = beginnings.Call("DelegateData", () => true, (List<GenPass> list) => { }, () => true, () => ModContent.GetInstance<VerdantGenSystem>().apotheosisLocation.Value);
            object result = beginnings.Call("ShortAddOrigin", ModContent.Request<Texture2D>("Verdant/Systems/ModCompat/Textures/MinorSummoner"), "MinorSummoner", "Lushman",
                "A child of the Apotheosis, in word only. A botanist in a past life.", 
                "Starts with a full Lush Wood set, a Yellow Sprout, a Lightbloom and lowered max health.", Array.Empty<(int, int)>(), equip, misc, dele);
        }
    }
}
