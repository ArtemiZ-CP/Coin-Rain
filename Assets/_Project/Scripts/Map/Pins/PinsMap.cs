using System.Collections.Generic;
using UnityEngine;

public class PinsMap : MonoBehaviour
{
    public static PinsMap Instance { get; private set; }

    private readonly List<PinsLine> _pinLines = new();

    [SerializeField] private PinItemsScriptableObject _pinItemsScriptableObject = default;

    private PinConstants _pinConstants = default;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _pinConstants = GameConstants.Instance.PinConstants;
    }

    public void UpdatePins()
    {
        _pinLines.Clear();

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < PlayerMapData.LinesCount; i++)
        {
            PinsLine pinsLine;

            if (i % 2 == 0)
            {
                pinsLine = Instantiate(_pinConstants.Line1Prefab, transform);
            }
            else
            {
                pinsLine = Instantiate(_pinConstants.Line2Prefab, transform);
            }

            pinsLine.transform.localPosition = new Vector3(0, -i * _pinConstants.OffsetBetweenLines, 0);
            pinsLine.UpdatePins();
            _pinLines.Add(pinsLine);
        }

        Reset();
    }

    public void Reset()
    {
        List<PinObject> pins = new();

        foreach (PinsLine pinsLine in _pinLines)
        {
            pins.AddRange(pinsLine.Pins);
        }

        foreach (PinItem pinItem in _pinItemsScriptableObject.PinItems)
        {
            SetPins(pinItem, pins);
        }

        SetPins(_pinItemsScriptableObject.BasePinItem, pins);
    }

    public float Blast(PinObject blastPin, PlayerBall playerBall, int range, float coins)
    {
        float blastRange = _pinConstants.OffsetBetweenPinsInLine * range + 0.1f;

        foreach (PinsLine pinsLine in _pinLines)
        {
            foreach (PinObject pin in pinsLine.Pins)
            {
                if (pin == blastPin ||
                    blastPin.transform.position.IsEnoughClose(pin.transform.position, blastRange) == false)
                {
                    continue;
                }

                if (pin.TryTouch(playerBall, out float pinCoins))
                {
                    coins += pinCoins;
                }
            }
        }

        return coins;
    }

    public List<(Pin.Type, int)> GetPinsCountByType()
    {
        List<(Pin.Type type, int count)> pinsCountByType = new();

        foreach (Pin.Type type in Pin.GetAllTypes())
        {
            pinsCountByType.Add((type, 0));
        }

        foreach (PinsLine pinsLine in _pinLines)
        {
            foreach (PinObject pin in pinsLine.Pins)
            {
                for (int i = 0; i < pinsCountByType.Count; i++)
                {
                    (Pin.Type type, int count) pinsCount = pinsCountByType[i];

                    if (pinsCount.type == pin.PinItem.Type)
                    {
                        pinsCount = (pinsCount.type, pinsCount.count + 1);
                        pinsCountByType[i] = pinsCount;
                        break;
                    }
                }
            }
        }

        for (int i = pinsCountByType.Count - 1; i >= 0; i--)
        {
            if (pinsCountByType[i].count == 0)
            {
                pinsCountByType.RemoveAt(i);
            }
        }

        return pinsCountByType;
    }

    private void SetPins(PinItem pinItem, List<PinObject> pins)
    {
        Pin pin = Pin.Get(pinItem.Type);

        for (int i = 0; i < pin.GetPinsCount(); i++)
        {
            if (pins.Count == 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, pins.Count);
            pins[randomIndex].SetPin(pinItem);
            pins.RemoveAt(randomIndex);
        }
    }
}
