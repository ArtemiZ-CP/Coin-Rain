using System.Collections.Generic;
using UnityEngine;

public class PinsMap : MonoBehaviour
{
    public static PinsMap Instance { get; private set; }

    [SerializeField] private List<PinsLine> _pinLines = new();

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

    [ContextMenu("Update Pins")]
    public void UpdatePins()
    {
#if UNITY_EDITOR
        if (Application.isPlaying == false)
        {
            _pinConstants = GameConstants.Instance.PinConstants;
        }
#endif

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

        for (int i = 0; i < PlayerMapUpgradesData.MapHeight; i++)
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
        List<Pin> pins = new();

        foreach (PinsLine pinsLine in _pinLines)
        {
            pinsLine.Reset();
            pins.AddRange(pinsLine.Pins);
        }

        SetPins(Pin.Type.Gold, PlayerPinsData.GoldPinsCount, pins);
        SetPins(Pin.Type.Multiplying, PlayerPinsData.MultiPinsCount, pins);
        SetPins(Pin.Type.Bomb, PlayerPinsData.BombPinsCount, pins);
    }

    public float Blast(Pin blastPin, PlayerBall playerBall)
    {
        float blastRange = _pinConstants.OffsetBetweenPinsInLine * PlayerPinsData.BombPinsValue + 0.1f;
        float coins = PlayerPinsData.RewardFromPin;

        foreach (PinsLine pinsLine in _pinLines)
        {
            foreach (Pin pin in pinsLine.Pins)
            {
                if (blastPin.transform.position.IsEnoughClose(pin.transform.position, blastRange) == false)
                {
                    continue;
                }

                if (pin.TryHit(playerBall, out float pinCoins))
                {
                    coins += pinCoins;
                }
            }
        }

        return coins;
    }

    public List<(Pin.Type, int)> GetPinsCountByType()
    {
        List<(Pin.Type type, int count)> pinsCountByType = new()
        {
            (Pin.Type.Base, 0),
            (Pin.Type.Gold, 0),
            (Pin.Type.Multiplying, 0),
            (Pin.Type.Bomb, 0),
        };

        foreach (PinsLine pinsLine in _pinLines)
        {
            foreach (Pin pin in pinsLine.Pins)
            {
                for (int i = 0; i < pinsCountByType.Count; i++)
                {
                    (Pin.Type type, int count) pinsCount = pinsCountByType[i];

                    if (pinsCount.type == pin.PinType)
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

    private void SetPins(Pin.Type pinType, int pinsCount, List<Pin> pins)
    {
        for (int i = 0; i < pinsCount; i++)
        {
            if (pins.Count == 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, pins.Count);
            pins[randomIndex].SetPinType(pinType);
            pins.RemoveAt(randomIndex);
        }
    }
}
