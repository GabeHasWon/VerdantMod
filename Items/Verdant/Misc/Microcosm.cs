using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Projectiles.Misc;
using Verdant.World;
using Verdant.World.RealtimeGeneration;

namespace Verdant.Items.Verdant.Misc
{
    class Microcosm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Microcosm");
            Tooltip.SetDefault("Creates a miniscule Verdant biome anywhere");
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 50);
            Item.consumable = true;
            Item.width = 24;
            Item.height = 34;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool CanUseItem(Player player) => !ModContent.GetInstance<VerdantSystem>().microcosmUsed;

        public override bool? UseItem(Player player)
        {
            //ModContent.GetInstance<VerdantSystem>().microcosmUsed = true;

            var gen = ModContent.GetInstance<RealtimeGen>();
            gen.CurrentAction = new(MicroVerdantGen.MicroVerdant(), 1);
            return true;
        }
    }
}