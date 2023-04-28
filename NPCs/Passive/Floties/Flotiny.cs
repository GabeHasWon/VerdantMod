using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;
using Verdant.Items.Verdant.Materials;

namespace Verdant.NPCs.Passive.Floties;

public class Flotiny : Flotie
{
    static Asset<Texture2D> glowTexture;

    public override void Unload() => glowTexture = null;

    public override void SetStaticDefaults()
    {
        Main.npcCatchable[NPC.type] = true;
        Main.npcFrameCount[NPC.type] = 2;

        NPCID.Sets.CountsAsCritter[Type] = true;

        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override void SetDefaults()
    {
        NPC.width = 22;
        NPC.height = 26;
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
        NPC.catchItem = (short)ModContent.ItemType<FlotinyItem>();
        SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            new FlavorTextBestiaryInfoElement("A tiny floating creature. Despite levitating, it makes no sound and doesn't move apart from gentle swaying, as if in a breeze."),
        });
    }

    public override void AI() => FlotieCommon.Behavior(NPC, 0.65f, 0.6f);
    public sealed override int SpawnNPC(int tileX, int tileY) => NPC.NewNPC(null, tileX * 16 + 8, tileY * 16, NPC.type);

    public override void FindFrame(int frameHeight)
    {
        if (NPC.frameCounter == 0)
            NPC.frameCounter = Main.rand.Next(2) + 1;

        NPC.frame.Y = frameHeight * (int)(NPC.frameCounter - 1);
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.AddCommon<LushLeaf>();
        npcLoot.AddCommon<Lightbulb>(3);
    }

    public override void HitEffect(int hitDirection, double damage)
    {
        if (NPC.life <= 0)
            FlotieCommon.DeathGores(NPC, 0.5f);
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo) => FlotieCommon.FlotieSpawnRate(spawnInfo, FlotieType.Verdant, 0.6f);
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => FlotieCommon.GlowDraw(NPC, screenPos, glowTexture.Value, new Vector2(1, -2));
}

public class MysteriaFlotiny : Flotiny
{
    static Asset<Texture2D> glowTexture;

    public override void Unload() => glowTexture = null;

    public override void SetStaticDefaults()
    {
        Main.npcCatchable[NPC.type] = true;
        Main.npcFrameCount[NPC.type] = 2;

        NPCID.Sets.CountsAsCritter[Type] = true;

        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override void SetDefaults()
    {
        base.SetDefaults();
        NPC.catchItem = (short)ModContent.ItemType<MysteriaFlotinyItem>();
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            new FlavorTextBestiaryInfoElement("A recently discovered creature, Mysteria Flotinies seem to congregate around Mysteria canopies. " +
            "Perhaps there's more to learn about these creatures?"),
        });
    }

    public override void AI()
    {
        FlotieCommon.Behavior(NPC, 0.85f, 0.8f);
        Player plr = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)];

        if (plr.active && !plr.dead && plr.DistanceSQ(NPC.Center) < 300 * 300)
            NPC.velocity += (plr.Center - NPC.Center) * (1 - (NPC.Distance(plr.Center) / 300f)) * 0.005f;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.AddCommon<LushLeaf>();
        npcLoot.AddCommon<MysteriaClump>();
        npcLoot.AddCommon<Lightbulb>(3);
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo) => FlotieCommon.FlotieSpawnRate(spawnInfo, FlotieType.Mysteria, 0.6f);
    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => FlotieCommon.GlowDraw(NPC, screenPos, glowTexture.Value, new Vector2(1, -2));
}