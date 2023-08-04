using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;
using Verdant.Players.Layers;

namespace Verdant.Items.Verdant.Armour.ApotheoticArmor;

[AutoloadEquip(EquipType.Head)]
public class ApotheoticTreeHelmet : ModItem, ITallHat
{
    static Asset<Texture2D> _hatSheet;
    static Asset<Texture2D> _hatBackSheet;

    public override void SetStaticDefaults()
    {
        _hatSheet = ModContent.Request<Texture2D>(Texture + "Sheet");
        _hatBackSheet = ModContent.Request<Texture2D>(Texture + "SheetBack");
    }

    public override void Unload() => _hatSheet = _hatBackSheet = null;

    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 46;
        Item.value = Item.buyPrice(0, 5, 0, 0);
        Item.rare = ItemRarityID.Purple;
        Item.defense = 10;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<ApotheoticChestplate>() && legs.type == ModContent.ItemType<MysteriaLeggings>();

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Summon) += 0.1f;
        player.maxMinions++;
    }

    public override void UpdateArmorSet(Player player)
    {
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<MysteriaClump>(6)
            .AddIngredient<MysteriaWood>(12)
            .AddIngredient(ItemID.ChlorophyteBar, 16)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }

    public Vector2 HatPosition(Player player) => player.position - new Vector2(-10, -6);
    public Texture2D HatTexture() => _hatSheet.Value;
    public Texture2D HatBackTexture() => _hatBackSheet.Value;

    public bool HatModifyFraming(Rectangle baseFrame, out Rectangle frame)
    {
        frame = baseFrame;
        frame.Height -= 4;
        return false;
    }
}