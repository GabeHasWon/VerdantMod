using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Misc;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Equipables;

internal class VineHook : ApotheoticItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.AmethystHook);
        Item.shootSpeed = 14f;
        Item.shoot = ModContent.ProjectileType<VineHookProjectile>();
        Item.rare = ItemRarityID.Purple;
        Item.value = Item.buyPrice(0, 5, 0, 0);
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(VineHook))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
        {
            ApotheosisDialogueCache.Chat("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.VineHook", true);
            return null;
        }
        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.VineHook", true);
    }
}