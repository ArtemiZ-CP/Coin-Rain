using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _messageText;
    [SerializeField] private Button _closeButton;

    public event Action OnClosed;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    public void Show(float rentCost)
    {
        _messageText.text = $"At the end of this round, you must pay {rentCost} coins.";
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        OnClosed?.Invoke();
    }

    private void Close()
    {
        OnClosed?.Invoke();
    }
}
