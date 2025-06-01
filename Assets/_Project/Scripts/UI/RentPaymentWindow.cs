using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RentPaymentWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _rentText;
    [SerializeField] private Button _payButton;

    private int _rent;

    public event System.Action OnPaid;

    private void OnEnable()
    {
        _payButton.onClick.AddListener(Pay);
    }

    private void OnDisable()
    {
        _payButton.onClick.RemoveListener(Pay);
    }

    public void Initialize(int rent)
    {
        _rentText.text = $"Rent: {rent}";
        _rent = rent;
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Pay()
    {
        if (PlayerCoinsData.TryToBuy(_rent))
        {
            OnPaid?.Invoke();
        }
    }
}
