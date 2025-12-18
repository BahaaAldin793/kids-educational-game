using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Image soundIconOn;
    [SerializeField] private Image soundIconOff;

    [Header("Game SFX (Puzzle)")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip snapClip;
    [SerializeField] private AudioClip winClip;

    private bool muted = false;
    private float previousVolume = 1f;

    private const string VolumePrefKey = "volume";
    private const string MutedPrefKey = "muted";

    private void Awake()
    {
        // Singleton (safe if you only have 1 SoundManager in the game)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Ensure we have an AudioSource for SFX if present on same GO
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();

        Load();

        // Apply loaded state to audio + UI
        ApplyVolume(volumeSlider != null ? volumeSlider.value : 1f, save: false);
        ApplyMute(muted, save: false);

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        UpdateIcons();
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
            volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    // ===== UI Volume / Mute =====

    private void OnVolumeChanged(float value)
    {
        // If user drags volume up while muted, auto-unmute
        if (muted && value > 0.0001f)
            ApplyMute(false, save: true);

        ApplyVolume(value, save: true);
        UpdateIcons();
    }

    public void ToggleMute()
    {
        ApplyMute(!muted, save: true);
        UpdateIcons();
    }

    private void ApplyVolume(float value, bool save)
    {
        AudioListener.volume = Mathf.Clamp01(value);

        // Track last non-zero volume for restoring after unmute
        if (AudioListener.volume > 0.0001f)
            previousVolume = AudioListener.volume;

        if (save)
            Save();
    }

    private void ApplyMute(bool isMuted, bool save)
    {
        muted = isMuted;

        if (muted)
        {
            // store current volume to restore later
            previousVolume = (volumeSlider != null) ? volumeSlider.value : AudioListener.volume;

            AudioListener.volume = 0f;
            if (volumeSlider != null) volumeSlider.value = 0f;
        }
        else
        {
            // restore volume
            float restore = previousVolume <= 0.0001f ? 1f : previousVolume;
            AudioListener.volume = restore;
            if (volumeSlider != null) volumeSlider.value = restore;
        }

        if (save)
            Save();
    }

    private void UpdateIcons()
    {
        bool isOn = !muted && AudioListener.volume > 0.0001f;

        if (soundIconOn != null) soundIconOn.enabled = isOn;
        if (soundIconOff != null) soundIconOff.enabled = !isOn;
    }

    private void Load()
    {
        float vol = PlayerPrefs.GetFloat(VolumePrefKey, 1f);
        muted = PlayerPrefs.GetInt(MutedPrefKey, 0) == 1;

        if (volumeSlider != null)
            volumeSlider.value = Mathf.Clamp01(vol);

        previousVolume = Mathf.Clamp01(vol);
    }

    private void Save()
    {
        float vol = (volumeSlider != null) ? volumeSlider.value : AudioListener.volume;

        PlayerPrefs.SetFloat(VolumePrefKey, Mathf.Clamp01(vol));
        PlayerPrefs.SetInt(MutedPrefKey, muted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // ===== Puzzle SFX API (called by Puzzle GameManager) =====

    public void PlaySnapSound()
    {
        PlayOneShot(snapClip);
    }

    public void PlayWinSound()
    {
        PlayOneShot(winClip);
    }

    private void PlayOneShot(AudioClip clip)
    {
        // Respect mute state
        if (muted || AudioListener.volume <= 0.0001f)
            return;

        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip);
    }
}
