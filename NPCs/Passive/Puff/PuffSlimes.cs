using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Critter;
using Verdant.Tiles.Verdant.Basic.Blocks;

namespace Verdant.NPCs.Passive.Puff;

public class PuffSlimeSmall : ModNPC
{
    public static Asset<Texture2D> _eyeTex;

    private float _rightBlink = 0;
    private float _leftBlink = 0;

    public override void SetStaticDefaults()
    {
        Main.npcCatchable[Type] = true;
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];

        NPCID.Sets.CountsAsCritter[Type] = true;

        _eyeTex = ModContent.Request<Texture2D>("Verdant/NPCs/Passive/Puff/PuffSlime_Eye");
    }

    public override void SetDefaults()
    {
        NPC.width = 16;
        NPC.height = 12;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.knockBackResist = 1.5f;
        NPC.noGravity = false;
        NPC.value = 0f;
        NPC.aiStyle = NPCAIStyleID.Slime;
        NPC.dontCountMe = true;
        NPC.catchItem = (short)ModContent.ItemType<PuffSlimeSmallItem>();
        NPC.HitSound = new SoundStyle("Verdant/Sounds/CloudLand") with { PitchVariance = 0.05f };

        AnimationType = NPCID.BlueSlime;
        AIType = NPCID.Crimslime;
        SpawnModBiomes = new int[1] { ModContent.GetInstance<Scenes.VerdantBiome>().Type };
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            new FlavorTextBestiaryInfoElement("A typical Slime, albeit significantly more friendly, curious, and fluffy!...and perhaps a little bit hyperactive, too."),
        });
    }

    public override void PostAI() => NPC.color = Color.White;

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (NPC.life <= 0)
        {
            for (int i = 0; i < 6; ++i)
                Dust.NewDust(NPC.Center, 26, 18, DustID.PinkStarfish, Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        bool valid = spawnInfo.Player.GetModPlayer<VerdantPlayer>().ZoneVerdant && VerdantGrassLeaves.CheckPuffMicrobiome(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY);
        return (valid ? 1.25f : 0f) * (spawnInfo.PlayerInTown ? 1.75f : 1f);
    }

    public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        _leftBlink += 0.15f + Main.rand.NextFloat(0.25f);
        _rightBlink += 0.15f + Main.rand.NextFloat(0.25f);

        bool left = NPC.direction == -1;

        DrawSingleEye(spriteBatch, new Vector2(left ? 0 : 2, 4), screenPos, drawColor, _leftBlink);
        DrawSingleEye(spriteBatch, new Vector2(NPC.width - (left ? 6 : 4), 4), screenPos, drawColor, _rightBlink);
    }

    private void DrawSingleEye(SpriteBatch spriteBatch, Vector2 offset, Vector2 screenPos, Color drawColor, float blinkTimer)
    {
        Texture2D tex = _eyeTex.Value;
        bool blink = blinkTimer % 70 > 62;
        Vector2 position = NPC.position + offset - screenPos;
        Color color = NPC.IsABestiaryIconDummy ? Color.White : Lighting.GetColor(NPC.Center.ToTileCoordinates(), drawColor);
        Rectangle source = new(0, 0, 4, 4);

        if (blink)
            source = new Rectangle(10, 0, 4, 4);

        spriteBatch.Draw(tex, position, source, color);

        if (!blink)
        {
            bool playerBelow = Main.player[NPC.target].Center.Y > NPC.Center.Y;
            bool playerRight = Main.player[NPC.target].Center.X > NPC.Center.X;

            if (playerBelow)
                position.Y += 2;

            if (playerRight)
                position.X += 2;

            source = new Rectangle(6, 0, 2, 2);
            spriteBatch.Draw(tex, position, source, color);
        }
    }
}