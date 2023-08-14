using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Tiles;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(20)]
class Halfsprout : ApotheoticItem
{
    public override void SetDefaults()
    {
        Item.Size = new Vector2(40, 56);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.rare = ItemRarityID.Green;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.autoReuse = true;
        Item.consumable = true;
        Item.maxStack = 99;
    }

    public override bool? UseItem(Player player)
    {
        Point mouse = Main.MouseWorld.ToTileCoordinates();
        (int x, int y) = (mouse.X, mouse.Y);

        Tile tile = Main.tile[x, y];

        if (tile.HasTile && IsTileAnHerb(tile.TileType, out bool vanilla))
        {
            if (vanilla)
            {
                short frameX = tile.TileFrameX;

                tile.TileType = TileID.MatureHerbs;
                TileHelper.SyncedKill(x, y);
                tile.HasTile = true;
                tile.TileType = TileID.ImmatureHerbs;
                tile.TileFrameX = frameX;
                tile.TileFrameY = 0;

                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y, 0f, 0, 0, 0);
            }
            else
            {
                short frameX = tile.TileFrameX;
                ushort tileType = tile.TileType;

                tile.TileFrameX = 18 * 2;

                TileHelper.SyncedKill(x, y);
                tile.HasTile = true;
                tile.TileType = tileType;
                tile.TileFrameX = frameX;
                tile.TileFrameY = 0;

                if (Main.netMode != NetmodeID.SinglePlayer)
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y, 0f, 0, 0, 0);
            }
            return true;
        }
        return false;
    }

    private static bool IsTileAnHerb(int type, out bool vanillaHerb)
    {
        bool isModdedHerb = type == ModContent.TileType<Wisplant>();
        bool isVanillaHerb = type == TileID.BloomingHerbs || type == TileID.ImmatureHerbs || type == TileID.MatureHerbs;
        vanillaHerb = isVanillaHerb;
        return isVanillaHerb || isModdedHerb;
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(Halfsprout))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Halfsprout", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Halfsprout", true);
    }
}
