using System;
using UnityEngine;

public class PlayerQuests : MonoBehaviour
{
    private Quest _activeQuest;

    public Quest ActiveQuest => _activeQuest;

    public event Action<Quest> OnQuestsUpdated;

    private void Awake()
    {
        OnQuestsUpdated?.Invoke(_activeQuest);
    }

    private void OnEnable()
    {
        SelectQuestButton.OnQuestSelected += SetQuest;
    }

    private void OnDisable()
    {
        SelectQuestButton.OnQuestSelected -= SetQuest;
    }

    public void SetQuest(Quest.Data quest)
    {
        _activeQuest = Quest.CreateQuest(quest);
        _activeQuest.OnQuestCompleted += HandleCompletedQuest;
        OnQuestsUpdated?.Invoke(_activeQuest);
    }

    private void HandleCompletedQuest(Quest quest)
    {
        _activeQuest.OnQuestCompleted -= HandleCompletedQuest;
        _activeQuest = null;
        OnQuestsUpdated?.Invoke(_activeQuest);
    }
}
