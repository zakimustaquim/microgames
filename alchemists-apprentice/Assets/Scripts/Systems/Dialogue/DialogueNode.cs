using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class representing a single node in a dialogue tree.
/// </summary>
[System.Serializable]
public class DialogueNode
{
    public DialogueCharacter speaker;
    [TextArea] public string dialogueText;
    public Sound audioClip; // optional audio clip for the dialogue
    public List<DialogueOption> options;
}
