using System.Collections.Generic;
using UnityEngine;

public class PinsLine : MonoBehaviour
{
    [SerializeField] private bool _firstLine;
    [SerializeField] private List<PinObject> _pins;

    public IReadOnlyList<PinObject> Pins => _pins;

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

        int pinsCount = PlayerMapData.GetPinsCount(_firstLine);

        for (int i = 0; i < pinsCount; i++)
        {
            PinObject pin = Instantiate(pinConstants.PinPrefab, transform);
            float position = (i - (pinsCount - 1) / 2.0f) * pinConstants.OffsetBetweenPinsInLine;
            pin.transform.localPosition = new Vector3(position, 0, 0);
            int pinPosition;

            if (_firstLine)
            {
                pinPosition = 0;
            }
            else
            {
                if (i == 0)
                {
                    pinPosition = -1;
                }
                else if (i == pinsCount - 1)
                {
                    pinPosition = 1;
                }
                else
                {
                    pinPosition = 0;
                }
            }

            pin.ResetPin(pinPosition);
            _pins.Add(pin);
        }
    }
}
