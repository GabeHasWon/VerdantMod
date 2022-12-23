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
    enum CoreState
    {
        Initialize,
        Move,
    }

    private Player Target => Main.player[NPC.target];

    private ref float Timer => ref NPC.ai[0];
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
        NPC.lifeMax = 1500;
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
            NPC.velocity = NPC.DirectionTo(Target.Center) * NPC.Distance(Target.Center) * 0.01f;
    }

    private void Initialize()
    {
        for (int i = 0; i < 9; ++i)
        {
            int shield = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CoreOrbiter>(), NPC.whoAmI);
            Main.npc[shield].ai[0] = NPC.whoAmI;
            Main.npc[shield].ai[1] = Main.rand.Next(8);
        }

        State = CoreState.Move;
    }

    private class CoreOrbiter : ModNPC
    {
        private Vector2 _offset = Vector2.Zero;

        public override void SetStaticDefaults() => DisplayName.SetDefault("Thorny Detritus");

        public override void SetDefaults()
        {
            NPC.width = 8;
            NPC.height = 8;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 40;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.Critter;
            NPC.DeathSound = SoundID.NPCDeath4;

            SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("Parts of dead thorns, floating around a Dim Core. Despite their look, these are soft to the touch...almost comfortable."),
            });
        }

        public override void AI()
        {
            if (_offset == Vector2.Zero)
            {
                _offset = new Vector2(Main.rand.NextFloat(20, 50) * (Main.rand.NextBool(2) ? -1 : 1), Main.rand.NextFloat(20, 50) * (Main.rand.NextBool(2) ? -1 : 1));
                NPC.netUpdate = true;
            }

            var parent = Main.npc[(int)NPC.ai[0]];
            NPC.Center = parent.oldPos[NPC.whoAmI % 10] + _offset + parent.Size / 2f;
        }

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