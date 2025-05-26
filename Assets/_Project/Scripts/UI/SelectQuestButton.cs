using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectQuestButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _questText;
    [SerializeField] private Image _background;

    private Quest.Data _data;

    public static event Action<Quest.Data> OnQuestSelected;

    public void Initialize(Quest.Data data)
    {
        _data = data;
        _background.color = data.Rarity.Color;
        DisplayText();
    }

    public void Select()
    {
        OnQuestSelected?.Invoke(_data);
    }

    private void DisplayText()
    {
        string text = string.Empty;

        text += $"Objectives:\n";

        foreach (QuestObjectiveData objectiveData in _data.ObjectivesData)
        {
            text += Quest.CreateObjective(objectiveData).GetDescriptionText();
            text += "\n";
        }

        text += $"\nRewards:\n";

        foreach (QuestRewardData rewardData in _data.RewardsData)
        {
            text += Quest.CreateReward(rewardData).GetDescriptionText();
            text += "\n";
        }

        _questText.text = text;
    }
}
