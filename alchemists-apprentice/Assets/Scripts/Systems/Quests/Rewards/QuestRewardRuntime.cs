/// <summary>
/// Runtime representation of a [QuestReward]. Contains
/// the data for the reward as well as the state
/// for the current player (whether it has been granted
/// or not).
/// </summary>
public abstract class QuestRewardRuntime : Logging
{
    public QuestReward rewardData;
    public bool granted;

    public QuestRewardRuntime(QuestReward rewardData)
    {
        this.rewardData = rewardData;
        granted = false;
    }
}
