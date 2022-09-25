using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Verdant.Foreground;

public static class ForegroundManager
{
    public static readonly List<ForegroundItem> Items = new List<ForegroundItem>();
    public static readonly List<int> SpecialDrawIndices = new();

    internal static void Hooks()
    {
        On.Terraria.Main.DrawGore += DrawForeground;
        On.Terraria.Main.DrawNPCs += Main_DrawNPCs;
        Main.OnTickForThirdPartySoftwareOnly += UpdateHook;
    }

    private static void Main_DrawNPCs(On.Terraria.Main.orig_DrawNPCs orig, Main self, bool behindTiles)
    {
        foreach (var val in SpecialDrawIndices)
            if (val >= 0 && val < Items.Count)
            Items[val].Draw();

        orig(self, behindTiles);
    }

    private static void UpdateHook()
    {
        if (Main.PlayerLoaded && !Main.gameMenu)
            Update();
    }

    private static void DrawForeground(On.Terraria.Main.orig_DrawGore orig, Main self)
    {
        orig(self);

        if (Main.PlayerLoaded && !Main.gameMenu)
            Draw();

        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        for (int i = 0; i < Main.maxProjectiles; ++i)
        {
            Projectile p = Main.projectile[i];
            if (p.active && p.ModProjectile is Drawing.IDrawAdditive additive)
                additive.DrawAdditive(Drawing.AdditiveLayer.AfterPlayer);
        }

        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
    }

    public static void Draw()
    {
        Rectangle screen = new((int)Main.screenPosition.X - Main.screenWidth, (int)Main.screenPosition.Y - Main.screenHeight, Main.screenWidth  * 3, Main.screenHeight * 3);

        foreach (var val in Items)
        {
            if (!SpecialDrawIndices.Contains(Items.IndexOf(val)))
                val.Draw();
        }
    }

    public static void Update()
    {
        List<ForegroundItem> removals = new List<ForegroundItem>();

        foreach (var val in Items)
        {
            if (Main.hasFocus && !Main.gamePaused)
                val.Update();

            if (val.killMe)
            {
                removals.Add(val);

                if (SpecialDrawIndices.Contains(Items.IndexOf(val)))
                    SpecialDrawIndices.Remove(Items.IndexOf(val));
            }
        }

        foreach (var item in removals)
            Items.Remove(item);
    }

    //public static void Load(TagCompound info)
    //{
    //}

    public static void Unload() => Items.Clear();

    public static int AddItem(ForegroundItem item, bool forced = false, bool playerLayer = false)
    {
        if (!ModContent.GetInstance<VerdantClientConfig>().BackgroundObjects && !forced) //Skip if option is turned off
            return -1;

        Items.Add(item);
        int index = Items.IndexOf(item);

        if (playerLayer)
            SpecialDrawIndices.Add(index);
        return index;
    }

    public static ForegroundItem AddItemDirect(ForegroundItem item, bool forced = false, bool playerLayer = false) => Items[AddItem(item, forced, playerLayer)];

    /// <summary>Shorthand for ModContent.ModContent.Request<Texture2D>("Verdant/Foreground/Textures/" + name).</summary>
    /// <param name="name">Name of the requested texture.</param>
    public static Texture2D GetTexture(string name) => VerdantMod.Instance.Assets.Request<Texture2D>("Foreground/Textures/" + name).Value;

    internal static TagCompound Save()
    {
        TagCompound compound = new TagCompound();
        foreach (var item in Items)
        {
            if (item.SaveMe)
            {
                var value = item.Save();
                if (value == null)
                    continue;

                compound.Add("fgInfo", value);
            }
        }
        return compound;
    }
}
