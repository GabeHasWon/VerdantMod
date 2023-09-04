using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Misc;

namespace Verdant.Tiles.Verdant.Misc
{
    internal class ResearchBooks : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.StyleOnTable1x1);
            TileObjectData.newTile.RandomStyleRange = 16; 
            TileObjectData.newTile.StyleHorizontal = true;
            QuickTile.SetMulti(this, 1, 1, DustID.UnusedBrown, SoundID.Dig, true, new Color(85, 82, 67));

            RegisterItemDrop(ModContent.ItemType<ResearchBooksItem>());
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;
        public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) => effects = (i % 2 == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    }
}
