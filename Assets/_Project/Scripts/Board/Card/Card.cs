using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private readonly int TurnAnimationHash = Animator.StringToHash("Turn");

    [SerializeField] private Image _cardBlowImage;
    [SerializeField] private Image _cursedImage;
    [SerializeField] private Image _rewardImage;
    [SerializeField] private Button _cardButton;
    [SerializeField] private Animator _cardAnimator;
    [SerializeField] private CardData[] _cardDatas;

    private bool _isActive;
    private Vector2Int _gridPosition;
    private Type _cardType;
    private CardReward _cardReward;

    public bool IsActive => _isActive;
    public Vector2Int GridPosition => _gridPosition;
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

    public void Initialize(Type type, Vector2Int gridPosition)
    {
        _isActive = true;
        _cardType = type;
        _cursedImage.gameObject.SetActive(false);
        _gridPosition = gridPosition;
        _cardBlowImage.color = _cardDatas.FirstOrDefault(data => data.Type == type).Color;
        _cardReward = CardRewardGenerator.GenerateReward(type);
        _rewardImage.sprite = _cardReward.GetRewardData().ItemSprite;
    }

    public void Disactive()
    {
        _isActive = false;
    }

    public void Curse()
    {
        _isActive = false;
        _cursedImage.gameObject.SetActive(true);
    }

    public void Turn()
    {
        _cardAnimator.SetTrigger(TurnAnimationHash);
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
        Throw
    }

    [Serializable]
    public struct CardData
    {
        public Type Type;
        public Color Color;
    }
}
