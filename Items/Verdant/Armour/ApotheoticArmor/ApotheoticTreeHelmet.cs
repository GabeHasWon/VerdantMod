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

        if (fruits.Any(x => x != FruitType.None) && Player.whoAmI == Main.myPlayer)
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
                        
                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projWhoAmI);
                            Main.projectile[projWhoAmI].netUpdate = true;
                        }

                        fruits[index] = 0;
                        return;
                    }
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

    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        binaryWriter.Write((byte)fruitBuff);
        binaryWriter.Write((short)fruitTime);
    }

    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    {
        fruitBuff = (FruitType)binaryReader.ReadByte();
        fruitTime = binaryReader.ReadInt16();
    }

    public override bool PreAI(Projectile projectile)
    {
        if (!projectile.TryGetOwner(out Player _))
            return true;

        bool setBonus = Main.player[projectile.owner].GetModPlayer<TreeHelmetPlayer>().setBonus;
        float damageBonus = setBonus ? 1.33f : 1;

        if (fruitBuff == FruitType.HoneyDrop)
            RepeatAI(projectile, !setBonus ? 1 : 1 + (fruitTime % 2));
        else if (fruitBuff == FruitType.SweetApple)
        {
            bool extraSpeed = setBonus ? fruitTime % 4 <= 2 : fruitTime % 4 == 0;

            if (extraSpeed)
                RepeatAI(projectile, 1);

            lastDamage = projectile.damage;
            projectile.damage = (int)(projectile.damage * 1.2f * damageBonus);
        }
        else if (fruitBuff == FruitType.SpicyPepper)
        {
            lastDamage = projectile.damage;
            projectile.damage = (int)(projectile.damage * 1.5f * damageBonus);
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
                return GetModColor(Color.LawnGreen);
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

public enum FruitType : int
{
    None,
    HoneyDrop, //Speed
    SweetApple, //Mix
    SpicyPepper //Damage
}