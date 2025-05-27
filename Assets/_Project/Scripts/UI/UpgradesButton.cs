using UnityEngine;

public class UpgradesButton : MonoBehaviour
{
    [SerializeField] private GameObject _upgradesButton;

    private void OnEnable()
    {
        PlayerMapUpgradesData.OnUpgradePin += OnBaseBinUpgradeActive;
        
        _upgradesButton.SetActive(PlayerMapUpgradesData.IsUpgradeUnlocked);
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
