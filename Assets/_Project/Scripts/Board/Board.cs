using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private readonly List<Card> _cards = new();

    [SerializeField] private Card _cardPrefab;
    [SerializeField] private Button _endStageButton;
    [SerializeField] private RectTransform _cardsParent;
    [SerializeField] private Vector2 _cardSize;
    [SerializeField] private float _cardOffset;

    private Vector2 _lastParentSize;

    public event Action<Card> OnCardClick;
    public event Action OnEndStageButtonClick;

    private void OnEnable()
    {
        _endStageButton.onClick.AddListener(EndStage);
    }

    private void OnDisable()
    {
        _endStageButton.onClick.RemoveListener(EndStage);
    }

    private void FixedUpdate()
    {
        CheckForSizeChanges();
    }

    public int GetActiveCardCount()
    {
        int count = 0;

        foreach (Card card in _cards)
        {
            if (card.IsActive)
            {
                count++;
            }
        }

        return count;
    }

    public void CreateCards()
    {
        Show();
        ClearCards();

        var gridPositions = PlayerCardsData.CardsPositions;
        int positionsToUse = gridPositions.Count;

        for (int i = 0; i < positionsToUse; i++)
        {
            Card card = Instantiate(_cardPrefab, _cardsParent);
            _cards.Add(card);
            InitializeCard(card);
            card.OnCardClick += HandleCardClick;
            SetCardTransform(card, gridPositions[i]);
        }

        if (_cardsParent != null)
        {
            _lastParentSize = _cardsParent.rect.size;
        }
    }

    public void ClearCards()
    {
        foreach (Card card in _cards)
        {
            card.OnCardClick -= HandleCardClick;
            Destroy(card.gameObject);
        }

        _cards.Clear();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void EndStage()
    {
        OnEndStageButtonClick?.Invoke();
    }

    private void InitializeCard(Card card)
    {
        Card.Type type = (Card.Type)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Card.Type)).Length);
        card.Initialize(type);
    }

    private void HandleCardClick(Card card)
    {
        OnCardClick?.Invoke(card);
    }

    private void SetCardTransform(Card card, Vector2Int gridPosition)
    {
        RectTransform cardRect = card.GetComponent<RectTransform>();
        RectTransform parentRect = _cardsParent.GetComponent<RectTransform>();

        if (ValidateRectTransforms(cardRect, parentRect) == false)
        {
            return;
        }

        Vector2 parentSize = parentRect.rect.size;
        float scale = CalculateGridScale(parentSize);
        Vector2 scaledCardSize = _cardSize * scale;
        float scaledOffset = _cardOffset * scale;

        Vector2 spacing = new(scaledCardSize.x + scaledOffset, scaledCardSize.y + scaledOffset);
        Vector2 gridSize = CalculateGridSize(spacing, scaledCardSize);
        Vector2 startPosition = CalculateStartPosition(parentSize, gridSize, scaledCardSize);

        Vector2 cardPosition = startPosition + new Vector2(gridPosition.x * spacing.x, gridPosition.y * spacing.y);
        Vector2 normalizedPosition = new(cardPosition.x / parentSize.x, cardPosition.y / parentSize.y);

        ApplyCardTransform(cardRect, scaledCardSize, normalizedPosition);
    }

    private bool ValidateRectTransforms(RectTransform cardRect, RectTransform parentRect)
    {
        if (cardRect == null || parentRect == null)
        {
            Debug.LogWarning("Card or parent does not have RectTransform component");
            return false;
        }

        return true;
    }

    private float CalculateGridScale(Vector2 parentSize)
    {
        Vector2 originalSpacing = new(_cardSize.x + _cardOffset, _cardSize.y + _cardOffset);
        Vector2 totalGridSize = CalculateGridSize(originalSpacing, _cardSize);

        float scaleX = parentSize.x / totalGridSize.x;
        float scaleY = parentSize.y / totalGridSize.y;

        return Mathf.Min(scaleX, scaleY);
    }

    private Vector2 CalculateGridSize(Vector2 spacing, Vector2 cardSize)
    {
        int gridSize = PlayerCardsData.DefaultCardsCountInRow;
        return new Vector2(
            (gridSize - 1) * spacing.x + cardSize.x,
            (gridSize - 1) * spacing.y + cardSize.y
        );
    }

    private Vector2 CalculateStartPosition(Vector2 parentSize, Vector2 gridSize, Vector2 cardSize)
    {
        return new Vector2(
            (parentSize.x - gridSize.x) * 0.5f + cardSize.x * 0.5f,
            (parentSize.y - gridSize.y) * 0.5f + cardSize.y * 0.5f
        );
    }

    private void ApplyCardTransform(RectTransform cardRect, Vector2 cardSize, Vector2 normalizedPosition)
    {
        cardRect.sizeDelta = cardSize;
        cardRect.anchorMin = normalizedPosition;
        cardRect.anchorMax = normalizedPosition;
        cardRect.anchoredPosition = Vector2.zero;
    }

    private void CheckForSizeChanges()
    {
        if (_cards.Count == 0)
        {
            return;
        }

        if (_cardsParent == null)
        {
            return;
        }
        
        Vector2 currentParentSize = _cardsParent.rect.size;

        if (currentParentSize.SqrDistance(_lastParentSize) > 0.01f)
        {
            _lastParentSize = currentParentSize;
            UpdateAllCardsLayout();
        }
    }

    private void UpdateAllCardsLayout()
    {
        var gridPositions = PlayerCardsData.CardsPositions;

        for (int i = 0; i < _cards.Count && i < gridPositions.Count; i++)
        {
            SetCardTransform(_cards[i], gridPositions[i]);
        }
    }
}
