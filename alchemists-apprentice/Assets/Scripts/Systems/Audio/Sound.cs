using UnityEngine;

[System.Serializable]
public class Sound : LoggingScriptableObject
{
    public string soundName;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.5f;

    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    public bool loop = false;
}
