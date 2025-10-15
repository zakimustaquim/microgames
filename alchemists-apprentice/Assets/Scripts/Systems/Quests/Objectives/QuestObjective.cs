/// <summary>
/// Base class for a Quest Objective.
/// </summary>
public abstract class QuestObjective : LoggingScriptableObject
{
    public abstract QuestObjectiveRuntime CreateRuntime();
}
