using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Verdant.NPCs.Verdant
{
    public class LegacyNPCMethods
    {
        public static void FlotieLegacyAI(NPC npc)
        {
            npc.TargetClosest(true);
            Player nearest = Main.player[npc.target];

            bool grounded = npc.velocity.Y == 0;// && Helper.SolidTile((int)(npc.position.X / 16f), (int)(npc.position.Y / 16f) + 1);

            npc.ai[0]++;
            if (!grounded)
            {
                if (npc.ai[0] >= 250)
                {
                    npc.ai[0] = 0;
                    npc.velocity.Y = -3;
                    if (Vector2.Distance(nearest.position, npc.position) > 400)
                    {
                        float side = Main.rand.Next(-100, 101) * 0.01f;
                        npc.velocity.X = 3 * side;
                        switch (Main.rand.Next(5))
                        {
                            default: break;
                            case 0:
                                npc.velocity.Y = 0;
                                break;
                            case 1:
                                npc.velocity.Y = -5;
                                break;
                        }
                    }
                    else
                    {
                        int side = nearest.position.X > npc.position.X ? -1 : 1;
                        npc.velocity.X = 3 * side;
                    }
                }
            }
            else
            {
                if (npc.ai[0] == 50 && Main.rand.Next(4) > 0)
                {
                    int side = nearest.position.X > npc.position.X ? -1 : 1;
                    npc.velocity.X = 1.75f * side;
                    npc.ai[0] = 0;
                }
                else
                {
                    npc.velocity.Y = -6;
                    npc.ai[0] = 0;
                }
            }

            npc.velocity.Y += 0.02f;
            if (npc.velocity.Y < -2.5f)
                npc.velocity.Y = -2.5f;
            npc.velocity.X *= 0.98f;

            Lighting.AddLight(npc.position, new Vector3(0.5f, 0.16f, 0.30f));
        }
    }
}