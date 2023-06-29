using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter.Fish;

namespace Verdant.NPCs.Passive.Fish;

public class BulbboxJelly : ModNPC
{
    private Player Target => Main.player[NPC.target];

    private ref float Timer => ref NPC.ai[0];

    public override void SetStaticDefaults()
    {
        Main.npcCatchable[Type] = true;
        Main.npcFrameCount[Type] = 4;

        NPCID.Sets.CountsAsCritter[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.width = 22;
        NPC.height = 24;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = false;
        NPC.dontTakeDamage = false;
        NPC.value = 0f;
        NPC.aiStyle = -1;
        NPC.dontCountMe = true;
        NPC.catchItem = (short)ModContent.ItemType<BulbboxJellyItem>();

        AnimationType = NPCID.BlueJellyfish;
        AIType = NPCID.Goldfish;
        SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            new FlavorTextBestiaryInfoElement("A curious, harmless little jelly. Its plant-like tentacles grow from their transparent body."),
        });
    }

    public override void OnSpawn(IEntitySource source)
    {
        NPC.direction = Main.rand.NextBool(2) ? -1 : 1;
        NPC.netUpdate = true;
    }

    public override void AI()
    {
        Timer++;

        NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
        NPC.noGravity = NPC.wet;
        NPC.TargetClosest(faceTarget: false);
        Lighting.AddLight(NPC.Center, new Vector3(0.6f, 0.6f, 0.7f));

        bool canHitPlayer = Collision.CanHit(NPC.position, NPC.width, NPC.height, Target.position, Target.width, Target.height);
        bool playerVisible = Target.wet && !Target.dead && canHitPlayer;

        if (NPC.wet)
        {
            if (Timer % 160 < 2)
            {
                if (playerVisible)
                    NPC.velocity = NPC.DirectionTo(Target.Center) * 6;
                else
                {
                    if (NPC.collideX)
                        NPC.direction *= -1;

                    NPC.velocity.X = NPC.direction * 4;

                }
            }
            else
            {
                NPC.velocity *= 0.98f;

                if (NPC.collideX || NPC.collideY)
                    Timer = 0;
            }
        }
        else
        {
            if (NPC.collideY)
            {
                if (Timer % 60 == 0)
                {
                    NPC.velocity = new Vector2(Main.rand.NextFloat(-3, 3), -5f);
                    NPC.netUpdate = true;
                }

                NPC.velocity.X *= 0.95f;
            }
        }
    }

    public override void HitEffect(int hitDirection, double damage)
    {
        if (NPC.life <= 0)
        {
            for (int i = 0; i < 2; ++i)
                Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(Main.rand.NextFloat(3), Main.rand.NextFloat(-5, 5)), Mod.Find<ModGore>("LushLeaf").Type);
            for (int i = 0; i < 6; ++i)
                Dust.NewDust(NPC.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (!Main.hardMode)
            return 0;

        float baseChance = spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && spawnInfo.Water ? 1.25f : 0f;
        return baseChance * (spawnInfo.PlayerInTown ? 1.75f : 1f);
    }
}