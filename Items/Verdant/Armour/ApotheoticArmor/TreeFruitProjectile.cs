using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace Verdant.Items.Verdant.Armour.ApotheoticArmor;

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