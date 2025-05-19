using TMPro;
using UnityEngine;

public class PlayerQuestsDisplayer : MonoBehaviour
{
    [SerializeField] private PlayerQuestDisplayer _questDisplayerPrefab;
    [SerializeField] private PlayerQuests _playerQuests;
    [SerializeField] private TMP_Text _questReward;

    private void OnEnable()
    {
        UpdateQuestDisplayer(null);
        _playerQuests.OnQuestsUpdated += UpdateQuestDisplayer;
    }

    private void OnDisable()
    {
        _playerQuests.OnQuestsUpdated -= UpdateQuestDisplayer;
    }

    private void UpdateQuestDisplayer(Quest quest)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (quest == null)
        {
            return;
        }

        foreach (QuestObjective objective in quest.Objectives)
        {
            PlayerQuestDisplayer questDisplayer = Instantiate(_questDisplayerPrefab, transform);
            questDisplayer.SetQuest(objective);
        }

        SetRewardText(quest.Rewards);
    }

    private void SetRewardText(QuestReward[] questRewards)
    {
        string rewardText = string.Empty;

        foreach (QuestReward questReward in questRewards)
        {
            rewardText += questReward.GetDescriptionText();
            rewardText += "\n";
        }

        _questReward.text = rewardText;
    }
}
