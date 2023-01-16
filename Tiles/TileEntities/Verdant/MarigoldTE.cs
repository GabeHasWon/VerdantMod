using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Tiles.TileEntities.Verdant;

internal class MarigoldTE : DrawableTE
{
    protected override Point Size => new Point(32, 32);

    internal List<int> coinTimes = new();

    private int _timer = 0;
    private float _rotate = 0;

    public override void SaveData(TagCompound tag) => tag.Add("timer", _timer);
    public override void LoadData(TagCompound tag) => _timer = tag.GetInt("timer");

    public override bool IsTileValidForEntity(int x, int y)
    {
        Tile tile = Main.tile[x, y];
        return tile.HasTile && tile.TileType == ModContent.TileType<MarigoldTile>();
    }

    public override void Update()
    {
        _timer++;
        _rotate = MathF.Sin(_timer * 0.02f) * 0.2f;

        if (coinTimes.Count > 0)
        {
            for (int i = 0; i < coinTimes.Count; i++)
            {
                coinTimes[i]--;

                if (coinTimes[i] == 0)
                    SpawnCoin();
            }

            coinTimes.RemoveAll(x => x <= 0);
        }
    }

    private void SpawnCoin()
    {
        WeightedRandom<int> type = new();
        type.Add(ProjectileID.CopperCoinsFalling, 0.8f);
        type.Add(ProjectileID.SilverCoinsFalling, 0.18f);
        type.Add(ProjectileID.GoldCoinsFalling, 0.02f);

        var vel = new Vector2((Main.rand.NextBool() ? -1 : 1) * Main.rand.NextFloat(-1.5f, 4f), Main.rand.NextFloat(-14, -10));
        int proj = Projectile.NewProjectile(new EntitySource_TileUpdate(Position.X, Position.Y), Position.ToWorldCoordinates(), vel, type, 0, 0, Main.myPlayer);

        if (Main.netMode != NetmodeID.SinglePlayer)
            NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        Texture2D tex = ModContent.Request<Texture2D>(GetType().FullName.Replace('.', '/')).Value;
        var drawPos = Position.ToWorldCoordinates() - Main.screenPosition + new Vector2(8, 28);
        var effect = Position.X % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        spriteBatch.Draw(tex, drawPos, new Rectangle(0, 0, 32, 32), Lighting.GetColor(Position.ToPoint()), 0, new Vector2(16, 32), 1f, effect, 0f);
        spriteBatch.Draw(tex, drawPos, new Rectangle(32, 0, 32, 32), Lighting.GetColor(Position.ToPoint()), _rotate, new Vector2(16, 32), 1f, effect, 0f);
    }
}
