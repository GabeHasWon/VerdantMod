using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Tiles.Verdant.Basic.Plants;
using Verdant.Tiles.Verdant.Decor;
using Verdant.Tiles.Verdant.Trees;
using static Terraria.ModLoader.ModContent;

namespace Verdant.Tiles
{
    class VerdantGlobalTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            int[] requireGroundTypes = new int[] { TileType<VerdantTree>(), TileType<Apotheosis>() };
            if (j > 0 && requireGroundTypes.Any(x => Helper.ActiveType(i, j - 1, x)) && Helper.SolidTile(i, j))
            {
                return false;
            }
            return true;
        }

        public override bool CanExplode(int i, int j, int type)
        {
            int[] requireGroundTypes = new int[] { TileType<VerdantTree>(), TileType<Apotheosis>() };
            if (j > 0 && requireGroundTypes.Any(x => Helper.ActiveType(i, j - 1, x)) && Helper.SolidTile(i, j))
            {
                return false;
            }
            return true;
        }

        public override void FloorVisuals(int type, Player player)
        {
            player.GetModPlayer<VerdantPlayer>().FloorVisuals(player, type);
        }

        public override bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            DrawGrounding(i, j, spriteBatch, TileType<VerdantStrongVine>(), type, 2);
            DrawGrounding(i, j, spriteBatch, TileType<VerdantVine>(), type, 0);
            return true;
        }

        public void DrawGrounding(int i, int j, SpriteBatch batch, int type, int thisType, int groundType = 0)
        {
            if (thisType != type)
            {
                if ((groundType == 0 || groundType == 2) && Helper.ActiveType(i, j + 1, type)) //Up-hold
                    batch.Draw(Main.tileTexture[type], Helper.TileCustomPosition(i, j) + new Vector2(0, 8), new Rectangle(0, 8, 16, 8), Lighting.GetColor(i, j));
                if ((groundType == 1 || groundType == 2) && Helper.ActiveType(i, j - 1, type)) //Grounding
                    batch.Draw(Main.tileTexture[type], Helper.TileCustomPosition(i, j), new Rectangle(0, 0, 16, 8), Lighting.GetColor(i, j));
            }
        }
    }
}
