using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Verdant.Items.Verdant.Tools.Paintbrush;

internal class CrudePaintbrushUISystem : ModSystem
{
    private UserInterface brushInterface;

    public static bool PlayerHoldingPaintbrush => Main.LocalPlayer.HeldItem.ModItem is CrudePaintbrush;
    public static CrudePaintbrush PlayersPaintbrush => Main.LocalPlayer.HeldItem.ModItem as CrudePaintbrush;
    public static bool Open => ModContent.GetInstance<CrudePaintbrushUISystem>().brushInterface.CurrentState is not null;

    public static void Toggle()
    {
        var brushInterface = ModContent.GetInstance<CrudePaintbrushUISystem>().brushInterface;

        if (brushInterface.CurrentState is not null)
            brushInterface.SetState(null);
        else
            brushInterface.SetState(new PaintbrushToolsetUI());
    }

    public override void Load() => brushInterface = new UserInterface();

    public override void UpdateUI(GameTime gameTime)
    {
        if (brushInterface?.CurrentState != null)
            brushInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "Verdant: Paintbrush UI",
                delegate
                {
                    if (brushInterface?.CurrentState != null)
                        brushInterface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}

internal class PaintbrushToolsetUI : UIState
{
    public override void OnInitialize()
    {
        Width = StyleDimension.FromPixels(240);
        Height = StyleDimension.FromPixels(60);
        Top = StyleDimension.FromPixelsAndPercent(-120, 0.5f);
        HAlign = 0.5f;

        OnUpdate += PaintbrushToolsetUI_OnUpdate;

        Setup();
    }

    private void PaintbrushToolsetUI_OnUpdate(UIElement affectedElement)
    {
        if (CrudePaintbrushUISystem.Open && (Main.LocalPlayer.controlInv || !CrudePaintbrushUISystem.PlayerHoldingPaintbrush))
        {
            CrudePaintbrushUISystem.Toggle();
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
    }

    private void Setup()
    {
        UIPanel panel = new()
        {
            Width = StyleDimension.FromPercent(1),
            Height = StyleDimension.FromPercent(1),
            BackgroundColor = Color.White * 0.25f
        };

        Append(panel);

        SetupButtons(panel);
    }

    private void SetupButtons(UIPanel panel)
    {
        List<UIColoredImageButton> buttons = new();

        void AddButton(string val, MouseEvent clickEvent, int index) 
        {
            var button = new UIColoredImageButton(ModContent.Request<Texture2D>("Verdant/Items/Verdant/Tools/Paintbrush/Textures/" + val))
            {
                Width = StyleDimension.FromPixels(40),
                Height = StyleDimension.FromPixels(40),
                Top = StyleDimension.FromPixels(0),
                Left = StyleDimension.FromPixels(index * 44),
            };

            button.OnClick += (evt, listener) =>
            {
                clickEvent(evt, listener);
                CrudePaintbrushUISystem.PlayersPaintbrush.SetMode((CrudePaintbrush.PlacementMode)index);

                int ind = 0;

                foreach (var butt in buttons) //nice
                {
                    if ((int)(Main.LocalPlayer.HeldItem.ModItem as CrudePaintbrush).mode == ind)
                        butt.SetColor(Color.White);
                    else
                        butt.SetColor(Color.Gray);

                    ind++;
                }

            };

            if ((int)(Main.LocalPlayer.HeldItem.ModItem as CrudePaintbrush).mode == index)
                button.SetColor(Color.White);
            else
                button.SetColor(Color.Gray);

            panel.Append(button);
            buttons.Add(button);
        }

        AddButton("Brush", ButtonClicked, 0);
        AddButton("Fill", ButtonClicked, 1);
        AddButton("Circle", ButtonClicked, 2);
        AddButton("Square", ButtonClicked, 3);

        var button = new UIColoredImageButton(ModContent.Request<Texture2D>("Verdant/Items/Verdant/Tools/Paintbrush/Textures/Undo"))
        {
            Width = StyleDimension.FromPixels(40),
            Height = StyleDimension.FromPixels(40),
            Top = StyleDimension.FromPixels(0),
            Left = StyleDimension.FromPixels(4 * 44),
        };
        button.OnClick += UndoButtonClick;
        panel.Append(button);
    }

    private void UndoButtonClick(UIMouseEvent evt, UIElement listeningElement) => CrudePaintbrushUISystem.PlayersPaintbrush.Undo(Main.LocalPlayer);
    private void ButtonClicked(UIMouseEvent evt, UIElement listeningElement) => SoundEngine.PlaySound(SoundID.MenuOpen);
}