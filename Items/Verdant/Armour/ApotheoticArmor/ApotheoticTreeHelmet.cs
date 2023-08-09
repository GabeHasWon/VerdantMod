using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
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
        Item.rare = ItemRarityID.Yellow;
        Item.defense = 10;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => 
        body.type == ModContent.ItemType<ApotheoticChestplate>() && legs.type == ModContent.ItemType<MysteriaLeggings>();

    public override void UpdateEquip(Player player)
    {
        player.GetModPlayer<TreeHelmetPlayer>().active = true;
        player.GetDamage(DamageClass.Summon) += 0.1f;
        player.maxMinions++;
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = Language.GetTextValue("Mods.Verdant.SetBonuses.Apotheotic.Tree");

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

    public Vector2 HatOffset(Player player, PlayerDrawSet info) => new(0, 6);
    public Texture2D HatTexture() => _hatSheet.Value;
    public Texture2D HatBackTexture() => _hatBackSheet.Value;

    public bool HatModifyFraming(Rectangle baseFrame, out Rectangle frame)
    {
        frame = baseFrame;
        frame.Height -= 4;
        return false;
    }
}

internal class TreeHelmetPlayer : ModPlayer
{
    const int MaxFruits = 3;

    internal bool active = false;
    internal FruitType[] fruits = new FruitType[MaxFruits];

    private int _fruitTimer = 0;

    public override void ResetEffects()
    {
        active = false;
    }

    public override void PostUpdateEquips()
    {
        if (active && fruits.Contains(FruitType.None) && _fruitTimer++ >= 2)// TreeFruitProjectile.MaxFruitTime * 1.5f)
        {
            fruits[Array.IndexOf(fruits, FruitType.None)] = (FruitType)(Main.rand.Next(3) + 1);
            _fruitTimer = 0;
        }

        if (fruits.Any(x => x != FruitType.None))
            ScanMinions();
    }

    private void ScanMinions()
    {
        foreach (var proj in ActiveEntities.Projectiles) 
        {
            if (proj.active && proj.owner == Player.whoAmI)
            {
                if (!proj.TryGetGlobalProjectile(out TreeFruitProjectile fruitProj))
                    continue;

                var hitbox = proj.Hitbox;
                hitbox.Inflate(20, 20);

                if (hitbox.Contains(Main.MouseWorld.ToPoint()) && fruitProj.fruitBuff == FruitType.None && Main.mouseLeft && Main.mouseLeftRelease)
                {
                    int index = Array.FindIndex(fruits, x => x != FruitType.None);

                    if (index == -1)
                        return;

                    Projectile.NewProjectile(Player.GetSource_Accessory(Player.armor[0]), Player.Center, Vector2.Zero, 
                        ModContent.ProjectileType<FruitProjectile>(), 0, 0, Player.whoAmI, proj.whoAmI, (float)fruits[index]);
                    fruits[index] = 0;
                }
            }
        }
    }
}

internal class TreeFruitProjectile : GlobalProjectile
{
    public const int MaxFruitTime = 15 * 60;

    public override bool InstancePerEntity => true;

    internal FruitType fruitBuff = FruitType.None;
    internal int fruitTime = 0;

    private int? lastDamage = null;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.minion;
    public override void Kill(Projectile projectile, int timeLeft) => fruitBuff = FruitType.None;

    public override bool PreAI(Projectile projectile)
    {
        if (fruitBuff == FruitType.HoneyDrop)
            RepeatAI(projectile, 1);
        else if (fruitBuff == FruitType.SweetApple)
        {
            if (projectile.timeLeft % 2 == 0)
                RepeatAI(projectile, 1);

            lastDamage = projectile.damage;
            projectile.damage = (int)(projectile.damage * 1.15f);
        }
        else if (fruitBuff == FruitType.SpicyPepper)
        {
            lastDamage = projectile.damage;
            projectile.damage = (int)(projectile.damage * 1.5f);
        }

        if (fruitBuff != FruitType.None)
        {
            if (--fruitTime <= 0)
                fruitBuff = FruitType.None;
        }
        return true;
    }

    public override void PostAI(Projectile projectile)
    {
        if (lastDamage.HasValue)
        {
            projectile.damage = lastDamage.Value;
            lastDamage = null;
        }
    }

    private static void RepeatAI(Projectile projectile, int repeats)
    {
        int type = projectile.type;
        bool actType = projectile.ModProjectile != null && projectile.ModProjectile.AIType > 0;

        for (int i = 0; i < repeats; ++i)
        {
            if (actType)
                projectile.type = projectile.ModProjectile.AIType;

            projectile.VanillaAI();

            if (actType)
                projectile.type = type;
        }

        ProjectileLoader.AI(projectile);
    }

    public override Color? GetAlpha(Projectile projectile, Color lightColor)
    {
        Color GetModColor(Color mod) => Color.Multiply(mod, Lighting.Brightness((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f)));

        switch (fruitBuff)
        {
            case FruitType.SpicyPepper:
                return GetModColor(Color.LightGreen);
            case FruitType.SweetApple:
                return GetModColor(Color.Orange);
            case FruitType.HoneyDrop:
                return GetModColor(Color.LightGoldenrodYellow);
            default:
                return null;
        }
    }

    public override void PostDraw(Projectile projectile, Color lightColor)
    {
        if (fruitBuff == FruitType.None)
            return;

        Main.instance.LoadProjectile(ModContent.ProjectileType<FruitProjectile>());
        var tex = TextureAssets.Projectile[ModContent.ProjectileType<FruitProjectile>()].Value;
        var pos = projectile.Center - new Vector2(0, projectile.height + projectile.gfxOffY + 4);
        var col = Color.Lerp(Lighting.GetColor(pos.ToTileCoordinates()), Color.White, 0.25f) * projectile.Opacity;
        float cutoff = fruitTime / (float)MaxFruitTime;
        var frame = new Rectangle(16 * (int)(fruitBuff - 1), 16 + (int)(18 * (1 - cutoff)), 14, (int)(18 * cutoff));

        Main.EntitySpriteDraw(tex, pos - Main.screenPosition, frame, col, 0f, frame.Size() / 2f, 1f, SpriteEffects.None, 0);
    }
}

internal enum FruitType : int
{
    None,
    HoneyDrop, //Speed
    SweetApple, //Mix
    SpicyPepper //Damage
}