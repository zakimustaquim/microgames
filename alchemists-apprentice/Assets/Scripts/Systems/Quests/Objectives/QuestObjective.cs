/// <summary>
/// Base class for a Quest Objective.
/// </summary>
public abstract class QuestObjective : LoggingScriptableObject
{
    public string description;
    
    public abstract QuestObjectiveRuntime CreateRuntime();
}
