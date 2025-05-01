public class Quest
{
    private readonly QuestObjective _objective;
    private readonly QuestReward _reward;

    public delegate void QuestUpdated(Quest quest);
    public event QuestUpdated OnQuestCompleted;

    public QuestObjective Objective => _objective;
    public QuestReward Reward => _reward;

    public Quest(QuestObjective objective, QuestReward rewardData)
    {
        _objective = objective;
        _reward = rewardData;
        _objective.OnObjectiveCompleted += CompleteQuest;
    }

    public void CompleteQuest()
    {
        _reward.ApplyReward();
        OnQuestCompleted?.Invoke(this);
        _objective.OnObjectiveCompleted -= CompleteQuest;
    }
}
