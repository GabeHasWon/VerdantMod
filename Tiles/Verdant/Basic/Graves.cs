using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Effects;
using Verdant.Items;
using Verdant.Items.Verdant;
using Verdant.Items.Verdant.Blocks.PestControl;
using Verdant.Items.Verdant.Misc;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.TearRain;

namespace Verdant.Tiles.Verdant.Basic;

public class Graves : ModTile
{
    internal class GraveDialogueCache : IDialogueCache
    {
        private static bool UseCustomSystem => ModContent.GetInstance<VerdantClientConfig>().CustomDialogue;

        public static LocalizedText GetText(string postfix) => Language.GetText("Mods.Verdant.GraveDialogue." + postfix);

        [DialogueCacheKey(nameof(GraveDialogueCache) + ".Ma")]
        public static ScreenText MaDialogue(bool forServer)
        {
            if (forServer)
                return null;

            Color color = new Color(182, 54, 87) * 0.45f;
            int index = Main.rand.Next(3);

            if (!UseCustomSystem)
            {
                Chat(GetText("Ma." + index).Value, GetText("Ma.Name").Value, color);
                return null;
            }

            return new ScreenText(GetText("Ma." + index))
            {
                speaker = GetText("Ma.Name").Value,
                speakerColor = color,
                shader = EffectIDs.TextWobbleEffect,
                color = Color.White * 0.8f * (index == 2 ? 0.6f : 1),
                shaderParams = new ScreenTextEffectParameters(0.02f, 0.01f, 30)
            };
        }

        [DialogueCacheKey(nameof(GraveDialogueCache) + ".Pa")]
        public static ScreenText PaDialogue(bool forServer)
        {
            if (forServer)
                return null;

            Color color = new Color(84, 36, 176) * 0.45f;

            if (!UseCustomSystem)
            {
                Chat(GetText("Pa." + Main.rand.Next(3)).Value, GetText("Pa.Name").Value, color);
                return null;
            }

            return new ScreenText(GetText("Pa." + Main.rand.Next(3)))
            {
                speaker = GetText("Pa.Name").Value,
                speakerColor = color,
                shader = EffectIDs.TextWobbleEffect,
                color = Color.White * 0.8f,
                shaderParams = new ScreenTextEffectParameters(0.02f, 0.01f, 30)
            };
        }

        [DialogueCacheKey(nameof(GraveDialogueCache) + ".Dreams")]
        public static ScreenText DreamsDialogue(bool forServer)
        {
            if (forServer)
                return null;

            Color color = new(234, 68, 123);

            if (!UseCustomSystem)
            {
                Chat(GetText("Dreams." + Main.rand.Next(3)).Value, GetText("Dreams.Name").Value, color);
                return null;
            }

            return new ScreenText(GetText("Dreams." + Main.rand.Next(3)))
            {
                speaker = GetText("Dreams.Name").Value,
                speakerColor = color,
                shader = EffectIDs.TextWobbleEffect,
                color = Color.White * 0.8f,
                shaderParams = new ScreenTextEffectParameters(0.02f, 0.01f, 30)
            };
        }

        [DialogueCacheKey(nameof(GraveDialogueCache) + ".Me")]
        public static ScreenText MeDialogue(bool forServer)
        {
            if (forServer)
                return null;

            Color color = new Color(225, 225, 255) * 0.65f;

            if (!UseCustomSystem)
            {
                Chat(GetText("Me." + Main.rand.Next(9)).Value, GetText("Me.Name").Value, color);
                return null;
            }

            return new ScreenText(GetText("Me." + Main.rand.Next(9)))
            {
                speaker = GetText("Me.Name").Value,
                speakerColor = color,
                shader = EffectIDs.TextWobbleEffect,
                color = Color.White * 0.8f,
                shaderParams = new ScreenTextEffectParameters(0.02f, 0.01f, 30)
            };
        }

        public static void Chat(string text, string name, Color color) => Main.NewText($"[c/{color.Hex3()}:{name}] {text}");
    }

    public abstract class GraveItem : ApotheoticItem
    {
        protected static ScreenText DefaultDialogue(bool forServer)
        {
            if (forServer)
                return null;

            if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            {
                LocalizedText localizedText = Language.GetText("Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ThornBlock");
                Main.NewText($"[c/32cd32:{Language.GetTextValue("Mods.Verdant.ApotheosisFullName")}:] " + localizedText.Value, Color.White);
                return null;
            }

            return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.ThornBlock");
        }
    }

    [Sacrifice(1)]
    public class MaGrave : GraveItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 26, 32, ModContent.TileType<Graves>(), true, 0, rarity: ItemRarityID.LightPurple);

        [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(MaGrave))]
        public override ScreenText Dialogue(bool forServer) => DefaultDialogue(forServer);
    }

    [Sacrifice(1)]
    public class PaGrave : GraveItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 26, 32, ModContent.TileType<Graves>(), true, 1, rarity: ItemRarityID.LightPurple);

        [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(PaGrave))]
        public override ScreenText Dialogue(bool forServer) => DefaultDialogue(forServer);
    }

    [Sacrifice(1)]
    public class DreamGrave : GraveItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 26, 32, ModContent.TileType<Graves>(), true, 2, rarity: ItemRarityID.LightPurple);

        [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(DreamGrave))]
        public override ScreenText Dialogue(bool forServer) => DefaultDialogue(forServer);
    }

    [Sacrifice(1)]
    public class MeGrave : GraveItem
    {
        public override void SetDefaults() => QuickItem.SetBlock(this, 22, 32, ModContent.TileType<Graves>(), true, 3, rarity: ItemRarityID.LightPurple);

        [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(MeGrave))]
        public override ScreenText Dialogue(bool forServer) => DefaultDialogue(forServer);
    }

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Height = 3;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.CoordinateHeights = [16, 16, 18];
        TileObjectData.newTile.StyleHorizontal = true;

        QuickTile.SetMulti(this, 2, 3, DustID.Stone, SoundID.Dig, true, new Color(112, 103, 111), false, false, false, "Grave", new Point16(0, 2));
    }

    public override bool IsTileSpelunkable(int i, int j) => true;

    public override bool RightClick(int i, int j)
    {
        if (!TearRainSystem.RainingAt(j) || ScreenTextManager.CurrentText is not null)
            return true;

        Tile tile = Main.tile[i, j];
        int type = tile.TileFrameX / 36;
        string sub = type switch
        {
            0 => "Ma",
            1 => "Pa",
            2 => "Dreams",
            3 => "Me",
            _ => throw new Exception("How did you get here? Invalid Grave subtile ID.")
        };
        
        DialogueCacheAutoloader.Play(nameof(GraveDialogueCache) + "." + sub, false);
        return true;
    }

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;

        if (!TearRainSystem.RainingAt(j))
            return;

        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = -1;
        player.cursorItemIconText = Language.GetTextValue("Mods.Verdant.GraveDialogue.Listen");
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        if (!TearRainSystem.RainingAt(j))
            return;

        Vector2 pos = TileHelper.TileCustomPosition(i, j);
        Tile tile = Main.tile[i, j];
        spriteBatch.Draw(TextureAssets.Tile[Type].Value, pos, new Rectangle(tile.TileFrameX, tile.TileFrameY + 56, 16, 16), Color.White);
    }
}