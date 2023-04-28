using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Trees;

namespace Verdant.NPCs.Passive.Floties;

internal class FlotieCommon
{
    public static void DeathGores(NPC npc, float mult = 1)
    {
        for (int i = 0; i < 6 * mult; ++i)
        {
            var pos = npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height));
            Gore.NewGore(npc.GetSource_Death(), pos, Vector2.Zero, ModContent.Find<ModGore>("Verdant/LushLeaf").Type);
        }

        for (int i = 0; i < 12 * mult; ++i)
            Dust.NewDust(npc.Center, 26, 18, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
    }

    public static int SpawnFlotinies(int tileX, int tileY, int ownType, int flotinyType)
    {
        int rnd = Main.rand.Next(1, 5);
        for (int i = 0; i < rnd; ++i)
            NPC.NewNPC(Entity.GetSource_NaturalSpawn(), tileX * 16 + Main.rand.Next(-120, 120), tileY * 16 + Main.rand.Next(-180, 180), flotinyType);

        return NPC.NewNPC(null, tileX * 16 + 8, tileY * 16, ownType);
    }

    public static float FlotieSpawnRate(NPCSpawnInfo spawnInfo, FlotieType type, float spawnMod = 1f)
    {
        bool baseValid;

        if (type == FlotieType.Mysteria)
        {
            int[] mysteriaTypes = new int[] { ModContent.TileType<MysteriaTree>(), ModContent.TileType<MysteriaTreeTop>() };
            baseValid = spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && mysteriaTypes.Contains(spawnInfo.SpawnTileType);

            if (baseValid && (spawnInfo.PlayerInTown || spawnInfo.PlayerSafe))
                return 1.75f * spawnMod;
            return baseValid ? 0.75f * spawnMod : 0f;
        }

        int[] invalidTypes = new int[] { ModContent.TileType<MysteriaTree>(), ModContent.TileType<MysteriaTreeTop>() };
        baseValid = spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && !invalidTypes.Contains(spawnInfo.SpawnTileType);

        if (baseValid && (spawnInfo.PlayerInTown || spawnInfo.PlayerSafe))
            return 1.25f * spawnMod;
        return baseValid ? 0.5f * spawnMod : 0f;
    }

    public static void GlowDraw(NPC self, Vector2 screenPos, Texture2D tex, Vector2 offset)
    {
        Color color = self.ModNPC.GetAlpha(Color.White) ?? Color.White;

        if (self.IsABestiaryIconDummy)
            color = Color.White;

        Vector2 size = self.frame.Size() - new Vector2(2);
        Main.EntitySpriteDraw(tex, self.Center - screenPos - offset, self.frame, color, self.rotation, size / 2f, 1f, SpriteEffects.None, 0);
    }

    public static void Behavior(NPC self, float multiplier = 1f, float lightMultiplier = 1f)
    {
        if (self.ai[1] == 0)
        {
            self.ai[0] = Main.rand.Next(50) * multiplier;
            self.ai[1] = Main.rand.Next(90, 131) * 0.01f * (Main.rand.NextBool() ? -1 : 1);
            self.ai[2] = Main.rand.Next(100, 121) * 0.01f * (Main.rand.NextBool() ? -1 : 1);
            self.netUpdate = true;
        }

        self.rotation = self.velocity.X * 0.4f;
        self.velocity.Y = (float)(Math.Sin(self.ai[0]++ * 0.02f * self.ai[1]) * 0.6f);
        self.velocity.X = (float)(Math.Sin(self.ai[0] * 0.006f) * 0.15f) * self.ai[2];

        Lighting.AddLight(self.position, new Vector3(0.5f, 0.16f, 0.30f) * lightMultiplier);
    }
}

public enum FlotieType
{
    Verdant,
    Mysteria,
    Puff
}
