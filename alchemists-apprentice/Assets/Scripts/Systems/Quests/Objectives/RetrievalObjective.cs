using UnityEngine;

[CreateAssetMenu(fileName = "New Retrieval Objective", menuName = "Quests/Objectives/Retrieval")]
public class RetrievalObjective : QuestObjective
{
    public string itemID; // replace with Item reference later
    public int quantity;

    public override QuestObjectiveRuntime CreateRuntime()
    {
        return new RetrievalObjectiveRuntime(this);
    }
}
