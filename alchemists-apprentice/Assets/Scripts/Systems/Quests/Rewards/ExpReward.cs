using UnityEngine;

[CreateAssetMenu(fileName = "New EXP Reward", menuName = "Quests/Rewards/EXP")]
public class ExpReward : QuestReward
{
    public int expAmount;

    public override QuestRewardRuntime CreateRuntime()
    {
        return new ExpRewardRuntime(this);
    }
}
