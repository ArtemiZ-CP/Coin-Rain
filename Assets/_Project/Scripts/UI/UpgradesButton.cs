using UnityEngine;

public class UpgradesButton : MonoBehaviour
{
    [SerializeField] private GameObject _upgradesButton;

    private void OnEnable()
    {
        PlayerMapUpgradesData.OnUpgradePin += OnBaseBinUpgradeActive;

        if (PlayerMapUpgradesData.IsUpgradeUnlocked == false)
        {
            _upgradesButton.SetActive(false);
        }
    }

    private void OnDisable()
    {
        PlayerMapUpgradesData.OnUpgradePin -= OnBaseBinUpgradeActive;
    }

    private void OnBaseBinUpgradeActive(Pin.Type upgradeType)
    {
        if (upgradeType == Pin.Type.Base)
        {
            _upgradesButton.SetActive(true);
        }
    }
}
