using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Tiles.TileEntities.Puff;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Blocks.TileEntity;

[Sacrifice(5)]
public class PickipuffItem : ModItem
{
    public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 28, ItemRarityID.Orange, 99, true);
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<PuffMaterial>(), 12), (ModContent.ItemType<LushLeaf>(), 8));

    public override bool CanUseItem(Player player)
    {
        Tile atTarget = Main.tile[Player.tileTargetX, Player.tileTargetY];
        return player.whoAmI == Main.myPlayer && player.InInteractionRange(Player.tileTargetX, Player.tileTargetY, TileReachCheckSettings.Simple) 
            && atTarget.HasTile && VerdantGrassLeaves.VerdantGrassTypes.Contains(atTarget.TileType);
    }

    public override bool? UseItem(Player player)
    {
        var j = Main.MouseWorld.ToTileCoordinates16();

        if (Terraria.DataStructures.TileEntity.ByPosition.ContainsKey(j))
            return false;
        
        ModContent.GetInstance<Pickipuff>().Place(j.X, j.Y);
        return true;
    }
}

internal class HarvestedPickipuffItem : PickipuffItem
{
    public override string Texture => "Verdant/Items/Verdant/Blocks/TileEntity/PickipuffItem";

    public override bool? UseItem(Player player)
    {
        var j = Main.MouseWorld.ToTileCoordinates16();

        if (Terraria.DataStructures.TileEntity.ByPosition.ContainsKey(j))
            return false;

        int id = ModContent.GetInstance<Pickipuff>().Place(j.X, j.Y);
        (Terraria.DataStructures.TileEntity.ByID[id] as Pickipuff).SetToHarvested();
        return true;
    }

    public override void AddRecipes() { } //No recipes for you!
}
