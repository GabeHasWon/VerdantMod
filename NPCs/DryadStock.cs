using StockableShops.Stock;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Verdant.Items.Verdant.Blocks.Misc;
using Verdant.Items.Verdant.Blocks.Misc.Books;

namespace Verdant.NPCs;

internal class DryadStock : StockedShop
{
    public override int NPCType => NPCID.Dryad;
    public override string RestockCondition => Language.GetTextValue("Mods.Verdant.DryadShop.Restock");

    private int _dayCounter = 5;
    private bool _wasNight = false;
    private bool _stockRotation = false;

    public override void SetupStock(NPC npc)
    {
        FullStock.Add(new ShopItem(Condition.DownedEyeOfCthulhu, new Item(ModContent.ItemType<GreenCrystalItem>(), 5) { shopCustomPrice = Item.buyPrice(0, 0, 25, 0) }));

        var condition = Language.GetText("Mods.Verdant.DryadShop.Condition");

        FullStock.Add(new ShopItem(new Condition(condition, () => _stockRotation), 
            new Item(ModContent.ItemType<HardyVineBook>()) { shopCustomPrice = Item.buyPrice(0, 15, 0, 0) }));
        FullStock.Add(new ShopItem(new Condition(condition, () => _stockRotation),
            new Item(ModContent.ItemType<LeafBook>()) { shopCustomPrice = Item.buyPrice(0, 15, 0, 0) }));
        FullStock.Add(new ShopItem(new Condition(condition, () => !_stockRotation),
            new Item(ModContent.ItemType<LightbulbBook>()) { shopCustomPrice = Item.buyPrice(0, 15, 0, 0) }));
        FullStock.Add(new ShopItem(new Condition(condition, () => !_stockRotation),
            new Item(ModContent.ItemType<RockBook>()) { shopCustomPrice = Item.buyPrice(0, 8, 0, 0) }));

        _dayCounter = Main.LocalPlayer.GetModPlayer<VerdantPlayer>().ZoneVerdant ? 2 : 0;
        _stockRotation = !_stockRotation;
    }

    public override bool ShouldRestockShop() => _dayCounter > 4;

    public override void Update()
    {
        if (!Main.dayTime)
            _wasNight = true;

        if (Main.dayTime && _wasNight)
        {
            _dayCounter++;
            _wasNight = false;
        }
    }
}
