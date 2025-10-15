using System.Linq;

/// <summary>
/// Runtime representation of a [Quest]. Contains
/// the data for the Quest (rewards, objectives, etc)
/// as well as the state for the current player.
/// </summary>
public class QuestRuntime : Logging
{
    public Quest questData;
    public bool completed;
    public bool active;

    public QuestObjectiveRuntime[] objectiveRuntimes;
    public QuestRewardRuntime[] rewardRuntimes;

    public QuestRuntime(Quest questData)
    {
        this.questData = questData;
        completed = false;
        active = false;
        objectiveRuntimes = questData.objectives.Select(obj => obj.CreateRuntime()).ToArray();
        rewardRuntimes = questData.rewards.Select(reward => reward.CreateRuntime()).ToArray();
    }
}
