using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerQuestDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _questDescription;
    [SerializeField] private TMP_Text _questReward;
    [SerializeField] private Image _progressBar;

    private Quest _currentQuest;

    private void OnDestroy()
    {
        if (_currentQuest != null)
        {
            _currentQuest.Objective.OnObjectiveProgressChanged -= UpdateProgressBar;
        }
    }

    public void SetQuest(Quest quest)
    {
        if (_currentQuest != null)
        {
            _currentQuest.Objective.OnObjectiveProgressChanged -= UpdateProgressBar;
        }

        _currentQuest = quest;
        _currentQuest.Objective.OnObjectiveProgressChanged += UpdateProgressBar;

        UpdateProgressBar(0);
        SetDescriptionText(quest);
        SetRewardText(quest);
    }

    private void SetDescriptionText(Quest quest)
    {
        if (quest.Objective is DropBallObjective)
        {
            _questDescription.text = $"Запустить шар";
        }
        else if (quest.Objective is HitFinishObjective hitFinish)
        {
            _questDescription.text = $"Попасть шаром во множитель x{hitFinish.FinishMultiplierToHit}";
        }
        else if (quest.Objective is HitPinsObjective hitPins)
        {
            string pinType = hitPins.PinType switch
            {
                Pin.Type.Base => "обычные штырьки",
                Pin.Type.Gold => "золотые штырьки",
                Pin.Type.Multiplying => "множители",
                Pin.Type.Bomb => "бомбы",
                _ => "неизвестные"
            };

            _questDescription.text = $"Задеть {pinType} за один раунд: {hitPins.PinsCount}";
        }
        else if (quest.Objective is EarnCoinsObjective earnCoins)
        {
            _questDescription.text = $"Заработать монет одним шаром: {earnCoins.CoinsCount}";
        }
        else if (quest.Objective is EarnCoinsByAllBallsObjective earnCoinsByAllBalls)
        {
            _questDescription.text = $"Заработать монет за один запуск: {earnCoinsByAllBalls.CoinsCount}";
        }
        else
        {
            _questDescription.text = "Неизвестный квест";
        }
    }

    private void SetRewardText(Quest quest)
    {
        if (quest.Reward is CoinsReward coinsReward)
        {
            _questReward.text = $"{coinsReward.CoinsCount} монет";
        }
        else if (quest.Reward is UnlockUpgrade unlockUpgrade)
        {
            if (unlockUpgrade.PinType == Pin.Type.Base)
            {
                _questReward.text = "Открыть улучшения";
            }
            else if (unlockUpgrade.PinType == Pin.Type.Gold)
            {
                _questReward.text = "Добавить золотой штырёк";
            }
            else if (unlockUpgrade.PinType == Pin.Type.Multiplying)
            {
                _questReward.text = "Добавить множитель";
            }
            else if (unlockUpgrade.PinType == Pin.Type.Bomb)
            {
                _questReward.text = "Добавить бомбу";
            }
            else
            {
                _questReward.text = "Неизвестная награда";
            }
        }
        else if (quest.Reward is IncreaseHeightReward)
        {
            _questReward.text = "Добавить линию штырьков";
        }
        else if (quest.Reward is IncreaseWidthReward)
        {
            _questReward.text = "Увеличить ширину линии штырьков";
        }
        else if (quest.Reward is IncreaseBallSizeReward)
        {
            _questReward.text = $"Увеличить размер шара";
        }
        else if (quest.Reward is IncreaseWinAreaMultiplierReward)
        {
            _questReward.text = $"Финальный множитель + 1";
        }
        else
        {
            _questReward.text = "Неизвестная награда";
        }
    }

    private void UpdateProgressBar(float progress)
    {
        if (_currentQuest == null)
        {
            return;
        }

        _progressBar.fillAmount = progress;
    }
}
