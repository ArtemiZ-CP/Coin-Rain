using System;
using System.Collections.Generic;
using UnityEngine;

public class WinAreas : MonoBehaviour
{
    private readonly List<WinArea> _winAreas = new();

    private void OnEnable()
    {
        BallsController.OnReset += UpdateAreas;
        PlayerMapData.OnMapUpdate += UpdateAreas;
        PlayerFinishData.OnFinishUpdate += UpdateAreas;
    }

    private void OnDisable()
    {
        BallsController.OnReset -= UpdateAreas;
        PlayerMapData.OnMapUpdate -= UpdateAreas;
        PlayerFinishData.OnFinishUpdate -= UpdateAreas;
    }

    public void UpdateAreas()
    {
        _winAreas.Clear();

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

        float mapWidth = PlayerMapData.GetMapWidth();
        float widthSum = 0;

        List<(float finishWidth, PlayerFinishData.FinishData finishData)> finishData = new();

        foreach (PlayerFinishData.FinishType finishType in Enum.GetValues(typeof(PlayerFinishData.FinishType)))
        {
            PlayerFinishData.FinishData finishCount = PlayerFinishData.GetFinishData(finishType);

            if (finishCount != null && finishCount.Count > 0)
            {
                for (int j = 0; j < finishCount.Count; j++)
                {
                    finishData.Add((finishCount.Width, finishCount));
                    widthSum += finishCount.Width;
                }
            }
        }

        System.Random rng = new((int)Time.time);
        for (int i = finishData.Count - 1; i > 0; i--)
        {
            int swapIndex = rng.Next(i + 1);
            (finishData[swapIndex], finishData[i]) = (finishData[i], finishData[swapIndex]);
        }

        float widthMultiplier = mapWidth / widthSum;
        float finishPosition = -mapWidth / 2 + finishData[0].finishWidth * widthMultiplier / 2;
        PinConstants pinConstants = GameConstants.Instance.PinConstants;

        for (int j = 0; j < finishData.Count; j++)
        {
            (float finishWidth, PlayerFinishData.FinishData finish) = finishData[j];
            SpawnFinish(pinConstants, finish, finishWidth * widthMultiplier, finishPosition);

            if (j < finishData.Count - 1)
            {
                finishPosition += finishWidth * widthMultiplier / 2;
                finishPosition += finishData[j + 1].finishWidth * widthMultiplier / 2;
            }
        }

        transform.position = -1 * PlayerMapData.GetMapHeight() * Vector3.up;
    }

    private void SpawnFinish(PinConstants pinConstants, PlayerFinishData.FinishData finish, float width, float finishPosition)
    {
        WinArea winArea = Instantiate(pinConstants.WinAreaPrefab, transform);
        winArea.transform.localPosition = finishPosition * Vector3.right;
        winArea.Initialize(finish.Multiplier, width, finish.Type);
        _winAreas.Add(winArea);
    }
}
