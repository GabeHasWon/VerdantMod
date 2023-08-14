using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;
using Verdant.Players.Layers;

namespace Verdant.Items.Verdant.Armour.ApotheoticArmor;

[AutoloadEquip(EquipType.Head)]
public class ApotheoticBeeHelmet : ModItem, ITallHat
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
        Item.rare = ItemRarityID.Yellow;
        Item.defense = 14;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) 
        => body.type == ModContent.ItemType<ApotheoticChestplate>() && legs.type == ModContent.ItemType<ApotheoticLeggings>();

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Summon) += 0.1f;
        player.maxMinions += 2;
        player.GetModPlayer<HostHelmetPlayer>().active = true;
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = Language.GetTextValue("Mods.Verdant.SetBonuses.Apotheotic.Bee");
        player.maxMinions++;
        player.GetModPlayer<HostHelmetPlayer>().setBonus = true;
        player.GetDamage(DamageClass.Summon) += 0.05f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<MysteriaClump>(), 10)
            .AddIngredient(ModContent.ItemType<MysteriaWood>(), 10)
            .AddIngredient<ApotheoticSoul>(1)
            .AddIngredient(ItemID.ChlorophyteBar, 16)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }

    public Vector2 HatOffset(Player player, PlayerDrawSet info) => new(0, 2);
    public Texture2D HatTexture() => _hatSheet.Value;
    public Texture2D HatBackTexture() => _hatBackSheet.Value;

    public bool HatModifyFraming(Rectangle baseFrame, out Rectangle frame)
    {
        frame = baseFrame;
        frame.Height -= 4;
        return false;
    }
}

internal class HostHelmetPlayer : ModPlayer
{
    internal bool active = false;
    internal bool setBonus = false;

    public override void ResetEffects()
    {
        active = false;
        setBonus = false;
    }

    public override void PreUpdate()
    {
        if (!Player.dead && active && Player.ownedProjectileCounts[ModContent.ProjectileType<HostSwarm>()] <= 0)
        {
            int dmg = (int)Player.GetDamage(DamageClass.Summon).ApplyTo(setBonus ? 14 : 8);
            Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, ModContent.ProjectileType<HostSwarm>(), dmg, 1f);
        }
    }
}