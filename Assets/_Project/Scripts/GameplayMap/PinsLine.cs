using System.Collections.Generic;
using UnityEngine;

public class PinsLine : MonoBehaviour
{
    [SerializeField] private bool _firstLine;
    [SerializeField] private List<Pin> _pins;

    public IReadOnlyList<Pin> Pins => _pins;

    [ContextMenu("Update Pins")]
    public void UpdatePins()
    {
        PinConstants pinConstants = GameConstants.Instance.PinConstants;

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

        int actualPinCount = _firstLine ? PlayerData.WidthUpgrade.Value * 2 + 1 : PlayerData.WidthUpgrade.Value * 2;

        for (int i = 0; i < actualPinCount; i++)
        {
            Pin pin = Instantiate(pinConstants.PinPrefab, transform);
            float position = (i - (actualPinCount - 1) / 2.0f) * pinConstants.OffsetBetweenPinsInLine;
            pin.transform.localPosition = new Vector3(position, 0, 0);
            pin.Reset();
            _pins.Add(pin);
        }
    }

    public void Reset()
    {
        foreach (Pin pin in _pins)
        {
            pin.Reset();
        }
    }
}
