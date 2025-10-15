/// <summary>
/// Base class for all Quest Rewards.
/// </summary>
public abstract class QuestReward : LoggingScriptableObject
{
    public abstract QuestRewardRuntime CreateRuntime();
}
