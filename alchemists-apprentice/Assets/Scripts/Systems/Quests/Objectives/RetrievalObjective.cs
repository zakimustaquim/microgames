using UnityEngine;

/// <summary>
/// A quest objective that requires the player to retrieve a certain
/// quantity of a specific item.
/// </summary>
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
