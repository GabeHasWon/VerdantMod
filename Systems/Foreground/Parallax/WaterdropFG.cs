using Microsoft.Xna.Framework;
using Terraria;
using Verdant.Scenes;
using Verdant.Systems.TearRain;

namespace Verdant.Systems.Foreground.Parallax;

public class WaterdropFG : ParallaxedFGItem
{
    private int offscreenTimer = 0;

    public static bool CanSpawn(Player p) => TearRainSystem.Raining && p.InModBiome<VerdantUndergroundBiome>() && Main.rand.NextFloat() < 0.15f + TearRainSystem.RainStrength * 0.4f;

    public WaterdropFG(Vector2 pos) : base(pos, Vector2.Zero, 1f, "Parallax/WaterdropFG")
    {
        parallax = Main.rand.Next(25, 150) * 0.01f;
        scale = parallax * 0.5f + 0.5f;
        velocity = new Vector2(Main.rand.NextFloat(-0.2f, 0.2f), Main.rand.NextFloat(6f, 7.5f) * parallax + 2f);
        source = new Rectangle(11 * Main.rand.Next(2), 0, 11, 30);
    }

    public override void Update()
    {
        base.Update();

        rotation = velocity.X * 0.5f;

        if (!new Rectangle((int)Main.screenPosition.X - 60, (int)Main.screenPosition.Y - 60, Main.screenWidth + 120, Main.screenHeight + 120).Contains(drawPosition.ToPoint()))
            offscreenTimer++;
        else
            offscreenTimer = 0;

        if (offscreenTimer > 600)
            killMe = true;
    }

    public override void Draw()
    {
        drawPosition = position + ParallaxPosition();
        Color lightColour = Lighting.GetColor((int)(drawPosition.X / 16f), (int)(drawPosition.Y / 16f));
        Color frontColour = (position.Y / 16f < Main.worldSurface) ? Main.ColorOfTheSkies : new Color(85, 85, 85);
        drawColor = Color.Lerp(lightColour, frontColour, (parallax - (0.25f)) / 1.25f);

        base.Draw();
    }
}