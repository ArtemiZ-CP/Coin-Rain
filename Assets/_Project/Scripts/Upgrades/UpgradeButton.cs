using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradeButton<T> : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _rewardText;
    [SerializeField] private TMP_Text _costText;
    [SerializeField, ReadOnly] private UpgradeDatas _upgrades;

    private int _level;
    private float _cost;

    private void Awake()
    {
        _upgrades.Datas = GetUpgrades(out _level);
        UpdateButton();
    }

    protected virtual void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    protected virtual void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    protected abstract T GetDefaultValue();
    protected abstract void UpdateValue(T value);
    protected abstract void SetRewardText();
    protected abstract UpgradeData[] GetUpgrades(out int level);

    protected void SetRewardText(string text)
    {
        _rewardText.text = text;
    }

    private void OnClick()
    {
        if (PlayerCurrencyData.TryToBuy(_cost))
        {
            _level++;
            UpdateButton();
        }
    }

    private void UpdateButton()
    {
        if (_level >= _upgrades.Datas.Length)
        {
            _cost = -1;
            UpdateButtonText();
            return;
        }

        if (_level < 0)
        {
            UpdateValue(GetDefaultValue());
        }
        else
        {
            UpdateValue(_upgrades.Datas[_level].Value);
        }

        if (_level + 1 >= _upgrades.Datas.Length)
        {
            _cost = -1;
            UpdateButtonText();
            return;
        }

        _cost = _upgrades.Datas[_level + 1].Cost;
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
