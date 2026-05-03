using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Buffs.Minion;
using Verdant.Projectiles.Minion;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Equipables;

class Propellerpad : ApotheoticItem
{
    public override void SetDefaults()
    {
        QuickItem.SetStaff(this, 48, 48, ModContent.ProjectileType<PropellerpadProjectile>(), 9, 0, 0, 0, 0, ItemRarityID.Green);

        Item.buffType = ModContent.BuffType<PropellerpadBuff>();
        Item.buffTime = 2;
        Item.noUseGraphic = true;
    }

    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<PropellerpadProjectile>()] == 0;

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(Propellerpad))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Propellerpad.", 3, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Propellerpad.0").
            With(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Propellerpad.1")).
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Propellerpad.2"));
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        int index = tooltips.FindIndex(x => x.Name == "BuffTime");

        if (index != -1)
            tooltips.RemoveAt(index);
    }
}
