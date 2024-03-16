using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;

namespace Verdant.NPCs.Passive.Floties;

public class Flotie : ModNPC
{
    public static Asset<Texture2D> glowTexture;

    public override void Unload() => glowTexture = null;

    public override void SetStaticDefaults()
    {
        Main.npcCatchable[NPC.type] = true;
        Main.npcFrameCount[NPC.type] = 4;

        NPCID.Sets.CountsAsCritter[Type] = true;

        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override void SetDefaults()
    {
        NPC.width = 36;
        NPC.height = 48;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.dontTakeDamage = false;
        NPC.value = 0f;
        NPC.knockBackResist = 0f;
        NPC.aiStyle = -1;
        NPC.dontCountMe = true;
        NPC.catchItem = (short)ModContent.ItemType<FlotieItem>();
        SpawnModBiomes = [ModContent.GetInstance<Scenes.VerdantBiome>().Type];
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.AddInfo(this, "");

    public override void AI() => FlotieCommon.Behavior(NPC, 1, 1);

    public override void FindFrame(int frameHeight)
    {
        if (NPC.frameCounter == 0)
            NPC.frameCounter = Main.rand.Next(2) * 2 + 1;

        int frame = (int)NPC.frameCounter - 1;

        if (NPC.velocity.Y < 0.01f)
            NPC.frame.Y = frame * frameHeight;
        else
            NPC.frame.Y = (frame + 1) * frameHeight;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
        {
            FlotieCommon.DeathGores(NPC);

            for (int i = 0; i < 3; ++i)
            {
                var pos = NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height));
                Gore.NewGore(NPC.GetSource_Death(), pos, Vector2.Zero, Mod.Find<ModGore>("RedPetalFalling").Type);
            }
        }
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.AddCommon<LushLeaf>(1, 1, 2);
        npcLoot.AddCommon<RedPetal>();
        npcLoot.AddCommon<Lightbulb>(10);
    }

    public override int SpawnNPC(int tileX, int tileY) => FlotieCommon.SpawnFlotinies(tileX, tileY, Type, ModContent.NPCType<Flotiny>());
    public override float SpawnChance(NPCSpawnInfo spawnInfo) => FlotieCommon.FlotieSpawnRate(spawnInfo, FlotieType.Verdant);
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => FlotieCommon.GlowDraw(NPC, screenPos, glowTexture.Value, new Vector2(1, -2));
}

public class MysteriaFlotie : Flotie
{
    public new static Asset<Texture2D> glowTexture;

    public override void Unload() => glowTexture = null;

    public override void SetStaticDefaults()
    {
        Main.npcCatchable[NPC.type] = true;
        Main.npcFrameCount[NPC.type] = 4;

        NPCID.Sets.CountsAsCritter[Type] = true;

        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.width = 30;
        NPC.height = 40;
        NPC.catchItem = (short)ModContent.ItemType<MysteriaFlotieItem>();
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.AddInfo(this, "");

    public override void AI()
    {
        FlotieCommon.Behavior(NPC, 1.5f, 1.25f);
        Player plr = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)];

        if (plr.active && !plr.dead && plr.DistanceSQ(NPC.Center) < 300 * 300)
            NPC.velocity += (plr.Center - NPC.Center) * (1 - (NPC.Distance(plr.Center) / 300f)) * 0.0075f;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.AddCommon<LushLeaf>(1, 1, 2);
        npcLoot.AddCommon<MysteriaClump>(1, 2, 5);
        npcLoot.AddCommon<Lightbulb>(10);
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo) => FlotieCommon.FlotieSpawnRate(spawnInfo, FlotieType.Mysteria, 1);
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => FlotieCommon.GlowDraw(NPC, screenPos, glowTexture.Value, new Vector2(1, -4));
}