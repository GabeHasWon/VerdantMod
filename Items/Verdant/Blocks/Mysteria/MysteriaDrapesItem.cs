using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Tiled;

namespace Verdant.Items.Verdant.Blocks.Mysteria;

public class MysteriaDrapesItem : ModItem
{
    public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Mysteria Drapes");
		Tooltip.SetDefault("Places on any solid tile\nGrows like a vine, slowly\nPlace more on an existing anchor to grow the drape\nIs always in front of the player");
	}

	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.useTime = 16;
		Item.useAnimation = 16;
		Item.maxStack = 999;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.knockBack = 6;
		Item.value = Item.buyPrice(0, 0, 0, 50);
		Item.rare = ItemRarityID.Green;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
        Item.consumable = true;
    }

    public override bool? UseItem(Player player)
    {
        var pos = Main.MouseWorld.ToTileCoordinates();

        if (WorldGen.SolidOrSlopedTile(pos.X, pos.Y))
        {
            bool exists = ForegroundManager.Items.Any(x => x is MysteriaDrapes drape && drape.position.ToTileCoordinates() == pos);

            if (!exists)
                ForegroundManager.AddItem(new MysteriaDrapes(pos), true);
            else
            {
                var drape = ForegroundManager.Items.First(x => x is MysteriaDrapes drape && drape.position.ToTileCoordinates() == pos) as MysteriaDrapes;
                drape.Grow();
            }
            return true;
        }
        return false;
    }
}