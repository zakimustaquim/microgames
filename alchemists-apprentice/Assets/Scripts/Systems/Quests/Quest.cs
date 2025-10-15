using UnityEngine;

/// <summary>
/// A quest that can be assigned to a player.
/// Contains a name, description, and a list of 
/// [QuestObjective]s.
/// </summary>
[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : LoggingScriptableObject
{
    public string questName;
    [TextArea] public string description;

    public QuestObjective[] objectives;
    public QuestReward[] rewards;
}
