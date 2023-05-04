using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Materials;

class PuffMaterial : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetMaterial(this, 28, 26, ItemRarityID.Blue, 999, false, Item.buyPrice(0, 0, 0, 10));
    public override void SetStaticDefaults() => QuickItem.SetStatic(this, "Puff", "Light and soft to the touch\nTastes bad");
    public override void Update(ref float gravity, ref float maxFallSpeed) => maxFallSpeed = 0.8f;

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(PuffMaterial))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.PuffMaterial", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.PuffMaterial", 100, true);
    }
}