public abstract class QuestObjectiveRuntime : Logging
{
    public QuestObjective objectiveData;
    public int currentProgress;
    public bool completed;

    public QuestObjectiveRuntime(QuestObjective objectiveData)
    {
        this.objectiveData = objectiveData;
        currentProgress = 0;
        completed = false;
    }
}
