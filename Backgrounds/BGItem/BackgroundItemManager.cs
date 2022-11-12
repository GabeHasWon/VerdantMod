using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Verdant.Backgrounds.BGItem;

public class BackgroundItemManager
{
    /// <summary>List of items to draw and update.</summary>
    private static List<BaseBGItem> bgItems;
    /// <summary>Organized list of items for layering.</summary>
    private static IOrderedEnumerable<BaseBGItem> organizedItems;

    /// <summary>True when this has been loaded and is usable.</summary>
    public static bool Loaded { get; private set; }

    public static void AddItem(BaseBGItem item, bool forceSpawn = false)
    {
        if (bgItems is null)
            bgItems = new List<BaseBGItem>();

        if ((!ModContent.GetInstance<VerdantClientConfig>().BackgroundObjects && !forceSpawn) || Main.netMode == NetmodeID.Server) //Skip if option is turned off & it's not forced
            return;

        if (item != null)
            bgItems.Add(item);
        else
        {
            VerdantMod mod = VerdantMod.Instance;
            mod.Logger.Warn("Null BGItem add attempt. Report to mod devs.");
        }
    }

    /// <summary>Draws all background items.</summary>
    public static void Draw()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        int range = Main.offScreenRange * 3;
        Rectangle screen = new Rectangle((int)Main.screenPosition.X - range, (int)Main.screenPosition.Y - range, Main.screenWidth + (range * 2), Main.screenHeight + (range * 2));

        if (bgItems is null)
            bgItems = new List<BaseBGItem>();

        organizedItems = bgItems.OrderBy(x => x.parallax); //For proper depth

        foreach (var item in organizedItems) //Draw everything
        {
            if (item is null)
                continue;

            Vector2 off = Lighting.LegacyEngine.Mode > 1 ? Vector2.Zero : Vector2.One;
            if (screen.Contains(item.DrawPosition.ToPoint()) && item.position.Y / 16f < Main.worldSurface)
                item.Draw(off);
        }
    }

    public static void Update()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        if (bgItems is null)
            bgItems = new List<BaseBGItem>();

        if (!Main.hasFocus || Main.gamePaused || organizedItems is null)
            return;

        foreach (var item in organizedItems)
            item?.Behaviour();

        bgItems?.RemoveAll(x => x is null || x.killMe);
    }

    public static List<TagCompound> Save()
    {
        if (!Loaded) 
            return null; //Somehow it's trying to save while not being loaded, abort 

        List<TagCompound> tags = new List<TagCompound>(); //Save the stuff
        foreach (var item in bgItems)
        {
            if (item is not null && item.SaveMe && !item.killMe)
            {
                TagCompound value = item.Save();
                if (value != null)
                {
                    value.Add("Name", item.GetType().FullName); //So I can get the type later
                    tags.Add(value);
                }
            }
        }
        return tags;
    }

    public static void Unload()
    {
        bgItems = new(); //Clear it and unload
        organizedItems = bgItems.OrderBy(x => x.parallax);
        Loaded = false;
    }

    public static void Load(IList<TagCompound> info)
    {
        bgItems = new List<BaseBGItem>(); //Load
        organizedItems = bgItems.OrderBy(x => x.scale);
        Loaded = true;

        foreach (var item in info)
        {
            string name = item.GetString("Name");
            try
            {
                Type bgItemType = typeof(BackgroundItemManager).Assembly.GetType(name, true);
                BaseBGItem bgItem = Activator.CreateInstance(bgItemType) as BaseBGItem;
                bgItem.Load(item);
                bgItems.Add(bgItem);
            }
            catch (Exception e)
            {
                VerdantMod mod = VerdantMod.Instance;
                mod.Logger.Warn("Failed to load BGItem assembly type.\n", e);
            }
        }
    }
}
