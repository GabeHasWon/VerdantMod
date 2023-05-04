using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Materials;

class Lightbulb : ApotheoticItem
{
    public override void Load() => VerdantPlayer.ItemDrawLayerEvent += PlayerDraw;
    public override void SetDefaults() => QuickItem.SetMaterial(this, 22, 24, ItemRarityID.White, noUseGraphic: true);
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Lightbulb", "'Keeps Charlie away'");
    public override void PostUpdate() => Lighting.AddLight(Item.position, new Vector3(0.1f, 0.03f, 0.06f) * 9);
    public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick) => wetTorch = true;
    public override void HoldItemFrame(Player player) => player.bodyFrame.Y = 56;
    public override void HoldItem(Player player) => Lighting.AddLight(player.MountedCenter - new Vector2(0, 28), new Vector3(0.1f, 0.03f, 0.06f) * 12);

    public void PlayerDraw(PlayerDrawSet info)
    {
        if (info.drawPlayer.HeldItem.type == Item.type && info.drawPlayer.itemAnimation <= 0)
        {
            Texture2D t = Mod.Assets.Request<Texture2D>("Items/Verdant/Materials/Lightbulb").Value;
            Vector2 pos = PlayerHelper.PlayerDrawPositionOffset(info.drawPlayer, new Vector2(0, -52));
            DrawData data = new DrawData(t, pos.Floor(), null, Color.White, 0f, new Vector2(t.Width / 2f, t.Height / 2f), 1f, info.drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            info.DrawDataCache.Add(data);
        }
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(Lightbulb))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Lightbulb.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Lightbulb.0", 80).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Lightbulb.1", 80));
    }
}
