using TMPro;
using UnityEngine;

public class RentView : MonoBehaviour
{
    [SerializeField] private TMP_Text _rentText;
    [SerializeField] private string _rentTextFormat = "Rent: {0}";

    private void OnEnable()
    {
        UpdateRentText();
        PlayerRentData.OnRentCostChanged += UpdateRentText;
    }

    private void OnDisable()
    {
        PlayerRentData.OnRentCostChanged -= UpdateRentText;
    }

    private void UpdateRentText()
    {
        _rentText.text = string.Format(_rentTextFormat, PlayerRentData.RentCost);
    }
}
