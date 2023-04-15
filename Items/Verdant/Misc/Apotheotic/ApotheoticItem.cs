using Terraria.ModLoader;
using Verdant.Systems.ScreenText;
using Verdant.Systems.ScreenText.Caches;

namespace Verdant.Items.Verdant.Misc.Apotheotic;

public abstract class ApotheoticItem : ModItem, IDialogueCache
{
    public abstract ScreenText Dialogue(bool forServer);
}
