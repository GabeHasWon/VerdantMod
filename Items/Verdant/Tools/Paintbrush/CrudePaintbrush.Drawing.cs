using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Verdant.Items.Verdant.Tools.Paintbrush;

public partial class CrudePaintbrush : ModItem
{
    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        if (_storedItemID == -1)
            return;

        Item item = ContentSamples.ItemsByType[_storedItemID];
        Main.DrawItemIcon(spriteBatch, item, position + new Vector2(36 * scale), Color.White, 24f * scale);

        string count = (GetTileAmmo(Main.LocalPlayer) + 1).ToString();
        var pos = position + (new Vector2(-6, 36) * scale);
        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, count, pos, Color.White, 0f, Vector2.Zero, new Vector2(1f) * scale);
    }

    public void GetLayerDrawing(PlayerDrawSet info)
    {
        if (mode == PlacementMode.Line)
        {
            if (_locations.Count == 1)
            {
                const int Offset = 1600;

                var mouse = Main.MouseWorld.ToTileCoordinates().ToVector2();
                int repeats = (int)Vector2.Distance(_locations.First().ToVector2(), mouse);
                int count = Main.LocalPlayer.CountItem(GetTileWand());
                int firstTileX = (int)((Main.screenPosition.X - Offset) / 16f - 1f);
                int lastTileX = (int)((Main.screenPosition.X + Main.screenWidth + Offset) / 16f) + 2;
                int firstTileY = (int)((Main.screenPosition.Y - Offset) / 16f - 1f);
                int lastTileY = (int)((Main.screenPosition.Y + Main.screenHeight + Offset) / 16f) + 5;
                Point last = new();

                for (int i = 0; i <= repeats; ++i)
                {
                    Color c = i > count - 1 ? Color.Red : Color.White;

                    Point placePos = Vector2.Lerp(_locations.First().ToVector2(), mouse, repeats == 0 ? 0 : i / (float)repeats).ToPoint();

                    if (placePos == last)
                        continue;

                    Texture2D tex = TextureAssets.Extra[2].Value;

                    if (placePos.X < lastTileX && placePos.X > firstTileX && placePos.Y < lastTileY && placePos.Y > firstTileY)
                        info.DrawDataCache.Add(new DrawData(tex, placePos.ToWorldCoordinates(0, 0) - Main.screenPosition, new Rectangle(0, 0, 16, 16), c * 0.5f));

                    last = placePos;
                }
            }
        }
    }
}
