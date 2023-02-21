using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Linq;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Verdant.Systems.UI;

internal class BookState : UIState
{
    private readonly string _title = "";
    private readonly object[] _body = Array.Empty<object>();
    private readonly float _titleScale = 1f;

    public BookState(string title, float titleScale, object[] body)
    {
        _title = title;
        _titleScale = titleScale;
        _body = body;

        if (ModContent.GetInstance<VerdantClientConfig>().FancyBookUI)
            BuildClassic();

        if (_body.Any(x => x is not Asset<Texture2D> && x is not string))
            throw new ArgumentException("Body contains invalid types!");
    }

    private void BuildClassic()
    {
        UIPanel panel = new UIPanel()
        {
            Width = StyleDimension.FromPercent(1 / 3.5f),
            Height = StyleDimension.FromPercent(1 / 1.5f),
            VAlign = 0.5f,
            HAlign = 0.5f
        };
        Append(panel);

        UIText text = new UIText(_title, _titleScale, true)
        {
            DynamicallyScaleDownToWidth = true,
            Width = StyleDimension.FromPixelsAndPercent(-8, 1),
            Height = StyleDimension.FromPixels(40),
            Left = StyleDimension.FromPixels(4),
            Top = StyleDimension.FromPixels(4)
        };
        panel.Append(text);

        BuildClassicListPanel(panel);
    }

    private void BuildClassicListPanel(UIPanel panel)
    {
        UIPanel listPanel = new UIPanel()
        {
            Width = StyleDimension.FromPixelsAndPercent(-8, 1),
            Height = StyleDimension.FromPixelsAndPercent(-48, 1),
            Left = StyleDimension.FromPixels(4),
            Top = StyleDimension.FromPixels(48)
        };
        panel.Append(listPanel);

        UIList list = new UIList()
        {
            Width = StyleDimension.FromPercent(0.95f),
            Height = StyleDimension.Fill,
            ListPadding = 8f,
        };
        listPanel.Append(list);

        UIScrollbar scroll = new UIScrollbar() //Scrollbar for above list
        {
            HAlign = 1f,
            Width = StyleDimension.FromPixels(32),
            Height = StyleDimension.FromPixelsAndPercent(-8, 1f),
            Top = StyleDimension.FromPixels(4)
        };

        list.SetScrollbar(scroll);
        listPanel.Append(scroll);

        foreach (var item in _body)
        {
            if (item is Asset<Texture2D> texture)
            {
                list.Add(new UIImage(texture)
                {
                    Width = StyleDimension.Fill,
                    Height = StyleDimension.FromPixels(texture.Value.Height - 8),
                    Left = StyleDimension.FromPixelsAndPercent(texture.Value.Width / -2, 0.5f),
                });
            }
            else if (item is string text)
            {
                DynamicSpriteFont dynamicSpriteFont = FontAssets.MouseText.Value;
                string visibleText = dynamicSpriteFont.CreateWrappedText(text, GetInnerDimensions().Width);

                Vector2 stringSize = ChatManager.GetStringSize(dynamicSpriteFont, visibleText, new Vector2(1f));

                list.Add(new UIText(text)
                {
                    Width = StyleDimension.Fill,
                    Height = StyleDimension.FromPixels(stringSize.Y + 28)
                });
            }
        }
    }
}
