using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.NPCs.Enemy.PestControl.Thorns;

namespace Verdant.NPCs.Enemy.PestControl;

public class ThornBeholder : ModNPC
{
    enum ThornState
    {
        Seek,
        Planting,
        Planted
    }

    public override bool IsLoadingEnabled(Mod mod) => false;

    private Player Target => Main.player[NPC.target];

    private ref float Timer => ref NPC.ai[0];
    private ThornState State { get => (ThornState)NPC.ai[1]; set => NPC.ai[1] = (float)value; }

    private List<int> _thorns = new();

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Thorny Beholder");
        Main.npcFrameCount[NPC.type] = 2;
    }

    public override void SetDefaults()
    {
        NPC.width = 60;
        NPC.height = 82;
        NPC.damage = 0;
        NPC.defense = 30;
        NPC.lifeMax = 800;
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
            new FlavorTextBestiaryInfoElement("The eye has seen far too much. A former ent, perhaps even a dryad, lost to the whim of corruption. Concerningly good at tic tac toe."),
        });
    }

    public override void AI()
    {
        NPC.TargetClosest(true);

        if (State != ThornState.Seek)
        {
            if (State == ThornState.Planting)
                PlantingAI();
            else
                PlantedAI();
        }
        else
            SeekAI();
    }

    private void PlantedAI()
    {
        const int MaxThorns = 3;

        _thorns.RemoveAll(x => x < 0 || x > Main.maxNPCs || !Main.npc[x].active);
        
        if (_thorns.Count < MaxThorns)
            Timer++;

        if (Timer > 120 && Timer % 80 == 0)
        {
            bool big = Main.rand.NextBool(3);
            int npc = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, big ? ModContent.NPCType<BigThorn>() : ModContent.NPCType<SmallThorn>());
            NPC thorn = Main.npc[npc];
            thorn.velocity = new Vector2(Main.rand.NextFloat(1.8f, 4f) * (Main.rand.NextBool() ? -1 : 1), Main.rand.NextFloat(-16, -11f));

            if (big)
                thorn.velocity *= 0.75f;

            _thorns.Add(npc);

            if (_thorns.Count >= MaxThorns)
                Timer = 0;
        }
    }

    private void SeekAI()
    {
        NPC.spriteDirection = Math.Sign(Target.Center.X - NPC.position.X);

        if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, Target.position, Target.width, Target.height))
            State = ThornState.Planting;
    }

    private void PlantingAI()
    {
        Timer++;

        if (Timer <= 10)
            NPC.velocity.Y -= 0.4f;
        else if (Timer <= 100)
            NPC.velocity.Y += 0.9f;

        bool left = Collision.SolidCollision(NPC.position + new Vector2(0, 28), NPC.width / 2, 10);
        bool middle = Collision.SolidCollision(NPC.position + new Vector2(NPC.width / 3, 28), NPC.width / 3, 10);
        bool right = Collision.SolidCollision(NPC.position + new Vector2(NPC.width / 1.5f, 28), NPC.width / 3, 10);

        if (left && middle && right)
        {
            State = ThornState.Planted;
            Timer = 0;

            SoundEngine.PlaySound(SoundID.DD2_OgreGroundPound with { Volume = 0.8f, Pitch = 0.4f, PitchVariance = 0.4f }, NPC.position + new Vector2(0, 20));
            Collision.HitTiles(NPC.position + new Vector2(0, 28), NPC.velocity, NPC.width, 10);

            NPC.velocity = Vector2.Zero;
            NPC.netUpdate = true;
        }
    }
}