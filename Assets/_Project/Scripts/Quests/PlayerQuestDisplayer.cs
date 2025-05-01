using TMPro;
using UnityEngine;

public class PlayerQuestDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _questDescription;
    [SerializeField] private TMP_Text _questReward;

    public void SetQuest(Quest quest)
    {
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
                Pin.Type.Base => "обычные",
                Pin.Type.Gold => "золотые",
                _ => "неизвестные"
            };

            _questDescription.text = $"Задеть шаром {pinType} штырьки за один раунд: {hitPins.PinsCount}";
        }
        else if (quest.Objective is EarnCoinsObjective earnCoins)
        {
            _questDescription.text = $"Заработать монет одним шаром: {earnCoins.CoinsCount}";
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
                if (PlayerData.GoldPinsCountUpgrade == 0)
                {
                    _questReward.text = "Открыть золотой штырек";
                }
                else
                {
                    _questReward.text = "Добавить золотой штырек";
                }
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
        else
        {
            _questReward.text = "Неизвестная награда";
        }
    }
}
