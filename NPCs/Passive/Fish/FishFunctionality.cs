using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Dusts;
using Verdant.Scenes;
using Verdant.Systems.TearRain;

namespace Verdant.NPCs.Passive.Fish;

[ReinitializeDuringResizeArrays]
internal class FishFunctionality : GlobalNPC
{
    public static bool[] IsFish = NPCID.Sets.Factory.CreateNamedSet("IsFish").Description("Determines if the given NPC ID is that of a fish.")
        .RegisterBoolSet(NPCID.GoldGoldfish, NPCID.Goldfish);

    public override bool InstancePerEntity => true;

    private bool lastWet = false;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => IsFish[entity.type];

    public override bool PreAI(NPC npc)
    {
        lastWet = npc.wet;

        if (TearRainSystem.Raining && Main.player[Player.FindClosest(npc.position, npc.width, npc.height)].InModBiome<VerdantUndergroundBiome>())
        {
            npc.wet = true;

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(npc.BottomLeft - (Vector2.UnitY * 6), npc.width, 2, ModContent.DustType<VerdantWaterSplash>(), 0, 2);
                Main.dust[dust].alpha = Main.rand.Next(0, 120);
            }
        }

        return true;
    }

    public override void PostAI(NPC npc)
    {
        npc.wet = lastWet;
    }
}
