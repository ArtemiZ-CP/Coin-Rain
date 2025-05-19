using UnityEngine;

public class SelectQuestWindow : MonoBehaviour
{
    [SerializeField] private GameObject _window;
    [SerializeField] private Transform _buttonParent;
    [SerializeField] private SelectQuestButton _questButtonPrefab;

    private void OnEnable()
    {
        SelectQuestButton.OnQuestSelected += CloseWindow;
    }

    private void OnDisable()
    {
        SelectQuestButton.OnQuestSelected -= CloseWindow;
    }

    public void DisplayQuests(Quest.Data[] quests)
    {
        foreach (Transform child in _buttonParent)
        {
            Destroy(child.gameObject);
        }

        _window.SetActive(true);

        foreach (Quest.Data quest in quests)
        {
            SelectQuestButton selectQuestButton = Instantiate(_questButtonPrefab, _buttonParent);
            selectQuestButton.Initialize(quest);
        }
    }

    private void CloseWindow(Quest.Data data)
    {
        _window.SetActive(false);
    }
}
