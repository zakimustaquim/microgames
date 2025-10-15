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
