using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class representing a speaker. Used to define 
/// speaker names, portraits, and default voice clips.
/// </summary>
[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
public class DialogueCharacter : LoggingScriptableObject
{
    public string characterName;
    public Sprite characterPortrait;
    public List<AudioClip> voiceClips; // optional list of voice clips for the character - will be randomly selected when they speak if no specific clip provided
}
