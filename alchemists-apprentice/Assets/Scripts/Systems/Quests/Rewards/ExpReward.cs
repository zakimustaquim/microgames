using UnityEngine;

/// <summary>
/// Gives the player a set amount of EXP upon quest completion.
/// </summary>
[CreateAssetMenu(fileName = "New EXP Reward", menuName = "Quests/Rewards/EXP")]
public class ExpReward : QuestReward
{
    public int expAmount;

    public override QuestRewardRuntime CreateRuntime()
    {
        return new ExpRewardRuntime(this);
    }
}
