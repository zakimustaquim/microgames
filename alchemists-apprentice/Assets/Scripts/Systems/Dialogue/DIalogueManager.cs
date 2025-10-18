using System;

public class DialogueManager : LoggingMonoBehaviour
{
    public static DialogueManager instance;
    private Dialogue currentDialogue;
    private DialogueNode currentNode;

    public event Action<DialogueNode> OnNodeChanged;
    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;

    protected override void Awake()
    {
        base.Awake();

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentNode = dialogue.startingNode;

        log($"Starting dialogue: {dialogue}");
        OnDialogueStarted?.Invoke();
        OnNodeChanged?.Invoke(currentNode);
    }

    public void EndDialogue()
    {
        log($"Ending dialogue: {currentDialogue}");
        currentDialogue = null;
        currentNode = null;
        OnDialogueEnded?.Invoke();
    }

}
