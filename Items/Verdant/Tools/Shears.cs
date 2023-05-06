using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using System.Linq;
using Verdant.Systems.Foreground.Tiled;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Tiles.Verdant.Decor.Bushes;
using Verdant.Tiles.TileEntities.Puff;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(1)]
class Shears : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Shears", "Trims certain plants\nCan be used to remove Pickipuff, Mysteria Drapes" +
        "\nCan be used as an axe, technically");

    public override void SetDefaults()
    {
        QuickItem.SetMaterial(this, 32, 32, ItemRarityID.Green, 1, false, Item.buyPrice(0, 0, 5, 0));

        Item.noMelee = false;
        Item.DamageType = DamageClass.Melee;
        Item.damage = 4;
        Item.axe = 2;
        Item.useTurn = true;
        Item.useTime = 8;
        Item.useAnimation = 8;
    }

    public override bool? UseItem(Player player)
    {
        var pos = Main.MouseWorld.ToTileCoordinates();

        if (ForegroundManager.Items.Any(x => x is MysteriaDrapes drape && drape.position.ToTileCoordinates() == pos))
        {
            (ForegroundManager.Items.First(x => x is MysteriaDrapes drape && drape.position.ToTileCoordinates() == pos) as MysteriaDrapes).Kill();
            return true;
        }
        else if (TileEntity.ByPosition.ContainsKey(new Point16(pos.X, pos.Y)) && TileEntity.ByPosition[new Point16(pos.X, pos.Y)] is Pickipuff pickipuff)
        {
            pickipuff.Kill(pos.X, pos.Y);
            return true;
        }

        CheckBush(pos);
        return true;
    }

    private static bool CheckBush(Point pos)
    {
        Tile tile = Main.tile[pos];

        if (tile.HasTile && tile.TileType >= TileID.Count && ModContent.GetModTile(tile.TileType) is IBush bush && bush.CanBeTrimmed(pos.X, pos.Y))
        {
            bush.ChooseTrim(pos.X, pos.Y);
            return true;
        }
        return false;
    }

    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ItemID.IronBar, 4), (ModContent.ItemType<MysteriaWood>(), 3));
}
