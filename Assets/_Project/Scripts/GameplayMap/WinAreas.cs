using System.Collections.Generic;
using UnityEngine;

public class WinAreas : MonoBehaviour
{
    [SerializeField] private List<WinArea> winAreas = new();

    private void OnEnable()
    {
        PlayerData.OnMapUpdate += UpdateAreas;
        PlayerData.OnFinishUpdate += UpdateAreas;
    }

    private void OnDisable()
    {
        PlayerData.OnMapUpdate -= UpdateAreas;
        PlayerData.OnFinishUpdate -= UpdateAreas;
    }

    [ContextMenu("Update Areas")]
    public void UpdateAreas()
    {
        PinConstants pinConstants = GameConstants.Instance.PinConstants;
        winAreas.Clear();

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

        int areasCount = (PlayerData.WidthUpgrade.Value + 1) * 2;
        int finishLevel = PlayerData.WinAreasUpgrade.Value + 1;

        for (int i = 0; i < areasCount / 2; i++)
        {
            SpawnFinish(pinConstants, finishLevel, areasCount, i, positionMultiplier: 1);
            SpawnFinish(pinConstants, finishLevel, areasCount, i, positionMultiplier: -1);
        }

        transform.position = -1 * (PlayerData.HeightUpgrade.Value - 1) * pinConstants.OffsetBetweenLines * Vector3.up;
    }

    private void SpawnFinish(PinConstants pinConstants, int finishLevel, int areasCount, int index, int positionMultiplier)
    {
        WinArea winArea = Instantiate(pinConstants.WinAreaPrefab, transform);
        float position = (index - (areasCount - 1) / 2.0f) * pinConstants.OffsetBetweenPinsInLine * positionMultiplier;
        winArea.transform.localPosition = position * Vector3.right;
        winArea.SetMultiplier(Mathf.Max(finishLevel - (areasCount / 2 - index) + 1, 1));
        winAreas.Add(winArea);
    }
}
