using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.RealtimeGeneration;

namespace Verdant.NPCs.Enemy.PestControl;

public class GrimLayer : ModNPC
{
    enum LayerState
    {
        Initialize,
        Move,
    }

    private Player Target => Main.player[NPC.target];

    private ref float TargetThorn => ref NPC.ai[0];
    private LayerState State { get => (LayerState)NPC.ai[1]; set => NPC.ai[1] = (float)value; }

    private RealtimeAction Pillar;

    public override void SetStaticDefaults()
    {
        DisplayName.SetDefault("Grim Layer");
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
            new FlavorTextBestiaryInfoElement("."),
        });
    }

    public override void AI()
    {
        NPC.TargetClosest(true);
    }
}