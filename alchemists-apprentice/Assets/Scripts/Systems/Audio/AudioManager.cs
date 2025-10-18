using UnityEngine;

/// <summary>
/// A Singleton class that manages audio playback.
/// </summary>
public class AudioManager : LoggingMonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [Tooltip("The source for background music")] public AudioSource musicSource;
    [Tooltip("The source for UI sounds")] public AudioSource uiSfxSource;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySfx(AudioSource source, Sound sound)
    {
        if (sound.loop)
        {
            PlayLooping(source, sound);
        }
        else
        {
            PlayOneShot(source, sound);
        }
    }

    private void PlayOneShot(AudioSource source, Sound sound)
    {
        log($"Playing one-shot sound: {sound.soundName}", 1);
        source.PlayOneShot(sound.clip, sound.volume);
    }

    private void PlayLooping(AudioSource source, Sound sound)
    {
        log($"Playing looping sound: {sound.soundName}", 1);
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.loop = true;
        source.Play();
    }

    public void PlayUiSfx(Sound sound) => PlaySfx(uiSfxSource, sound);
}
