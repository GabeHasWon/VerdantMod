using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter.Fish;
using Verdant.Walls;

namespace Verdant.NPCs.Passive.Fish;

public class Axolotl : ModNPC
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
        NPC.width = 40;
        NPC.height = 20;
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
        NPC.catchItem = (short)ModContent.ItemType<AxolotlItem>();

        if (Main.netMode != NetmodeID.Server)
            NPC.HitSound = new SoundStyle("Verdant/Sounds/AxolotlBoop") with { Pitch = 0.35f, PitchVariance = 0.2f, Volume = 1.5f };

        AnimationType = NPCID.BlueJellyfish;
        SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            new FlavorTextBestiaryInfoElement("A bizarre specimen, mostly aquatic but survives on land just fine...if a little jumpy."),
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

        NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
        NPC.noGravity = NPC.wet;
        NPC.TargetClosest(faceTarget: false);

        if (Main.rand.NextBool(1000))
            SoundEngine.PlaySound(new SoundStyle("Verdant/Sounds/AxolotlBoop") with { Pitch = 0.85f, PitchVariance = 0.15f }, NPC.Center);

        if (NPC.wet)
        {
            if (NPC.collideX)
                NPC.direction *= -1;

            NPC.velocity.X = NPC.direction * 1.5f;
            NPC.velocity.Y *= 0.8f;
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
            for (int i = 0; i < 18; ++i)
            {
                int type = Main.rand.NextFloat() < 0.1f ? DustID.PinkSlime : DustID.BubbleBurst_White;
                Dust.NewDust(NPC.position, NPC.width, NPC.height, type, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
            }
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        const int Distance = 8;

        if (!spawnInfo.Water)
            return 0;

        for (int i = spawnInfo.SpawnTileX - Distance; i < spawnInfo.SpawnTileX + Distance; ++i)
        {
            for (int j = spawnInfo.SpawnTileY - Distance; j < spawnInfo.SpawnTileY + Distance; ++j)
            {
                int wall = Main.tile[i, j].WallType;

                if (wall == ModContent.WallType<BubblingWall_Unsafe>() || wall == ModContent.WallType<BubblingWall>())
                    return 6f * (spawnInfo.PlayerInTown ? 1.75f : 1f);
            }
        }
        return 0;
    }
}