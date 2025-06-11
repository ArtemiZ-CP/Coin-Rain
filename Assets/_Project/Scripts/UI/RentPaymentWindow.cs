using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RentPaymentWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _rentText;
    [SerializeField] private TMP_Text _buttonText;
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

    public void Show(int rent)
    {
        if (PlayerCoinsData.Coins < rent)
        {
            _rentText.text = $"You don't have enough coins to pay the rent. You need {rent} coins.";
            _buttonText.text = "End game";
            _rent = -1;
        }
        else
        {
            _rentText.text = $"Rent: {rent}";
            _buttonText.text = "Pay";
            _rent = rent;
        }

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
        if (_rent < 0)
        {
            PlayerData.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (PlayerCoinsData.TryToBuy(_rent))
        {
            OnPaid?.Invoke();
        }
    }
}
