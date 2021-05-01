using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Fishing
{
    public class Shellfish : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overgrown Shellfish");
        }

        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 44;
            item.maxStack = 1;
            item.questItem = true;
            item.uniqueStack = true;
            item.rare = ItemRarityID.Quest;
        }

        public override bool IsQuestFish() => true;

        public override bool IsAnglerQuestAvailable() => true;

        public override void AnglerQuestChat(ref string description, ref string catchLocation)
        {
            description = "Oh hey there minio- uh, I mean, friend. I've been hearing that there's this cool, like, shellfish in the growy caves. Go get me one!";
            catchLocation = "Caught in the Verdant.";
        }
    }
}
