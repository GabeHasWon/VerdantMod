using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.IO;

namespace Verdant.Backgrounds.BGItem.Verdant
{
    public class LushBushBG : BaseBGItem
    {
        public override bool SaveMe => true;

        public LushBushBG(Vector2 pos) : base(pos, 1f, new Point(36, 43))
        {
            tex = Terraria.ModLoader.ModContent.GetTexture("Verdant/Backgrounds/BGItem/Verdant/LushBushBG");
        }

        public LushBushBG() { tex = Terraria.ModLoader.ModContent.GetTexture("Verdant/Backgrounds/BGItem/Verdant/LushBushBG"); }

        internal override void Behaviour()
        {
            base.Behaviour();
            BaseParallax(0);
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound
            {
                { "Position", position }
            };
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            position = tag.Get<Vector2>("Position");
            DrawPosition = position;
            scale = 1f;
            source = new Rectangle(0, 0, 36, 43);
        }

        internal override void Draw(Vector2 off)
        {
            drawColor = Main.bgColor;
            base.Draw(GetParallax());
        }
    }
}
