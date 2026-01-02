using Microsoft.Xna.Framework;
using NPCUtils;
using System;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;

namespace Verdant.NPCs.Passive;

public class LunarJelly : ModNPC
{
    private ref float Timer => ref NPC.ai[0];
    private ref float RotationCurrent => ref NPC.ai[1];
    private ref float RotationTarget => ref NPC.ai[2];
    private ref float RotationFactor => ref NPC.ai[3];

    private float _velocity = 0;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = 4;
        Main.npcCatchable[Type] = true;

        NPCID.Sets.CountsAsCritter[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.width = 36;
        NPC.height = 26;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.dontTakeDamage = false;
        NPC.value = 0;
        NPC.knockBackResist = 0f;
        NPC.aiStyle = -1;
        NPC.HitSound = SoundID.Critter;
        NPC.DeathSound = SoundID.Critter;
        NPC.catchItem = (short)ModContent.ItemType<LunarJellyItem>();
        NPC.scale = Main.rand.NextFloat(0.9f, 1.1f);

        SpawnModBiomes = [ModContent.GetInstance<Scenes.VerdantBiome>().Type];
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) => bestiaryEntry.AddInfo(this, "");

    public override void AI()
    {
        Lighting.AddLight(NPC.Center, new Vector3(0, 0.05f, 0.2f));
        Timer++;

        if (Timer == 1)
        {
            RotationCurrent = Main.rand.NextFloat(MathHelper.Pi) - MathHelper.PiOver2;
            RotationTarget = RotationCurrent + Main.rand.NextFloat(MathHelper.Pi) - MathHelper.PiOver2;
        }

        if (Timer > 800)
        {
            NPC.Opacity -= 0.01f;

            if (NPC.Opacity <= 0)
            {
                NPC.active = false;
                NPC.netUpdate = true;
            }
        }
        else if (Timer <= 120)
            NPC.Opacity = Timer / 120f; 

        if (NPC.frame.Y / 28 > 1)
        {
            _velocity += 0.08f;

            if ((RotationFactor += 0.03f) >= 1f)
            {
                RotationFactor -= 1;
                RotationCurrent = RotationTarget;
                RotationTarget += Main.rand.NextFloat(MathHelper.Pi) - MathHelper.PiOver2;
                NPC.netUpdate = true;
            }
        }
        else
            _velocity *= 0.92f;

        NPC.rotation = MathHelper.Lerp(RotationCurrent, RotationTarget, RotationFactor);
        NPC.velocity = new Vector2(_velocity, 0).RotatedBy(NPC.rotation - MathHelper.PiOver2);
    }

    public override Color? GetAlpha(Color drawColor) => GetColor(NPC.Center.ToTileCoordinates(), NPC.Opacity);

    public static Color GetColor(Point center, float opacity)
    {
        float sine = MathF.Pow(MathF.Sin(Main.GameUpdateCount * 0.006f * 3), 2);
        Color light = Lighting.GetColor(center);
        return Color.Lerp(light, Color.White, sine * 0.5f + 0.5f) * opacity;
    }

    public override void FindFrame(int frameHeight)
    {
        double frame = (NPC.frameCounter += 0.06) % 4;
        NPC.frame.Y = (int)frame * frameHeight;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (NPC.life <= 0)
        {
            for (int i = 0; i < 15; ++i)
                Dust.NewDust(NPC.Center, 26, 18, DustID.BlueFairy, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
        }
    }
}
