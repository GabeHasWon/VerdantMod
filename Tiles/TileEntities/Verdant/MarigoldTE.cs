using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Verdant.Drawing;
using Verdant.Tiles.Verdant.Basic.Plants;

namespace Verdant.Tiles.TileEntities.Verdant;

internal class MarigoldTE : DrawableTE, IDrawAdditive
{
    public const int CoinTimeMax = 45;
    public const int SpawnCoinTime = 10;

    protected override Point Size => new(32, 32);

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
        _rotate = MathF.Sin(_timer * 0.02f) * 0.5f * Main.instance.TilesRenderer.GetWindCycle(Position.X, Position.Y, Main.windCounter);

        if (coinTimes.Count > 0)
        {
            for (int i = 0; i < coinTimes.Count; i++)
            {
                coinTimes[i]--;

                if (coinTimes[i] == SpawnCoinTime)
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

        SoundEngine.PlaySound(SoundID.CoinPickup, Position.ToWorldCoordinates());
    }

    internal override void Draw(SpriteBatch spriteBatch)
    {
        Texture2D tex = ModContent.Request<Texture2D>(GetType().FullName.Replace('.', '/')).Value;
        var drawPos = Position.ToWorldCoordinates() - Main.screenPosition + new Vector2(8, 28);
        var effect = Position.X % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        spriteBatch.Draw(tex, drawPos, new Rectangle(0, 0, 32, 32), Lighting.GetColor(Position.ToPoint()), 0, new Vector2(16, 32), 1f, effect, 0f);
        spriteBatch.Draw(tex, drawPos, new Rectangle(32, 0, 32, 32), Lighting.GetColor(Position.ToPoint()), _rotate, new Vector2(16, 32), 1f, effect, 0f);
    }

    void IDrawAdditive.DrawAdditive(AdditiveLayer layer)
    {
        if (coinTimes.Count == 0)
            return;

        Texture2D tex = Mod.Assets.Request<Texture2D>("Textures/Circle").Value;
        Texture2D flower = ModContent.Request<Texture2D>(GetType().FullName.Replace('.', '/')).Value;

        var drawPos = Position.ToWorldCoordinates() - Main.screenPosition + new Vector2(-16, 0);
        var effect = Position.X % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        for (int i = 0; i < coinTimes.Count; ++i)
        {
            float factor = 1 - ((coinTimes[i] - SpawnCoinTime) / (float)(CoinTimeMax - SpawnCoinTime));
            if (coinTimes[i] <= SpawnCoinTime)
                factor = coinTimes[i] / (float)SpawnCoinTime;

            Main.spriteBatch.Draw(flower, drawPos + new Vector2(24, 28), new Rectangle(32, 0, 32, 32), Color.Gold * factor * 0.75f, _rotate, new Vector2(16, 32), 1f + (factor * 0.5f), effect, 0f);
            Main.spriteBatch.Draw(tex, drawPos, null, Color.LightGoldenrodYellow * factor * 0.45f, _rotate * 0.25f, new Vector2(16, 32), 0.45f + (factor * 0.4f), effect, 0f);
        }
    }
}
