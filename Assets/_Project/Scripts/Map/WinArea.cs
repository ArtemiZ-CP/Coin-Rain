using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private GameObject _leftBorder;
    [SerializeField] private GameObject _rightBorder;
    [SerializeField] private List<FinishData> _backgroundColor;

    private int _multiplier;

    public int Multiplier => _multiplier;

    private void OnValidate()
    {
        _text.text = $"x{_multiplier}";
    }

    public void Initialize(int multiplier, float width, PlayerFinishData.FinishType finishType)
    {
        _multiplier = multiplier;
        _text.text = $"x{_multiplier}";
        _background.transform.localScale = new Vector3(width, 1, 1);
        float xPosition = (width + _leftBorder.transform.localScale.x) / 2;
        _leftBorder.transform.localPosition = new Vector3(-xPosition, 0, 0);
        _rightBorder.transform.localPosition = new Vector3(xPosition, 0, 0);

        foreach (FinishData finishData in _backgroundColor)
        {
            if (finishData.Type == finishType)
            {
                _background.color = finishData.Color;
                return;
            }
        }
    }

    [Serializable]
    private struct FinishData
    {
        public PlayerFinishData.FinishType Type;
        public Color Color;
    }
}
