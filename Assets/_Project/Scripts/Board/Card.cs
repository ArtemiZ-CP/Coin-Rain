using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image _usedCardImage;
    [SerializeField] private Image _cardBlowImage;
    [SerializeField] private TMP_Text _turnsText;
    [SerializeField] private CardData[] _cardDatas;
    [SerializeField] private Button _cardButton;

    private bool _isActive;
    private int _turnsCount;
    private Type _cardType;
    private CardReward _cardReward;

    public bool IsActive => _isActive;
    public int TurnsCount => _turnsCount;
    public Type CardType => _cardType;
    public CardReward CardReward => _cardReward;

    public event Action<Card> OnCardClick;

    private void OnEnable()
    {
        _cardButton.onClick.AddListener(HandleCardClick);
    }

    private void OnDisable()
    {
        _cardButton.onClick.RemoveListener(HandleCardClick);
    }

    public void Initialize(int turnsCount, Type type)
    {
        _isActive = true;
        _turnsCount = turnsCount;
        _cardType = type;
        _turnsText.text = turnsCount.ToString();
        _cardBlowImage.color = _cardDatas.FirstOrDefault(data => data.Type == type).Color;
        _cardReward = CardRewardGenerator.GenerateReward(type);
        _usedCardImage.gameObject.SetActive(false);
    }

    public void Disactive()
    {
        _isActive = false;
        _usedCardImage.gameObject.SetActive(true);
    }

    private void HandleCardClick()
    {
        if (_isActive == false)
        {
            return;
        }

        OnCardClick?.Invoke(this);
    }

    public enum Type
    {
        Blessed,
        Base,
        Cursed,
    }

    [Serializable]
    public struct CardData
    {
        public Type Type;
        public Color Color;
    }
}
