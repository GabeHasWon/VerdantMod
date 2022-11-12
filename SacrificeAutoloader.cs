using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Verdant.Items.Verdant;

namespace Verdant
{
	/// <summary>Copied from my implementation of the same name in Spirit.</summary>
	public class SacrificeAutoloader
	{
		public static void Load(Mod mod)
		{
			var types = typeof(VerdantMod).Assembly.GetTypes().Where(x => typeof(ModItem).IsAssignableFrom(x) && !x.IsAbstract);
			foreach (var info in types)
			{
				if (!mod.TryFind(info.Name, out ModItem modItem))
					continue;

				int type = modItem.Type;

				if (Attribute.IsDefined(info, typeof(SacrificeAttribute)))
				{
					SacrificeAttribute attr = Attribute.GetCustomAttribute(info, typeof(SacrificeAttribute)) as SacrificeAttribute;
					CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[type] = attr.Count;
					continue;
				}

				Item item = new Item(type);

				bool placesTile = item.createTile >= TileID.Dirt;
				bool placesWall = item.createWall > WallID.None;
				bool isWeapon = item.damage > 0 || item.mana > 0;
				bool accessoryOrArmor = item.accessory || item.defense > 0 || Attribute.IsDefined(info, typeof(AutoloadEquip));

				if (placesTile) //Tiles
					CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[type] = 100;
				else if (placesWall) //Walls
					CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[type] = 400;
				else if (isWeapon) //Weapons
				{
					if (item.consumable) //Consumable weapons
						CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[type] = 99;
					else //Non-consumable weapons
						CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[type] = 1;
				}
				else if (accessoryOrArmor) //Accessories or armor
					CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[type] = 1;
				else //Everything else, namely materials
					CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[type] = 25;
			}
		}
	}
}
