using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.NPCs.Enemy.PestControl;

public class DimCore : ModNPC
{
    public const int Radius = 180;

    enum CoreState
    {
        Initialize,
        Move,
    }

    private Player Target => Main.player[NPC.target];

    private ref float TargetThorn => ref NPC.ai[0];
    private CoreState State { get => (CoreState)NPC.ai[1]; set => NPC.ai[1] = (float)value; }

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Dim Core");

        Main.npcFrameCount[NPC.type] = 1;
        NPCID.Sets.TrailingMode[Type] = 2;
        NPCID.Sets.TrailCacheLength[Type] = 10;
    }

    public override void SetDefaults()
    {
        NPC.width = 42;
        NPC.height = 42;
        NPC.damage = 0;
        NPC.defense = 30;
        NPC.lifeMax = 1100;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.value = Item.buyPrice(0, 0, 10);
        NPC.knockBackResist = 0f;
        NPC.aiStyle = -1;
        NPC.HitSound = SoundID.Critter;
        NPC.DeathSound = SoundID.NPCDeath4;
        SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            new FlavorTextBestiaryInfoElement("A light bulb that has been completely corrupted. The gel-like body holds a pus-like substance and a dimmed light."),
        });
    }

    public override void AI()
    {
        NPC.TargetClosest(true);

        if (State == CoreState.Initialize)
            Initialize();
        else
        {
            if (TargetThorn == -1)
                FindThorn();

            if (TargetThorn == -1)
                NPC.velocity = NPC.DirectionTo(Target.Center) * NPC.Distance(Target.Center) * 0.01f;
            else
                NPC.velocity = NPC.DirectionTo(Main.npc[(int)TargetThorn].Center) * NPC.Distance(Target.Center) * 0.01f;
        }

        NPC.rotation = NPC.velocity.X * 0.08f;
    }

    private void FindThorn()
    {
        foreach (var npc in ActiveEntities.NPCs)
        {
            if (npc.CanBeChasedBy() && NPC.DistanceSQ(npc.Center) > Radius * Radius * 0.99f && (NPC.type == ModContent.NPCType<Thorns.SmallThorn>() || NPC.type == ModContent.NPCType<Thorns.BigThorn>()))
            {
                TargetThorn = npc.whoAmI;
                return;
            }
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < 9; ++i)
        {
            int shield = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CoreOrbiter>(), NPC.whoAmI);
            Main.npc[shield].ai[0] = NPC.whoAmI;
            Main.npc[shield].ai[1] = Main.rand.Next(8);
        }

        TargetThorn = -1;
        State = CoreState.Move;
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter++;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D tex = TextureAssets.Npc[Type].Value;
        float sin = MathF.Sin((float)NPC.frameCounter * 0.06f) * 0.25f;
        Vector2 scale = new Vector2(sin + 1, -sin + 1);

        spriteBatch.Draw(tex, NPC.Center - screenPos, null, drawColor, NPC.rotation, NPC.Size / 2f, scale * 0.9f, SpriteEffects.None, 0);
        return false;
    }

    public static void OrbiterAI(NPC npc, NPC parent, ref Vector2 _offset)
    {
        if (_offset == Vector2.Zero)
        {
            _offset = new Vector2(Main.rand.NextFloat(20, 50) * (Main.rand.NextBool(2) ? -1 : 1), Main.rand.NextFloat(20, 50) * (Main.rand.NextBool(2) ? -1 : 1));
            npc.netUpdate = true;
        }

        float rotSin = MathF.Sin((float)(parent.frameCounter + (npc.whoAmI * 0.8f)) * 0.002f);
        npc.Center = parent.oldPos[npc.whoAmI % 10] + _offset.RotatedBy(rotSin * 3) + parent.Size / 2f;
        npc.position.Y += MathF.Sin((float)(parent.frameCounter + (npc.whoAmI * 0.8f)) * 0.07f) * 14f;
        npc.rotation = parent.velocity.X * 0.05f;
    }

    private class CoreOrbiter : ModNPC
    {
        private Vector2 _offset = Vector2.Zero;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Thorny Detritus");

        public override void SetDefaults()
        {
            NPC.width = 10;
            NPC.height = 10;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 120;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.Critter;
            NPC.DeathSound = SoundID.NPCDeath4;

            for (int i = 0; i < NPC.buffImmune.Length; ++i)
                NPC.buffImmune[i] = true;

            SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("Parts of dead thorns, floating around a Dim Core. Despite their look, these are soft to the touch...almost comfortable."),
            });
        }

        public override void AI() => OrbiterAI(NPC, Main.npc[(int)NPC.ai[0]], ref _offset);

        public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(_offset);
        public override void ReceiveExtraAI(BinaryReader reader) => _offset = reader.ReadVector2();

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;

            Main.EntitySpriteDraw(tex, NPC.position - screenPos - NPC.Size / 2f, new Rectangle((int)(16 * NPC.ai[1]), 0, 14, 20), drawColor, NPC.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}