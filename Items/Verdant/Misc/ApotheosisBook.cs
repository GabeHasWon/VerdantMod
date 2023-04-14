using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Verdant.Items.Verdant.Misc;

public class ApotheosisBook : ModItem
{
	public override void SetStaticDefaults() => QuickItem.SetStatic(this, "'My Apotheosis'", "- Emily");
    public override void SetDefaults() => QuickItem.SetMaterial(this, 36, 46, ItemRarityID.Purple, 1, false, 0, true);
	public override bool AltFunctionUse(Player player) => true;

	public override bool? UseItem(Player player)
	{
		if (player.altFunctionUse == 2 && player.itemAnimation > 14)
		{
			QuickItem.ToggleBookUI("'My Apotheosis'", 0.8f,
				new object[] { "\n\"It's been a while since I've run out of supplies.\nMy caravan is empty aside from research,\nwith mice and bats being all I can eat.\n" +
                "Water is surprisingly clean and fresh\naround here, so...it’s just hunger.\nSome research is done, however.\nI’m categorizing the flora around here, and it's...magical.\n" +
                "To a point where I’m not sure that's simply a term,\n or just the truth.\nSome surprisingly lush grass and foliage around,\nI’ve even seen some minor tree saplings here and there.\n" +
                "A most curious wood, too!\nDespite my starvation, this is a heaven I’ll suffer for.\"\n\n...\n\n" +
                "\"The meager morsels of this land have been sufficient for some\ntime. " +
                "I've set up a small but acceptable study,\nboth to hold my things and to keep myself safe.\nOften an errant skeleton or angry bat finds itself around me,\n" +
                "and I am an explorer; monster slayer naught.\nThat being said, food grows scarcer yet.\nBeyond bats, I’ve lost mice - seems the little critters have\nlearned their biggest threat.\n" +
                "Time lies outside of my reach.\nStudies notwithstanding, I’ve little left anyhow.\"\n\n" +
                "...\n\n\"Time grows weary; as do I.\nStudies continue, fervor reaching new heights\nas I see things unique to this strange cavern.\nBehind the hunger pains, the fatigue;\ndespite my" +
                " dismal state of being,\nI live stronger than I've ever lived.\nTo me, this is life, the creatures around here\nthrive and play and talk and fight.\n" +
                "Perhaps there's even more I can't see.\nAlas, Death comes knocking, and I've no way to close the door.\nBut perhaps, in some other time,\nthese plants find another pair of eyes,\n" +
                "and bewilder as I have been bewildered.\"\n\n...\n\n\"View; a flower. A flower with a bulb in it, a curious one -\nbetwixt the petals, a gentle glow.\nA calming glow.\n" +
                "A glow unlike anything I’ve seen in my time here.\nI’ve all but abandoned my study.\nThere’s no food, and I’m on my last breaths.\nMay someone find my work.\n" +
                "May some passion revive what I’ve died doing.\nMay this journal find someone in good health,\nin good spirits,\nand goodbye.\""});
            return true;
		}

		Item.placeStyle = Main.rand.Next(2) + 4;
		return null;
	}
}
