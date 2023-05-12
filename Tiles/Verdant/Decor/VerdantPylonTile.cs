﻿using Terraria;
using Terraria.GameContent;
using Verdant.Items.Verdant.Blocks;

namespace Verdant.Tiles.Verdant.Decor
{
	internal class VerdantPylonTile : SimplePylonTile<VerdantPylonItem>
	{
		internal override string MapKeyName => "Mods.Verdant.MapObject.VerdantPylonTile";

		public override bool ValidTeleportCheck_BiomeRequirements(TeleportPylonInfo pylonInfo, SceneMetrics sceneData) => VerdantSystem.InVerdant;
		public override bool IsSold(int npcType, Player player, bool npcHappyEnough) => npcHappyEnough && VerdantSystem.InVerdant;
	}
}