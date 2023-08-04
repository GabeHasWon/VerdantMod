using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
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
        Item.rare = ItemRarityID.Purple;
        Item.defense = 14;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<ApotheoticChestplate>() && legs.type == ModContent.ItemType<MysteriaLeggings>();

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Summon) += 0.1f;
        player.maxMinions += 2;
        player.GetModPlayer<HostHelmetPlayer>().active = true;
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = "Bee swarm does increased damage\nWhen the bees kill something, they drop a honey heart pickup which heals 10 health";
        player.GetModPlayer<HostHelmetPlayer>().setBonus = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<MysteriaClump>(), 10)
            .AddIngredient(ModContent.ItemType<MysteriaWood>(), 10)
            .AddIngredient(ItemID.ChlorophyteBar, 16)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }

    public Vector2 HatPosition(Player player) => player.position - new Vector2(-12 + (player.direction == -1 ? 4 : 0), -2);
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

internal class HostSwarm : ModProjectile
{
    const int MaxBees = 25;
    const float MaxTimeLeft = 20;

    public static Asset<Texture2D> BeeTexture;

    private Player Owner => Main.player[Projectile.owner];
    private bool Active => !Owner.dead && Main.mouseLeft && Owner.HeldItem.IsAir;

    private List<HostSwarmBee> bees = new();

    private bool Remove
    {
        get => Projectile.ai[0] == 1;
        set => Projectile.ai[0] = value ? 1 : 0;
    }

    private ref float Timer => ref Projectile.ai[1];

    public override void SetStaticDefaults() => BeeTexture = ModContent.Request<Texture2D>(Texture + "Bee");
    public override void Unload() => BeeTexture = null;

    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.width = Projectile.height = 70;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.hostile = false;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.extraUpdates = 1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 7;
        Projectile.timeLeft = (int)MaxTimeLeft;

        AIType = 0;
    }

    public override void OnSpawn(IEntitySource source)
    {
        bees = new List<HostSwarmBee>(MaxBees);

        for (int i = 0; i < MaxBees; ++i)
            bees.Add(new(Projectile));
    }

    public override void AI()
    {
        if (!Remove)
            Projectile.timeLeft++;

        foreach (var item in bees)
            item.Update(Remove);

        if ((!Remove && !Owner.GetModPlayer<HostHelmetPlayer>().active) || Owner.dead)
            Remove = true;

        Projectile.damage = (int)Owner.GetDamage(DamageClass.Summon).ApplyTo(Owner.GetModPlayer<HostHelmetPlayer>().setBonus ? 14 : 8);

        Vector2 target = Active ? Main.MouseWorld : Owner.Center - new Vector2(0, 30);
        Projectile.Center = Vector2.Lerp(Projectile.Center, target, Active ? 0.03f : 0.05f);
        Projectile.velocity = Vector2.Zero;
    }

    public override void PostDraw(Color lightColor)
    {
        Timer++;
        float alpha = Remove ? Projectile.timeLeft / MaxTimeLeft : 1f;

        for (int i = 0; i < bees.Count; ++i)
            bees[i].Draw(alpha, i + (int)Timer);
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) => modifiers.FinalDamage.Base += Projectile.damage;
    public override bool? CanHitNPC(NPC target) => Active;
    public override bool? CanCutTiles() => false;

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (target.life <= 0)
        {
            var item = Owner.QuickSpawnItemDirect(target.GetSource_OnHurt(Projectile), ItemID.Heart);
            item.Center = target.Center;
        }
    }

    internal class HostSwarmBee
    {
        internal Vector2 position;
        internal Vector2 velocity;
        internal Projectile parent;
        internal Vector2 scale;

        public HostSwarmBee(Projectile parent)
        {
            this.parent = parent;

            position = Vector2.Zero;
            scale = new(Main.rand.NextFloat(0.6f, 1.4f));
            velocity = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(1f, 3f);
        }

        public void Update(bool fade)
        {
            const float MaxDistance = 40;

            position += velocity;

            if (!fade && position.LengthSquared() > MaxDistance * MaxDistance)
            {
                velocity *= -1;
                position += velocity;
            }
            else
                velocity = velocity.RotatedByRandom(0.08f);
        }

        public void Draw(float alpha, int timer)
        {
            var realPos = parent.Center + position;
            var light = Lighting.GetColor(realPos.ToTileCoordinates()) * alpha;
            var effect = velocity.X <= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            var source = new Rectangle(14 * (timer % 10 < 5 ? 1 : 0), 0, 12, 14);
            Main.spriteBatch.Draw(BeeTexture.Value, realPos - Main.screenPosition, source, light, 0f, Vector2.Zero, scale, effect, 0);
        }
    }
}