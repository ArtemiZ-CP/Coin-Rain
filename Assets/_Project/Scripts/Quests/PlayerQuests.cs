using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuests : MonoBehaviour
{
    private readonly List<Quest> ActiveQuests = new();
    private readonly List<Quest> DisactiveQuests = new();

    [SerializeField] private int _maxActiveQuests = 3;
    [SerializeField] private int _completeQuestsAtStartCount = 0;

    private List<Quest.Data> _playerQuests = new();

    public event Action OnQuestsUpdated;

    private void Awake()
    {
        foreach (Quest.Data objectiveData in _playerQuests)
        {
            DisactiveQuests.Add(Quest.CreateQuest(objectiveData));
        }

        UpdateActiveQuests();
        OnQuestsUpdated?.Invoke();
    }

    private void Start()
    {
        for (int i = 0; i < _completeQuestsAtStartCount; i++)
        {
            Quest quest = ActiveQuests[0];
            quest.CompleteQuest();
        }
    }

    public IReadOnlyList<Quest> GetActiveQuests() => ActiveQuests;

    public void AddActiveQuest(Quest quest)
    {
        ActiveQuests.Add(quest);
        quest.OnQuestCompleted += HandleCompletedQuest;
    }

    public void RemoveActiveQuest(Quest quest)
    {
        if (ActiveQuests.Contains(quest))
        {
            ActiveQuests.Remove(quest);
            quest.OnQuestCompleted -= HandleCompletedQuest;
        }
    }

    private void HandleCompletedQuest(Quest quest)
    {
        RemoveActiveQuest(quest);
        UpdateActiveQuests();
        OnQuestsUpdated?.Invoke();
    }

    private void UpdateActiveQuests()
    {
        for (int i = 0; i < DisactiveQuests.Count; i++)
        {
            if (ActiveQuests.Count < _maxActiveQuests)
            {
                Quest quest = DisactiveQuests[i];
                AddActiveQuest(quest);
                DisactiveQuests.Remove(quest);
                i--;
            }
            else
            {
                return;
            }
        }
    }
}
