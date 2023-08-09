using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.Items.Verdant.Materials;
using Verdant.Systems;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Items.Verdant.Armour;

[AutoloadEquip(EquipType.Legs)]
public class VerdantLeggings : ModItem
{
    public override bool IsLoadingEnabled(Mod mod)
    {
        VerdantPlayer.FloorVisualEvent += FloorVisuals;
        return true;
    }

    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 12;
        Item.value = 10000;
        Item.rare = ItemRarityID.Green;
        Item.defense = 4;
        Item.lifeRegen = 2;
    }

    public override void UpdateEquip(Player player) => player.maxMinions++;

    public override void AddRecipes()
    {
        Recipe m = CreateRecipe();
        m.AddIngredient(ModContent.ItemType<VerdantStrongVineMaterial>(), 4);
        m.AddIngredient(ModContent.ItemType<RedPetal>(), 4);
        m.AddIngredient(ModContent.ItemType<PinkPetal>(), 4);
        m.AddIngredient(ModContent.ItemType<LushLeaf>(), 16);
        m.AddIngredient(ModContent.ItemType<YellowBulb>(), 2);
        m.AddTile(TileID.Anvils);
        m.Register();
    }

    public void FloorVisuals(Player p, int type) //grows plants beneath the player
    {
        if (p.ArmourEquipped(Item) && System.Math.Abs(p.velocity.X) > 0.01f)
        {
            int x = (int)(p.Center.X / 16f);
            int y = (int)((p.Center.Y + 8) / 16f);
            Tile tile = Main.tile[x, y];
            int[] types = new int[] { TileID.Grass, TileID.JungleGrass, ModContent.TileType<VerdantGrassLeaves>() };

            if (!tile.HasTile || !types.Contains(tile.TileType))
                return;

            RandomUpdating.Auto(x, y, false, 3, (i, j) =>
            {
                Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.TerraBlade, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
            });
        }
    }
}