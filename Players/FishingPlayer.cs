using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Equipables;
using Verdant.Items.Verdant.Fishing;

namespace Verdant.Players;

internal class FishingPlayer : ModPlayer
{
    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        bool inVerdant = Player.GetModPlayer<VerdantPlayer>().ZoneVerdant;

        if (inVerdant && attempt.crate && Main.rand.NextBool(Player.cratePotion ? 3 : 4))
            itemDrop = !Main.hardMode ? ModContent.ItemType<LushWoodCrateItem>() : ModContent.ItemType<MysteriaCrateItem>();

        if (inVerdant && attempt.questFish == ModContent.ItemType<Shellfish>() && Main.rand.NextBool(3))
            itemDrop = ModContent.ItemType<Shellfish>();

        if (inVerdant && Main.hardMode && attempt.legendary && Main.rand.NextBool(6))
            itemDrop = ModContent.ItemType<VineHook>();
    }
}
