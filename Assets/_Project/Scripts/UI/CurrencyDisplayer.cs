using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyDisplayer : MonoBehaviour
{
    private static readonly Dictionary<PlayerBall, float> _temporaryCoins = new();

    [SerializeField] private TMP_Text _diamondsText;
    [SerializeField] private TMP_Text _mainCoinsText;
    [SerializeField] private TMP_Text _temporaryCoinsText;

    public static IReadOnlyDictionary<PlayerBall, float> TemporaryCoins => _temporaryCoins;

    public static event System.Action<float> OnTemporaryCoinsChanged;

    private void Awake()
    {
        ResetTemporaryCoinsText(null);
    }

    private void OnEnable()
    {
        UpdateCurrencyText();
        PlayerCurrencyData.OnCurrencyChanged += UpdateCurrencyText;
        PlayerBall.OnTemporaryCoinsChanged += UpdateTemporaryCoinsText;
        BallsController.OnBallDropped += ResetTemporaryCoinsText;
    }

    private void OnDisable()
    {
        PlayerCurrencyData.OnCurrencyChanged -= UpdateCurrencyText;
        PlayerBall.OnTemporaryCoinsChanged -= UpdateTemporaryCoinsText;
        BallsController.OnBallDropped -= ResetTemporaryCoinsText;
    }

    private void UpdateCurrencyText()
    {
        _mainCoinsText.text = $"{PlayerCurrencyData.Coins:0.#} Coins";
        _diamondsText.text = $"{PlayerCurrencyData.Diamonds} Diamonds";
    }

    private void UpdateTemporaryCoinsText(PlayerBall playerBall, float coins)
    {
        if (_temporaryCoinsText == null)
        {
            return;
        }

        if (_temporaryCoins.ContainsKey(playerBall))
        {
            _temporaryCoins[playerBall] = coins;
        }
        else
        {
            _temporaryCoins.Add(playerBall, coins);
        }

        float totalTemporaryCoins = 0f;

        foreach (var kvp in _temporaryCoins)
        {
            totalTemporaryCoins += kvp.Value;
        }

        _temporaryCoinsText.text = $"+{totalTemporaryCoins:0.#} Coins";
        OnTemporaryCoinsChanged?.Invoke(totalTemporaryCoins);
    }

    private void ResetTemporaryCoinsText(PlayerBall playerBall)
    {
        if (_temporaryCoinsText == null)
        {
            return;
        }
        
        _temporaryCoins.Clear();
        _temporaryCoinsText.text = "+0 Coins";
    }
}
