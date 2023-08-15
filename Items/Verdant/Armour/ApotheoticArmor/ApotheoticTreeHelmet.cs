using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Verdant.Items.Verdant.Blocks.Mysteria;
using Verdant.Items.Verdant.Materials;
using Verdant.Players.Layers;
using Verdant.Systems.Syncing;

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
        body.type == ModContent.ItemType<ApotheoticChestplate>() && legs.type == ModContent.ItemType<ApotheoticLeggings>();

    public override void UpdateEquip(Player player)
    {
        player.GetModPlayer<TreeHelmetPlayer>().active = true;
        player.GetDamage(DamageClass.Summon) += 0.1f;
        player.maxMinions++;
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = Language.GetTextValue("Mods.Verdant.SetBonuses.Apotheotic.Tree");
        player.GetModPlayer<TreeHelmetPlayer>().setBonus = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<MysteriaClump>(6)
            .AddIngredient<MysteriaWood>(12)
            .AddIngredient<ApotheoticSoul>(1)
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
    internal bool setBonus = false;
    internal FruitType[] fruits = new FruitType[MaxFruits];
    internal int fruitTimer = 0;

    public override void ResetEffects()
    {
        active = false;
        setBonus = false;
    }

    public override void CopyClientState(ModPlayer targetCopy)
    {
        var target = targetCopy as TreeHelmetPlayer;
        target.fruits = fruits;
        target.fruitTimer = fruitTimer;
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        var clone = clientPlayer as TreeHelmetPlayer;

        if (fruits != clone.fruits || fruitTimer != clone.fruitTimer)
            SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) => new SyncTreebandModule(fruits, fruitTimer, (short)fromWho).Send(toWho, fromWho);

    public override void PostUpdateEquips()
    {
        if (active && fruits.Contains(FruitType.None) && ++fruitTimer > TreeFruitProjectile.MaxFruitTime * (setBonus ? 1.15f : 1.5f))
        {
            fruits[Array.IndexOf(fruits, FruitType.None)] = (FruitType)(Main.rand.Next(3) + 1);
            fruitTimer = 0;
        }

        if (fruits.Any(x => x != FruitType.None) && Main.myPlayer == Player.whoAmI)
            ScanMinions();
    }

    private void ScanMinions()
    {
        foreach (var proj in ActiveEntities.Projectiles) 
        {
            if (proj.active && (proj.owner == Player.whoAmI || (proj.TryGetOwner(out Player otherPlayer) && otherPlayer.InOpposingTeam(Player))))
            {
                if (!proj.TryGetGlobalProjectile(out TreeFruitProjectile fruitProj))
                    continue;

                var hitbox = proj.Hitbox;
                hitbox.Inflate(20, 20);

                if (hitbox.Contains(Main.MouseWorld.ToPoint()) && fruitProj.fruitBuff == FruitType.None)
                {
                    int index = Array.FindIndex(fruits, x => x != FruitType.None);

                    if (index == -1)
                        return;

                    Player.noThrow = 2;
                    Player.cursorItemIconEnabled = true;
                    Player.cursorItemIconID = fruits[index] switch
                    {
                        FruitType.SpicyPepper => ModContent.ItemType<FruitIconGreen>(),
                        FruitType.SweetApple => ModContent.ItemType<FruitIconRed>(),
                        FruitType.HoneyDrop => ModContent.ItemType<FruitIconYellow>(),
                        _ => throw new Exception("Uh oh? Bad fruit type in ApotheoticTreeHelmet/ScanMinions!")
                    };

                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        int projWhoAmI = Projectile.NewProjectile(Player.GetSource_Accessory(Player.armor[0]), Player.Center, Vector2.Zero,
                            ModContent.ProjectileType<FruitProjectile>(), 0, 0, Player.whoAmI, proj.whoAmI, (float)fruits[index]);

                        fruits[index] = 0;
                        return;
                    }
                }
            }
        }
    }
}