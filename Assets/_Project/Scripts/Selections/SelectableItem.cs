using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableItem : MonoBehaviour
{
    [SerializeField] private Button _selectButton;
    [SerializeField] private TMP_Text _priceText;

    private Item _item;

    public event System.Action<SelectableItem> OnItemSelected;

    private void OnEnable()
    {
        _selectButton.onClick.AddListener(OnSelectButtonClicked);
        PlayerCoinsData.OnCurrencyChanged += UpdatePriceText;
    }

    private void OnDisable()
    {
        _selectButton.onClick.RemoveListener(OnSelectButtonClicked);
        PlayerCoinsData.OnCurrencyChanged -= UpdatePriceText;
    }

    public void Initialize(Item item)
    {
        _item = item;
        HidePriceText();
    }

    private void OnSelectButtonClicked()
    {
        // Handle the selection logic here
        Debug.Log($"{gameObject.name} selected.");
    }

    private void UpdatePriceText()
    {
        _priceText.gameObject.SetActive(true);
        Color textColor = _item.Price > PlayerCoinsData.Coins ? Color.red : Color.white;
        _priceText.color = textColor;
        _priceText.text = $"{_item.Price} Coins";
    }

    private void HidePriceText()
    {
        _priceText.gameObject.SetActive(false);
    }
}
