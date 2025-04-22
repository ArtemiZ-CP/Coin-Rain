using TMPro;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    [SerializeField] private int _multiplier;
    [SerializeField] private TMP_Text _text;

    public int Multiplier => _multiplier;

    private void OnValidate()
    {
        _text.text =  $"x{_multiplier}";
    }

    public void SetMultiplier(int multiplier)
    {
        _multiplier = multiplier;
        _text.text =  $"x{_multiplier}";
    }
}
