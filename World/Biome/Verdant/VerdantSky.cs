using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace Verdant.World.Biome.Verdant
{
    public class VerdantSky : CustomSky
    {
        private bool isActive;
        private float _intensity = 0;

        public override void Update(GameTime gameTime)
        {
            if (isActive && _intensity < 0.2f) _intensity += 0.04f;
            else if (!isActive && _intensity > 0f) _intensity -= 0.04f;
        }

        private float GetIntensity() => _intensity;

        public override Color OnTileColor(Color inColor) => new Color(Vector4.Lerp(new Vector4(0.24f, 0.39f, 0.24f, 1f), inColor.ToVector4(), 1f - GetIntensity()));

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                float intensity = GetIntensity();
                spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Green * intensity);
            }
        }

        public override float GetCloudAlpha() => 0f;
        public override void Activate(Vector2 position, params object[] args) => isActive = true;
        public override void Deactivate(params object[] args) => isActive = false;
        public override void Reset() => isActive = false;
        public override bool IsActive() => isActive || _intensity > 0f;
    }

    public class VerdantScreenShaderData : ScreenShaderData
    {
        public VerdantScreenShaderData(string passName) : base(passName)
        {
        }

        public override void Apply()
        {
            UseTargetPosition(Main.LocalPlayer.position);
            base.Apply();
        }
    }
}