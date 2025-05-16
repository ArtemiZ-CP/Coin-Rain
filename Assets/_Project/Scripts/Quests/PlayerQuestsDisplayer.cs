using UnityEngine;

public class PlayerQuestsDisplayer : MonoBehaviour
{
    [SerializeField] private PlayerQuestDisplayer _questDisplayerPrefab;
    [SerializeField] private PlayerQuests _playerQuests;

    private void OnEnable()
    {
        UpdateQuestDisplayer();
        _playerQuests.OnQuestsUpdated += UpdateQuestDisplayer;
    }

    private void OnDisable()
    {
        _playerQuests.OnQuestsUpdated -= UpdateQuestDisplayer;
    }

    private void UpdateQuestDisplayer()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var activeQuests = _playerQuests.GetActiveQuests();

        foreach (Quest quest in activeQuests)
        {
            PlayerQuestDisplayer questDisplayer = Instantiate(_questDisplayerPrefab, transform);
            questDisplayer.SetQuest(quest);
        }
    }
}
