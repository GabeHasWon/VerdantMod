using Microsoft.Xna.Framework;
using NPCUtils;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.TearRain;

namespace Verdant.NPCs.Passive.Snails;

[AutoloadCritter]
public class Dropsnail : ModNPC
{
    public enum Direction : byte
    {
        Left,
        Right,
        Up,
        Down,
        Fall,
    }

    private Direction State
    {
        get => (Direction)NPC.ai[0];
        set => NPC.ai[0] = (float)value;
    }

    private Direction Anchor
    {
        get => (Direction)NPC.ai[1];
        set => NPC.ai[1] = (float)value;
    }

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 4;
        NPCID.Sets.CountsAsCritter[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Snail);
        NPC.width = 18;
        NPC.height = 18;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.value = 0f;
        NPC.knockBackResist = 0f;
        NPC.dontCountMe = true;
        //NPC.aiStyle = -1;
        NPC.noGravity = true;

        SpawnModBiomes = [ModContent.GetInstance<Scenes.VerdantBiome>().Type];
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.AddInfo(this, "");

    public override bool PreAI()
    {
        return true;
    }

    public override void AI()
    {
        return;
        if (NPC.direction == 0)
        {
            State = Main.rand.NextBool() ? Direction.Left : Direction.Right;
            NPC.direction = State == Direction.Left ? -1 : 1;
            Anchor = Direction.Down;
        }

        if (State == Direction.Left)
        {
            NPC.velocity = new Vector2(-1, 0);
            NPC.rotation = 0;
            NPC.direction = -1;

            if (Anchor == Direction.Down)
                CheckDirectionalTravel((Direction.Up, Direction.Left), (Direction.Down, Direction.Right));
            else if (Anchor == Direction.Up)
                CheckDirectionalTravel((Direction.Down, Direction.Left), (Direction.Up, Direction.Right));
        }
        else if (State == Direction.Right)
        {
            NPC.velocity = new Vector2(1, 0);
            NPC.rotation = 0;
            NPC.direction = 1;

            if (Anchor == Direction.Down)
                CheckDirectionalTravel((Direction.Up, Direction.Right), (Direction.Down, Direction.Left));
            else if (Anchor == Direction.Up)
                CheckDirectionalTravel((Direction.Down, Direction.Right), (Direction.Up, Direction.Left));
        }
        else if (State == Direction.Up)
        {
            NPC.velocity = new Vector2(0, -1);

            if (Anchor == Direction.Right)
                CheckDirectionalTravel((Direction.Left, Direction.Up), (Direction.Right, Direction.Down));
            else if (Anchor == Direction.Left)
                CheckDirectionalTravel((Direction.Right, Direction.Up), (Direction.Left, Direction.Down));
        }
        else if (State == Direction.Down)
        {
            NPC.velocity = new Vector2(0, 1);

            if (Anchor == Direction.Right)
                CheckDirectionalTravel((Direction.Left, Direction.Down), (Direction.Right, Direction.Up));
            else if (Anchor == Direction.Left)
                CheckDirectionalTravel((Direction.Right, Direction.Down), (Direction.Left, Direction.Up));
        }
        else if (State == Direction.Fall)
        {
            NPC.noGravity = false;

            if (SolidCollision(NPC.BottomLeft, NPC.width, 4, out bool fall) && !fall)
            {
                SetState(Main.rand.NextBool() ? Direction.Left : Direction.Right, Direction.Down);
                NPC.noGravity = true;
            }
            else
                NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, NPC.direction, 0.05f);
        }

        NPC.spriteDirection = NPC.direction;
        Main.NewText(State);
    }

    private void SetState(Direction state, Direction anchor)
    {
        State = state;
        Anchor = anchor;
    }

    private void CheckDirectionalTravel((Direction direction, Direction anchor) block, (Direction direction, Direction anchor) open)
    {
        Vector2 checkPos = NPC.position + NPC.velocity * 2;

        if (SolidCollision(checkPos, NPC.width, NPC.height, out bool fall) && !fall)
        {
            SetState(block.direction, block.anchor);
            return;
        }

        if (fall)
        {
            State = Direction.Fall;
            Anchor = Direction.Down;
            NPC.velocity = new Vector2(0, 0.2f);
        }

        Vector2 downVector = Anchor switch
        {
            Direction.Left => new(-1, 0),
            Direction.Right => new(1, 0),
            Direction.Up => new(0, -1),
            Direction.Down => new Vector2(0, 1),
            _ => throw null,
        };

        checkPos = NPC.position + downVector * 2;

        if (SolidCollision(checkPos, NPC.width, NPC.height, out fall) && !fall)
            SetState(open.direction, open.anchor);

        if (fall)
        {
            State = Direction.Fall;
            Anchor = Direction.Down;
            NPC.velocity = new Vector2(0, 0.2f);
        }
    }

    public static bool SolidCollision(Vector2 Position, int Width, int Height, out bool fall)
    {
        int value = (int)(Position.X / 16f) - 1;
        int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
        int value3 = (int)(Position.Y / 16f) - 1;
        int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
        int num = Utils.Clamp(value, 0, Main.maxTilesX - 1);
        value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
        value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
        value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
        Vector2 vector = default;
        bool singleAnchor = true;

        fall = false;

        for (int i = num; i < value2; i++)
        {
            for (int j = value3; j < value4; j++)
            {
                Tile tile = Main.tile[i, j];
                if (!tile.IsActuated && tile.HasTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType])
                {
                    if (singleAnchor && tile.Slope != SlopeType.Solid)
                        fall = true;
                    else if (fall && tile.Slope == SlopeType.Solid)
                        fall = false;

                    vector.X = i * 16;
                    vector.Y = j * 16;
                    int num2 = 16;

                    if (tile.IsHalfBlock)
                    {
                        vector.Y += 8f;
                        num2 -= 8;
                    }

                    if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num2)
                        return true;
                }
            }
        }

        return false;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (NPC.life <= 0)
        {
            if (Main.netMode != NetmodeID.Server)
                for (int i = 0; i < 3; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)), Vector2.Zero, Mod.Find<ModGore>("LushLeaf").Type);

            for (int i = 0; i < 4; ++i)
                Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.Grass, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (!TearRainSystem.AnyRainingAt(spawnInfo.SpawnTileY))
            return 0;

        if (spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && spawnInfo.PlayerInTown)
            return 1.2f;

        return spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant ? 0.5f : 0f;
    }
}
