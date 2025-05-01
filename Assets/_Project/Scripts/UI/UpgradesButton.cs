using UnityEngine;

public class UpgradesButton : MonoBehaviour
{
    [SerializeField] private GameObject _upgradesButton;

    private void OnEnable()
    {
        PlayerData.OnUpgradeUnlock += OnBaseBinUpgradeActive;

        if (PlayerData.IsUpgradeUnlocked == false)
        {
            _upgradesButton.SetActive(false);
        }
    }

    private void OnDisable()
    {
        PlayerData.OnUpgradeUnlock -= OnBaseBinUpgradeActive;
    }

    private void OnBaseBinUpgradeActive(UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.Base)
        {
            _upgradesButton.SetActive(true);
        }
    }
}
