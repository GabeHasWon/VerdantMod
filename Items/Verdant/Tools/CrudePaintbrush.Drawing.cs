using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Tools;

public partial class CrudePaintbrush : ModItem
{
    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        if (_storedItemID != -1)
            Main.DrawItemIcon(spriteBatch, ContentSamples.ItemsByType[_storedItemID], position + new Vector2(36 * scale), Color.White, 24f * scale);
    }
}
