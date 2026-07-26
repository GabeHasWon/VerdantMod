using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace Verdant.Effects;

public class EffectIDs : ILoadable
{
    //Screen shaders
    public const string BiomeSteam = "Verdant:SteamForeground";
    public const string RainSteam = "Verdant:RainSteam";

    //Normal shaders
    public const string TextWobble = "Verdant/Effects/Text/TextWobble";

    public static readonly Asset<Effect> TextWobbleEffect = ModContent.Request<Effect>(TextWobble);

    void ILoadable.Load(Mod mod)
    {
    }

    void ILoadable.Unload()
    {
    }
}
