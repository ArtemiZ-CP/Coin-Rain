using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableItem : MonoBehaviour
{
    [SerializeField] private Button _selectButton;
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Color _ableToBuyColor = Color.white;
    [SerializeField] private Color _notAbleToBuyColor = Color.red;

    private Item _item;
    private bool _haveToBuy;

    public event System.Action OnItemSelected;

    private void OnEnable()
    {
        // _selectButton.onClick.AddListener(OnSelectButtonClicked);
        // PlayerCoinsData.OnCurrencyChanged += UpdatePriceText;
    }

    private void OnDisable()
    {
        // _selectButton.onClick.RemoveListener(OnSelectButtonClicked);
        // PlayerCoinsData.OnCurrencyChanged -= UpdatePriceText;
    }

    public void Initialize(Item item, bool haveToBuy)
    {
        _item = item;
        _haveToBuy = haveToBuy;
        _itemImage.sprite = _item.ItemSprite;
        _itemNameText.text = _item.Name;

        // UpdatePriceText();
    }

    private void OnSelectButtonClicked()
    {
        // if (_haveToBuy && PlayerCoinsData.TryToBuy(_item.Price) == false)
        // {
        //     return;
        // }

        OnItemSelected?.Invoke();
    }

    // private void UpdatePriceText()
    // {
    //     _priceText.gameObject.SetActive(_haveToBuy);

    //     if (_haveToBuy == false)
    //     {
    //         return;
    //     }

    //     Color textColor = _item.Price > PlayerCoinsData.Coins ? _notAbleToBuyColor : _ableToBuyColor;
    //     _priceText.color = textColor;
    //     _priceText.text = $"{_item.Price} Coins";
    // }
}
