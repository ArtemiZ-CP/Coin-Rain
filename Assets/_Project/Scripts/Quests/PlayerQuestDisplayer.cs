using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerQuestDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _questDescription;
    [SerializeField] private Image _progressBar;

    private QuestObjective _currentQuestObjective;

    private void OnDestroy()
    {
        if (_currentQuestObjective != null)
        {
            _currentQuestObjective.OnObjectiveProgressChanged -= UpdateProgressBar;
        }
    }

    public void SetQuest(QuestObjective questObjective)
    {
        if (_currentQuestObjective != null)
        {
            _currentQuestObjective.OnObjectiveProgressChanged -= UpdateProgressBar;
        }

        _currentQuestObjective = questObjective;
        _currentQuestObjective.OnObjectiveProgressChanged += UpdateProgressBar;

        UpdateProgressBar(0);
        SetDescriptionText(questObjective);
    }

    private void SetDescriptionText(QuestObjective questObjective)
    {
        _questDescription.text = questObjective.GetDescriptionText();
    }

    private void UpdateProgressBar(float progress)
    {
        if (_currentQuestObjective == null)
        {
            return;
        }

        _progressBar.fillAmount = progress;
    }
}