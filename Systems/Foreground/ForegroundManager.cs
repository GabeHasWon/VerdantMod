using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Verdant.Systems.Foreground;

public static class ForegroundManager
{
    public static readonly List<ForegroundItem> Items = new List<ForegroundItem>();
    public static readonly List<ForegroundItem> PlayerLayerItems = new();

    internal static void Hooks()
    {
        On_Main.DrawProjectiles += PlayerLayerHook;
        On_Main.DoUpdate += On_Main_DoUpdate;
    }

    private static void On_Main_DoUpdate(On_Main.orig_DoUpdate orig, Main self, ref GameTime gameTime)
    {
        orig(self, ref gameTime);

        if (Main.PlayerLoaded && !Main.gameMenu)
            Update();
    }

    private static void PlayerLayerHook(On_Main.orig_DrawProjectiles orig, Main self)
    {
        orig(self);

        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

        foreach (var val in PlayerLayerItems)
            val.Draw();

        Main.spriteBatch.End();
    }

    public static void Draw()
    {
        Rectangle screen = new((int)Main.screenPosition.X - Main.screenWidth, (int)Main.screenPosition.Y - Main.screenHeight, Main.screenWidth * 3, Main.screenHeight * 3);

        foreach (var val in Items)
        {
            if (screen.Contains(new Rectangle((int)val.position.X, (int)val.position.Y, val.Texture.Width(), val.Texture.Height())))
                val.Draw();
        }
    }

    public static void Update()
    {
        UpdateSet(PlayerLayerItems);
        UpdateSet(Items);
    }

    private static void UpdateSet(List<ForegroundItem> set)
    {
        List<ForegroundItem> removals = new();

        foreach (var val in set)
        {
            if (!Main.gamePaused)
                val.Update();

            if (val.killMe)
                removals.Add(val);
        }

        foreach (var item in removals)
            set.Remove(item);
    }

    public static void Unload()
    {
        Items.Clear();
        PlayerLayerItems.Clear();
    }

    public static int AddItem(ForegroundItem item, bool forced = false, bool playerLayer = false)
    {
        if (!ModContent.GetInstance<VerdantClientConfig>().BackgroundObjects && !forced) //Skip if option is turned off
            return -1;

        if (playerLayer)
        {
            PlayerLayerItems.Add(item);
            return PlayerLayerItems.IndexOf(item);
        }
        else
        {
            Items.Add(item);
            return Items.IndexOf(item);
        }
    }

    public static ForegroundItem AddItemDirect(ForegroundItem item, bool forced = false, bool playerLayer = false)
    {
        if (!playerLayer)
            return Items[AddItem(item, forced, playerLayer)];
        else
            return PlayerLayerItems[AddItem(item, forced, playerLayer)];
    }

    /// <summary>Shorthand for ModContent.ModContent.Request<Texture2D>("Verdant/Foreground/Textures/" + name).</summary>
    /// <param name="name">Name of the requested texture.</param>
    public static Texture2D GetTexture(string name) => VerdantMod.Instance.Assets.Request<Texture2D>("Foreground/Textures/" + name).Value;

    internal static void Save(TagCompound compound)
    {
        TagCompound compounds = new()
        {
            { "count", Items.Where(x => x.SaveMe).Count() }
        };

        int index = 0;
        foreach (var item in Items)
        {
            if (item.SaveMe)
            {
                TagCompound itemTag = new()
                {
                    { "type", item.GetType().AssemblyQualifiedName }
                };

                item.Save(itemTag);
                compounds.Add("item" + index, itemTag);
                index++;
            }
        }

        TagCompound playerLayerCompounds = new()
        {
            { "count", PlayerLayerItems.Where(x => x.SaveMe).Count() }
        };

        index = 0;
        foreach (var item in PlayerLayerItems)
        {
            if (item.SaveMe)
            {
                TagCompound itemTag = new()
                {
                    { "type", item.GetType().AssemblyQualifiedName }
                };

                item.Save(itemTag);
                playerLayerCompounds.Add("item" + index, itemTag);
                index++;
            }
        }

        compound.Add("Verdant:Foreground", compounds);
        compound.Add("Verdant:PlayerLayerForeground", playerLayerCompounds);
    }

    internal static void Load(TagCompound compound)
    {
        if (compound.ContainsKey("Verdant:Foreground"))
        {
            if (compound["Verdant:Foreground"] is not TagCompound tag)
                return;

            int count = tag.GetInt("count");

            for (int i = 0; i < count; i++)
            {
                if (!tag.ContainsKey("item" + i))
                    continue;

                var item = tag.GetCompound("item" + i);
                string type = item.GetString("type");
                ForegroundItem fgItem = Activator.CreateInstance(Type.GetType(type)) as ForegroundItem;
                fgItem.Load(item);
                Items.Add(fgItem);
            }
        }

        if (compound.ContainsKey("Verdant:PlayerLayerForeground"))
        {
            if (compound["Verdant:PlayerLayerForeground"] is not TagCompound playerLayerTag)
                return;

            int count = playerLayerTag.GetInt("count");

            for (int i = 0; i < count; i++)
            {
                if (!playerLayerTag.ContainsKey("item" + i))
                    continue;

                var item = playerLayerTag.GetCompound("item" + i);
                string type = item.GetString("type");
                ForegroundItem fgItem = Activator.CreateInstance(Type.GetType(type)) as ForegroundItem;
                fgItem.Load(item);
                PlayerLayerItems.Add(fgItem);
            }
        }
    }
}

public class ForegroundWorld : ModSystem
{
    public override void SaveWorldData(TagCompound tag) => ForegroundManager.Save(tag);
    public override void LoadWorldData(TagCompound tag) => ForegroundManager.Load(tag);
}
