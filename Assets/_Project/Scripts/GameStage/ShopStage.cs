using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopStage : GameStage
{
    private const int BuyPinsCount = 2;

    [SerializeField] private ItemSelectWindow _itemSelectWindow;
    [SerializeField] private ShopItemsScriptableObject _shopItemsScriptableObject;
    [SerializeField] private PinItemsScriptableObject _pinItemsScriptableObject;
    [SerializeField] private BlessingItemsScriptableObject _blessingItemsScriptableObject;

    private void OnEnable()
    {
        _itemSelectWindow.OnItemSelected += HandleItemSelected;
        _itemSelectWindow.OnWindowClosed += EndStage;
    }

    private void OnDisable()
    {
        _itemSelectWindow.OnItemSelected -= HandleItemSelected;
        _itemSelectWindow.OnWindowClosed -= EndStage;
    }

    public override void StartStage()
    {
        base.StartStage();
        _itemSelectWindow.Initialize(GenerateItems(),
            "Shop", haveToBuy: true, showCloseButton: true);
    }

    public override void EndStage()
    {
        base.EndStage();
        _itemSelectWindow.Hide();
    }

    private List<Item> GenerateItems()
    {
        List<ShopItem> items = _shopItemsScriptableObject.ShopItems.ToList();
        List<Item> generatedItems = new();

        for (int i = 0; i < PlayerShopData.Capacity; i++)
        {
            if (items.Count == 0)
            {
                break;
            }

            ShopItem shopItem = Item.GetRandomItem(items);
            ShopItem item = GenerateItem(shopItem);

            items.Remove(shopItem);

            if (item == null)
            {
                i--;
                continue;
            }

            generatedItems.Add(item);
        }

        return generatedItems;
    }

    private ShopItem GenerateItem(ShopItem shopItem)
    {
        if (shopItem == null)
        {
            return null;
        }

        if (shopItem.ItemType == ShopItem.Type.BuyPin)
        {
            PinItem pinItem = Item.GetRandomItem(_pinItemsScriptableObject.PinItems.ToList());
            return new ShopItem(shopItem.ItemType, pinItem.Type, shopItem.Price * pinItem.Price * BuyPinsCount,
                pinItem.ItemSprite, shopItem.SideSprite, shopItem.ItemRare);
        }
        if (shopItem.ItemType == ShopItem.Type.DestroyPin)
        {
            List<PinItem> receivedItems = _pinItemsScriptableObject.GetReceivedItems();

            if (receivedItems.Count == 0)
            {
                return null;
            }

            PinItem pinItem = Item.GetRandomItem(receivedItems);
            return new ShopItem(shopItem.ItemType, pinItem.Type, shopItem.Price * pinItem.Price,
                pinItem.ItemSprite, shopItem.SideSprite, shopItem.ItemRare);
        }
        if (shopItem.ItemType == ShopItem.Type.UpgradePin)
        {
            PinItem pinItem = Item.GetRandomItem(_pinItemsScriptableObject.GetReceivedItems(includeBasePin: true));
            return new ShopItem(shopItem.ItemType, pinItem.Type, shopItem.Price * pinItem.Price,
                pinItem.ItemSprite, shopItem.SideSprite, shopItem.ItemRare);
        }
        if (shopItem.ItemType == ShopItem.Type.IncreasePinDurability)
        {
            PinItem pinItem = Item.GetRandomItem(_pinItemsScriptableObject.GetReceivedItems(includeBasePin: true));
            return new ShopItem(shopItem.ItemType, pinItem.Type, shopItem.Price * pinItem.Price,
                pinItem.ItemSprite, shopItem.SideSprite, shopItem.ItemRare);
        }
        if (shopItem.ItemType == ShopItem.Type.UpgradeBlessing)
        {
            List<BlessingItem> receivedItems = _blessingItemsScriptableObject.GetReceivedItems();

            if (receivedItems.Count == 0)
            {
                return null;
            }

            BlessingItem blessingItem = Item.GetRandomItem(receivedItems);
            return new ShopItem(shopItem.ItemType, blessingItem.Type, shopItem.Price * blessingItem.Price,
                blessingItem.ItemSprite, shopItem.SideSprite, shopItem.ItemRare);
        }
        if (shopItem.ItemType == ShopItem.Type.IncreaseMapHeight ||
                 shopItem.ItemType == ShopItem.Type.IncreaseMapWidth)
        {
            return new ShopItem(shopItem.ItemType, shopItem.Price, shopItem.ItemSprite,
                shopItem.SideSprite, shopItem.ItemRare);
        }

        return null;
    }

    private void HandleItemSelected(Item item)
    {
        if (IsActive == false)
        {
            return;
        }

        if (item is ShopItem shopItem)
        {
            if (shopItem.ItemType == ShopItem.Type.BuyPin)
            {
                Pin.Get(shopItem.PinType).IncreaseCount(BuyPinsCount);
            }
            else if (shopItem.ItemType == ShopItem.Type.DestroyPin)
            {
                Pin.Get(shopItem.PinType).DecreaseCount();
            }
            else if (shopItem.ItemType == ShopItem.Type.UpgradePin)
            {
                Pin.Get(shopItem.PinType).Upgrade();
            }
            else if (shopItem.ItemType == ShopItem.Type.IncreasePinDurability)
            {
                Pin.Get(shopItem.PinType).IncreaseDurability();
            }
            else if (shopItem.ItemType == ShopItem.Type.UpgradeBlessing)
            {
                Blessing.Get(shopItem.BlessingType).Upgrade();
            }
            else if (shopItem.ItemType == ShopItem.Type.IncreaseMapHeight)
            {
                PlayerMapData.IncreaseHeight();
            }
            else if (shopItem.ItemType == ShopItem.Type.IncreaseMapWidth)
            {
                PlayerMapData.IncreaseWidth();
            }
        }

        EndStage();
    }
}
