using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class representing a dialogue option.
/// </summary>
[CreateAssetMenu(fileName = "New Dialogue Option", menuName = "Dialogue/Option", order = 1)]
public class DialogueOption : LoggingScriptableObject
{
    public string text;
    public DialogueNode nextNode;
    public bool endsConversation;
    public bool progressesConversation;

    // Conditions that must be met for this option to be available
    public List<IDialogueCondition> conditions;
}
