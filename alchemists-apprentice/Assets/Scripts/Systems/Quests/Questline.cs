using UnityEngine;

/// <summary>
/// A series of quests that can be assigned to a player.
/// </summary>
[CreateAssetMenu(menuName = "Quests/Questline")]
public class Questline : LoggingScriptableObject
{
    public string questlineName;
    [TextArea] public string description;

    public Quest[] quests;

    public override string ToString()
    {
        return $"[Questline: questlineName={questlineName}, description={description}, quests={quests.Length}]";
    }
}
