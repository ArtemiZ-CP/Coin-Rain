using UnityEngine;
using UnityEngine.UI;

public class GetNewQuestsButton : MonoBehaviour
{
    [SerializeField] private PlayerQuests _playerQuests;
    [SerializeField] private Button _button;

    public event System.Action OnClick;

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleButtonClick);
        SelectQuestButton.OnQuestSelected += HideButton;
        _playerQuests.OnQuestsUpdated += ShowButton;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleButtonClick);
        SelectQuestButton.OnQuestSelected -= HideButton;
        _playerQuests.OnQuestsUpdated -= ShowButton;
    }

    private void HandleButtonClick()
    {
        OnClick?.Invoke();
    }

    private void HideButton(Quest.Data questData)
    {
        _button.gameObject.SetActive(false);
    }
    
    private void ShowButton(Quest quest)
    {
        if (quest != null)
        {
            return;
        }

        _button.gameObject.SetActive(true);
    }
}
