using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Verdant.Systems.UI;

internal class UISystem : ModSystem
{
    internal UserInterface BookInterface;

    public override void Load()
    {
        On.Terraria.Player.ToggleInv += Player_ToggleInv;

        if (!Main.dedServ)
            BookInterface = new UserInterface();
    }

    public override void UpdateUI(GameTime gameTime) => BookInterface?.Update(gameTime);

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));

        if (inventoryIndex != -1)
        {
            layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer(
                "Verdant: BookUI",
                delegate
                {
                    BookInterface.Draw(Main.spriteBatch, Main.gameTimeCache);
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }

    private static void Player_ToggleInv(On.Terraria.Player.orig_ToggleInv orig, Player self)
    {
        var inter = ModContent.GetInstance<UISystem>().BookInterface;

        if (inter.CurrentState is not null)
        {
            inter.SetState(null);
            SoundEngine.PlaySound(SoundID.MenuClose);
            return;
        }

        orig(self);
    }
}
