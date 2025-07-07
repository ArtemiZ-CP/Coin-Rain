using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private readonly List<Card> _cards = new();

    [SerializeField] private Card _cardPrefab;
    [SerializeField] private CardsHint _cardsHint;
    [SerializeField] private Button _endStageButton;
    [SerializeField] private RectTransform _cardsParent;
    [SerializeField] private Vector2 _cardSize;
    [SerializeField] private float _cardOffset;
    [SerializeField] private float _clickCardDelay = 0.2f;

    private Vector2 _lastParentSize;
    private float _lastClickTime;

    public event Action<Card> OnCardClick;
    public event Action OnEndStageButtonClick;

    private void Awake()
    {
        _cardsHint.Initialize();
    }

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
            if (card.Clickable)
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
        List<Card.Type> cardTypes = GetCardTypes(positionsToUse);

        for (int i = 0; i < positionsToUse; i++)
        {
            Card card = Instantiate(_cardPrefab, _cardsParent);
            _cards.Add(card);
            card.Initialize(cardTypes[i], gridPositions[i]);
            card.OnCardClick += HandleCardClick;
            SetCardTransform(card, gridPositions[i]);
        }

        if (_cardsParent != null)
        {
            _lastParentSize = _cardsParent.rect.size;
        }
        
        SetCardsHint();
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

    private List<Card.Type> GetCardTypes(int cardsCount)
    {
        List<Card.Type> cardTypes = new();

        for (int i = 0; i < PlayerCardsData.BlessedCardsCount; i++)
        {
            cardTypes.Add(Card.Type.Blessed);
        }

        for (int i = 0; i < PlayerCardsData.CursedCardsCount; i++)
        {
            cardTypes.Add(Card.Type.Cursed);
        }

        for (int i = 0; i < PlayerCardsData.ThrowCardsCount; i++)
        {
            cardTypes.Add(Card.Type.Throw);
        }

        int baseCardsCount = cardsCount - cardTypes.Count;

        for (int i = 0; i < baseCardsCount; i++)
        {
            cardTypes.Add(Card.Type.Base);
        }

        for (int i = 0; i < cardTypes.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, cardTypes.Count);
            (cardTypes[randomIndex], cardTypes[i]) = (cardTypes[i], cardTypes[randomIndex]);
        }

        return cardTypes;
    }

    private void HandleCardClick(Card clickedCard)
    {
        if (Time.time - _lastClickTime < _clickCardDelay)
        {
            return;
        }

        _lastClickTime = Time.time;
        clickedCard.Turn();
        StartCoroutine(CardClickWithDelay(clickedCard));
    }

    private IEnumerator CardClickWithDelay(Card clickedCard)
    {
        yield return new WaitForSeconds(_clickCardDelay);

        OnCardClick?.Invoke(clickedCard);
        Vector2Int gridPosition = clickedCard.GridPosition;

        if (clickedCard.CardType == Card.Type.Cursed)
        {
            foreach (Card card in _cards)
            {
                if (card.GridPosition.x == gridPosition.x || card.GridPosition.y == gridPosition.y)
                {
                    card.Curse();
                }
            }
        }
        
        SetCardsHint();
        clickedCard.CardReward.ApplyReward();
    }

    private void SetCardsHint()
    {
        if (_cardsHint == null)
        {
            Debug.LogWarning("CardsHint is not assigned in the inspector");
            return;
        }

        List<(Card.Type type, int count)> cardTypes = new()
        {
            (Card.Type.Blessed, 0),
            (Card.Type.Cursed, 0),
            (Card.Type.Throw, 0),
            (Card.Type.Base, 0)
        };

        foreach (Card card in _cards)
        {
            if (card.Turned)
            {
                continue;
            }

            int idx = cardTypes.FindIndex(type => type.type == card.CardType);

            if (idx != -1)
            {
                var tuple = cardTypes[idx];
                tuple.count++;
                cardTypes[idx] = tuple;
            }
        }

        _cardsHint.SetCardsHint(cardTypes);
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
