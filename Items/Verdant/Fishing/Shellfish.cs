using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Fishing
{
    [Sacrifice(1)]
    public class Shellfish : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.maxStack = 1;
            Item.questItem = true;
            Item.uniqueStack = true;
            Item.rare = ItemRarityID.Quest;
        }

        public override bool IsQuestFish() => true;
        public override bool IsAnglerQuestAvailable() => true;

        public override void AnglerQuestChat(ref string description, ref string catchLocation)
        {
            description = "Oh hey there minio- uh, I mean, friend. I've been hearing that there's this cool, like, shellfish in the growy caves. I want one. Go get me one!";
            catchLocation = "Caught in the Verdant.";
        }
    }
}
