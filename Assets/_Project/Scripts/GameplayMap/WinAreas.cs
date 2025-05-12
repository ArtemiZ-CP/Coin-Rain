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

        int areasCount = (PlayerData.WidthUpgrade + 1) * 2;
        int finishUpgrade = PlayerData.WinAreasUpgrade;

        for (int i = 0; i < areasCount / 2; i++)
        {
            SpawnFinish(pinConstants, finishUpgrade, areasCount, i, positionMultiplier: 1);
            SpawnFinish(pinConstants, finishUpgrade, areasCount, i, positionMultiplier: -1);
        }

        transform.position = -1 * (PlayerData.HeightUpgrade - 1) * pinConstants.OffsetBetweenLines * Vector3.up;
    }

    private void SpawnFinish(PinConstants pinConstants, int finishUpgrade, int areasCount, int index, int positionMultiplier)
    {
        WinArea winArea = Instantiate(pinConstants.WinAreaPrefab, transform);
        float positionX = (index - (areasCount - 1) / 2.0f) * pinConstants.OffsetBetweenPinsInLine * positionMultiplier;
        winArea.transform.localPosition = positionX * Vector3.right;
        winArea.SetMultiplier(index + finishUpgrade);
        winAreas.Add(winArea);
    }
}
