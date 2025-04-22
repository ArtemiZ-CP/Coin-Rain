using TMPro;
using UnityEngine;

public class BallsCoinsDisplayer : MonoBehaviour
{
    [SerializeField] private PlayerBall _ball;
    [SerializeField] private TMP_Text _coinsText;

    private void OnEnable()
    {
        _ball.OnCoinsChanged += UpdateCoins;
    }

    private void OnDisable()
    {
        _ball.OnCoinsChanged -= UpdateCoins;
    }

    private void UpdateCoins(float coins)
    {
        _coinsText.text = coins.ToString("0.##");
    }
}
