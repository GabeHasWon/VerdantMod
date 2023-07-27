using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        Terraria.On_Main.DrawProjectiles += PlayerLayerHook;
        Main.OnTickForThirdPartySoftwareOnly += UpdateHook;
    }

    private static void PlayerLayerHook(Terraria.On_Main.orig_DrawProjectiles orig, Main self)
    {
        orig(self);

        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

        foreach (var val in PlayerLayerItems)
            val.Draw();

        Main.spriteBatch.End();
    }

    private static void UpdateHook()
    {
        if (Main.PlayerLoaded && !Main.gameMenu)
            Update();
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
            if (Main.hasFocus && !Main.gamePaused)
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
        List<TagCompound> compounds = new();

        foreach (var item in Items)
        {
            if (item.SaveMe)
            {
                TagCompound itemTag = new()
                {
                    { "ForegroundItem:type", item.GetType().FullName }
                };

                item.Save(itemTag);
                compounds.Add(itemTag);
            }
        }

        compound.Add("Verdant:Foreground", compounds);
    }

    internal static void Load(TagCompound compound)
    {
        if (compound.ContainsKey("Verdant:Foreground"))
        {
            var tags = compound.GetList<TagCompound>("Verdant:Foreground");

            foreach (var item in tags)
            {
                string name = item.GetString("ForegroundItem:type");
                var fg = Activator.CreateInstance(Type.GetType(name)) as ForegroundItem;
                fg.Load(item);

                Items.Add(fg);
            }
        }
    }
}

public class ForegroundWorld : ModSystem
{
    public override void SaveWorldData(TagCompound tag) => ForegroundManager.Save(tag);
    public override void LoadWorldData(TagCompound tag) => ForegroundManager.Load(tag);
}
