using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Systems.Foreground;
using Verdant.Systems.Foreground.Parallax;
using Verdant.Items.Verdant.Materials;
using Verdant.Systems.ScreenText.Caches;
using Verdant.Systems.ScreenText;

namespace Verdant.Items.Verdant.Tools;

[Sacrifice(1)]
class CloudsproutPuff : ApotheoticItem
{
    public override void SetDefaults() => QuickItem.SetStaff(this, 40, 20, ProjectileID.GolemFist, 9, 0, 24, 0, 0, ItemRarityID.Green);
    public override void AddRecipes() => QuickItem.AddRecipe(this, TileID.LivingLoom, 1, (ModContent.ItemType<YellowBulb>(), 1), (ModContent.ItemType<PuffMaterial>(), 14));

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) => position = Main.MouseWorld;

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        foreach (var item in ForegroundManager.PlayerLayerItems)
        {
            if (item is CloudbloomEntity && Vector2.DistanceSquared(item.Center, Main.MouseWorld) < 40 * 40)
            {
                item.killMe = true;
                return false;
            }
        }

        ForegroundManager.AddItem(new CloudbloomEntity(Main.MouseWorld, true), true, true);
        return false;
    }

    int rightClickTimer = 0;

    public override void HoldItem(Player player)
    {
        if (Main.mouseRight)
        {
            rightClickTimer++;

            if (rightClickTimer > 120)
            {
                YellowPetalFloater.ClearAll(true);
                rightClickTimer = 0;
            }
        }
        else
            rightClickTimer = 0;
    }

    [DialogueCacheKey(nameof(ApotheoticItem) + "." + nameof(CloudsproutPuff))]
    public override ScreenText Dialogue(bool forServer)
    {
        if (forServer)
            return null;

        if (!ModContent.GetInstance<VerdantClientConfig>().CustomDialogue)
            return ApotheosisDialogueCache.ChatLength("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Bouncebloom.", 2, true);

        return ApotheosisDialogueCache.StartLine("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Bouncebloom.0").
            FinishWith(new ScreenText("$Mods.Verdant.ScreenText.Apotheosis.ItemInteractions.Bouncebloom.1"));
    }
}
