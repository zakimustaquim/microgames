using UnityEngine;

/// <summary>
/// A class representing a dialogue. Typically, each
/// interactable will have one dialogue associated with it.
/// In the event that multiple dialogues are needed for a single
/// interactable based on game state, it may have multiple dialogues.
/// </summary>
public class Dialogue : LoggingScriptableObject
{
    public string dialogueID;
    public DialogueNode startingNode;
}
