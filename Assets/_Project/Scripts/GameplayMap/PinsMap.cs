using System.Collections.Generic;
using UnityEngine;

public class PinsMap : MonoBehaviour
{
    [SerializeField] private List<PinsLine> _pinLines = new();

    private PinConstants _pinConstants = default;
 
    private void Awake()
    {
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

        for (int i = 0; i < PlayerData.HeightUpgrade; i++)
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

        SetPins(Pin.Type.Gold, PlayerData.GoldPinsCountUpgrade, pins);
        SetPins(Pin.Type.Multiplying, PlayerData.MultiPinsCountUpgrade, pins);
        SetPins(Pin.Type.Bomb, PlayerData.BombPinsCountUpgrade, pins);
    }

    public float Blast(Pin blastPin, PlayerBall playerBall)
    {
        float blastRange = _pinConstants.OffsetBetweenPinsInLine * PlayerData.BombPinsValueUpgrade + 0.1f;
        float coins = PlayerData.RewardFromPin;

        foreach (PinsLine pinsLine in _pinLines)
        {
            foreach (Pin pin in pinsLine.Pins)
            {
                if (blastPin.transform.position.IsEnoughClose(pin.transform.position, blastRange) == false)
                {
                    continue;
                }

                if (pin.Touch(playerBall, out float pinCoins))
                {
                    coins += pinCoins;
                }
            }
        }

        return coins;
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
