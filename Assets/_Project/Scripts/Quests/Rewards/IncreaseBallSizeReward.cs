public class IncreaseBallSizeReward : QuestReward
{
    private readonly float _increaseValue = 0.1f;

    public float IncreaseValue => _increaseValue;

    public IncreaseBallSizeReward(float increaseValue)
    {
        _increaseValue = increaseValue;
    }

    public override void ApplyReward()
    {
        PlayerData.IncreaseBallSize(_increaseValue);
    }
}
