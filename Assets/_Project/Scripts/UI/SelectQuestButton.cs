using System;
using TMPro;
using UnityEngine;

public class SelectQuestButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _questText;

    private Quest.Data _data;

    public static event Action<Quest.Data> OnQuestSelected;

    public void Initialize(Quest.Data data)
    {
        _data = data;
        DisplayText();
    }

    public void Select()
    {
        OnQuestSelected?.Invoke(_data);
    }

    private void DisplayText()
    {
        string text = string.Empty;

        foreach (QuestObjectiveData objectiveData in _data.ObjectivesData)
        {
            text += Quest.CreateObjective(objectiveData).GetDescriptionText();
            text += "\n";
        }

        foreach (QuestRewardData rewardData in _data.RewardsData)
        {
            text += Quest.CreateReward(rewardData).GetDescriptionText();
            text += "\n";
        }

        _questText.text = text;
    }
}
