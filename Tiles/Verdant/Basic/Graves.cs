using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
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

        [DialogueCacheKey(nameof(GraveDialogueCache) + ".Ma")]
        public static ScreenText IntroDialogue(bool forServer)
        {
            if (forServer)
                return null;

            if (!UseCustomSystem)
            {
                for (int i = 0; i < 6; ++i)
                    Chat(Key(i), i > 2);

                return null;
            }

            return new ScreenText(Language.GetText(""))
            {
                shader = ModContent.Request<Effect>(EffectIDs.TextWobble),
                color = Color.White * 0.7f,
                shaderParams = new ScreenTextEffectParameters(0.02f, 0.01f, 30)
            };
        }
    }

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.Height = 3;
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);
        TileObjectData.newTile.CoordinateHeights = [16, 16, 18];
        TileObjectData.newTile.StyleHorizontal = true;

        QuickTile.SetMulti(this, 2, 3, DustID.Stone, SoundID.Dig, true, new Color(112, 103, 111), false, false, false, "Grave");
    }

    public override bool IsTileSpelunkable(int i, int j) => true;

    public override bool RightClick(int i, int j)
    {
        return true;
    }

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;

        if (TearRainSystem.RainingAt(j))
            return;

        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = ModContent.ItemType<PassionflowerBulb>();
    }
}