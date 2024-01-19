using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Misc;
using Verdant.Items.Verdant.Critter;

namespace Verdant.NPCs.Passive.Snails;

public class VerdantRedGrassSnail : ModNPC
{
    public override void SetStaticDefaults()
    {
        Main.npcCatchable[NPC.type] = true;
        Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Snail];

        NPCID.Sets.CountsAsCritter[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Snail);
        NPC.width = 22;
        NPC.height = 18;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.value = 0f;
        NPC.knockBackResist = 0f;
        NPC.dontCountMe = true;
        NPC.catchItem = (short)ModContent.ItemType<RedGrassSnail>();

        AnimationType = NPCID.Snail;
        SpawnModBiomes = [ModContent.GetInstance<Scenes.VerdantBiome>().Type];
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.AddInfo(this, "");

    public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<SnailShellBlockItem>(1, 1, 3);

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (NPC.life <= 0)
        {
            if (Main.netMode != NetmodeID.Server)
                for (int i = 0; i < 3; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)), Vector2.Zero, Mod.Find<ModGore>("LushLeaf").Type);

            for (int i = 0; i < 4; ++i)
                Dust.NewDust(NPC.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && spawnInfo.PlayerInTown)
            return 2f;
        return spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant ? 0.8f : 0f;
    }
}

public class VerdantBulbSnail : VerdantRedGrassSnail
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.width = 20;
        NPC.height = 16;
        NPC.catchItem = (short)ModContent.ItemType<BulbSnail>();
    }

    public override void AI() => Lighting.AddLight(NPC.Center, Color.HotPink.ToVector3() * 0.4f);
}

public class ShellSnail : VerdantRedGrassSnail
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        NPC.width = 20;
        NPC.height = 16;
        NPC.catchItem = (short)ModContent.ItemType<BulbSnail>();

        DrawOffsetY = 2;
    }

    public override void AI() => Lighting.AddLight(NPC.Center, Color.HotPink.ToVector3() * 0.4f);
}