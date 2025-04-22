using TMPro;
using UnityEngine;

public class CoinsDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        PlayerData.OnCoinsChanged += UpdateCoinsText;
    }

    private void OnDisable()
    {
        PlayerData.OnCoinsChanged -= UpdateCoinsText;
    }

    private void UpdateCoinsText()
    {
        _text.text = $"{PlayerData.Coins} Coins";
    }
}
