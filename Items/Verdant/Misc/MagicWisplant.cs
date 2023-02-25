using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Materials;
using Verdant.Projectiles.Misc;

namespace Verdant.Items.Verdant.Misc;

[Sacrifice(3)]
internal class MagicWisplant : ModItem
{
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Magic-Infused Wisplant", "'A powerful aura of druidic energy surrounds it'\nSpeeds up all natural growth in an area by a dramatic amount");

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.ThrowingKnife);
        Item.damage = 0;
        Item.useTime = 60;
        Item.useAnimation = 60;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<MagicWisplantProj>();
        Item.shootSpeed = 6f;
        Item.rare = ItemRarityID.Green;
    }

    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.Bottles, 1, (ModContent.ItemType<LushLeaf>(), 4), (ModContent.ItemType<WisplantItem>(), 1), (ItemID.FallenStar, 1));
}
