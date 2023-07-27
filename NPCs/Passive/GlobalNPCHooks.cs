using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using Verdant.NPCs.Passive.Puff;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.NPCs.Passive;

internal class GlobalNPCHooks : ILoadable
{
    public void Load(Mod mod)
    {
        Terraria.IL_NPC.SlimeRainSpawns += AddPuffToSlimeRain;
    }

    private void AddPuffToSlimeRain(ILContext il)
    {
        ILCursor c = new(il);

        if (!c.TryGotoNext(MoveType.After, x => x.MatchStloc(12)))
            return;

        ILLabel marker = c.DefineLabel();

        c.Emit(OpCodes.Ldloc_S, (byte)12);
        c.Emit(OpCodes.Ldloc_S, (byte)3);
        c.EmitDelegate((int whoAmI, Player player) =>
        {
            Point pos = player.Center.ToTileCoordinates();
            if (VerdantGrassLeaves.CheckPuffMicrobiome(pos.X, pos.Y, 1.5f) && Main.rand.NextBool(12))
            {
                Main.npc[whoAmI].SetDefaults(ModContent.NPCType<PuffSlimeSmall>());
                return true;
            }
            return false;
        });

        c.Emit(OpCodes.Brfalse, marker);
        c.Emit(OpCodes.Ret);

        c.MarkLabel(marker);
    }

    public void Unload() { }
}
