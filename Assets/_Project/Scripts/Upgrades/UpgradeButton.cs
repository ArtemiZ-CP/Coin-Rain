using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradeButton<T> : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private GameObject _nextUpgrade;
    [SerializeField, ReadOnly] private UpgradeDatas _upgrades;

    private float _cost;

    private void Awake()
    {
        _upgrades.Datas = GetUpgrades(out int level);
        UpdateButton(level);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    protected abstract T GetDefaultValue();
    protected abstract void UpdateValue(T value);
    protected abstract void SetRewardText();
    protected abstract UpgradeData[] GetUpgrades(out int level);
    protected abstract int LevelUp();

    protected void SetRewardText(string text)
    {
        _rewardText.text = text;
    }

    private void OnClick()
    {
        if (PlayerData.TryToBuy(_cost))
        {
            UpdateButton(LevelUp());
        }
    }

    private void UpdateButton(int level)
    {
        if (level >= _upgrades.Datas.Length)
        {
            _cost = -1;
            UpdateButtonText();
            return;
        }

        if (level < 0)
        {
            UpdateValue(GetDefaultValue());

            if (_nextUpgrade != null)
            {
                _nextUpgrade.SetActive(false);
            }
        }
        else
        {
            UpdateValue(_upgrades.Datas[level].Value);

            if (_nextUpgrade != null)
            {
                _nextUpgrade.SetActive(true);
            }
        }

        if (level + 1 >= _upgrades.Datas.Length)
        {
            _cost = -1;
            UpdateButtonText();
            return;
        }

        _cost = _upgrades.Datas[level + 1].Cost;
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        _costText.text = _cost < 0 ? "MAX" : $"{_cost} Coins";
        SetRewardText();
    }

    [System.Serializable]
    public struct UpgradeDatas
    {
        public UpgradeData[] Datas;
    }

    [System.Serializable]
    public struct UpgradeData
    {
        public T Value;
        public float Cost;
    }
}
