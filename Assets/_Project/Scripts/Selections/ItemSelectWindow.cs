using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectWindow : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private SelectableItem _selectableItemPrefab;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Button _closeButton;

    public event System.Action OnItemSelected;
    public event System.Action OnWindowClosed;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(CloseWindow);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(CloseWindow);
    }

    public void Show(List<Item> items, string title, bool haveToBuy, bool showCloseButton)
    {
        Show();
        ClearContent();

        foreach (Item item in items)
        {
            SelectableItem selectableItem = Instantiate(_selectableItemPrefab, _content);
            selectableItem.Initialize(item, haveToBuy);
            selectableItem.OnItemSelected += SelectItem;
        }

        _titleText.text = title;
        _closeButton.gameObject.SetActive(showCloseButton);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void CloseWindow()
    {
        Hide();
        OnWindowClosed?.Invoke();
    }

    private void ClearContent()
    {
        foreach (Transform child in _content)
        {
            if (child.TryGetComponent(out SelectableItem selectableItem))
            {
                selectableItem.OnItemSelected -= SelectItem;
            }

            Destroy(child.gameObject);
        }
    }

    private void SelectItem()
    {
        OnItemSelected?.Invoke();
    }
}
