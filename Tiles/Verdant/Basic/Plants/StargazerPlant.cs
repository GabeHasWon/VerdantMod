using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Verdant.Items.Verdant.Blocks.Plants;
using Verdant.NPCs.Passive;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.Tiles.Verdant.Basic.Plants;

public class StargazerPlant : ModTile, IFlowerTile
{
    private static Asset<Texture2D> glowTex;

    public override void SetStaticDefaults()
    {
        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, 2, 0);

        List<int> grasses = new(VerdantGrassLeaves.VerdantGrassTypes) { ModContent.TileType<LushSoil>(), TileID.Dirt };

        for (int i = 0; i < TileID.Sets.Grass.Length; ++i)
            if (TileID.Sets.Grass[i])
                grasses.Add(i);

        TileObjectData.newTile.AnchorValidTiles = [.. grasses];
        TileObjectData.newTile.RandomStyleRange = 3;
        TileObjectData.newTile.StyleHorizontal = false;

        QuickTile.SetMulti(this, 2, 2, DustID.Grass, SoundID.Grass, true, new Color(143, 21, 193));
        glowTex = ModContent.Request<Texture2D>(Texture + "_Glow");

        RegisterItemDrop(ModContent.ItemType<StargazerPlantItem>());
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Tile t = Framing.GetTileSafely(i, j);
        Vector2 pos = TileHelper.TileCustomPosition(i, j);

        for (int k = 0; k < 2; ++k)
        {
            float sine = MathF.Pow(MathF.Sin(Main.GameUpdateCount * 0.02f + (i + j) * 3) + MathHelper.PiOver2 * k, 2);
            Color light = Lighting.GetColor(i, j);
            Color color = Color.Lerp(Color.LightBlue, light, sine);
            spriteBatch.Draw(glowTex.Value, pos, new Rectangle(t.TileFrameX + (k * 32), t.TileFrameY, 16, 16), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }

    public Vector2[] GetOffsets() => new Vector2[] { new Vector2(16, 16) };
    public bool IsFlower(int i, int j) => true;
    public Vector2[] OffsetAt(int i, int j) => GetOffsets();

    bool IFlowerTile.OnPollenate(int i, int j)
    {
        if (WorldGen.PlayerLOS(i, j))
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                NPC.NewNPC(new EntitySource_TileUpdate(i, j), (i + 1) * 16, (j + 1) * 16, ModContent.NPCType<LunarJelly>());
        }

        return true;
    }
}