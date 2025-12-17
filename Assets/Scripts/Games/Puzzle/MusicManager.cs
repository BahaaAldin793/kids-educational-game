using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    private const string VolumeKey = "MusicVolume";

    private float lastVolume = 0.5f;

    [Header("UI References")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button muteButton;

    private Image muteButtonImage;

    [Header("Mute Icons")]
    [SerializeField] private Sprite unmuteIcon;
    [SerializeField] private Sprite muteIcon;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (muteButton != null)
        {
            muteButtonImage = muteButton.GetComponent<Image>();
        }
    }

    void Start()
    {

        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0.5f);

        audioSource.volume = 0f;



        lastVolume = savedVolume;



        if (volumeSlider != null)
        {
            volumeSlider.value = 0f;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }


        if (muteButton != null)
        {
            muteButton.onClick.AddListener(ToggleMute);
        }

        UpdateMuteButtonIcon();
    }

    public void SetVolume(float volume)
    {

        audioSource.volume = volume;
        PlayerPrefs.SetFloat(VolumeKey, volume);
        PlayerPrefs.Save();


        if (volume > 0)
        {
            lastVolume = volume;
        }


        audioSource.mute = (volume == 0);

        UpdateMuteButtonIcon();
    }

    public void ToggleMute()
    {

        if (audioSource.volume > 0)
        {

            lastVolume = audioSource.volume;


            audioSource.volume = 0f;
            if (volumeSlider != null)
            {
                volumeSlider.value = 0f;
            }
        }

        else
        {


            audioSource.volume = lastVolume;
            if (volumeSlider != null)
            {
                volumeSlider.value = lastVolume;
            }
        }


        audioSource.mute = (audioSource.volume == 0);


        PlayerPrefs.SetFloat(VolumeKey, audioSource.volume);
        PlayerPrefs.Save();

        UpdateMuteButtonIcon();
    }


    private void UpdateMuteButtonIcon()
    {
        if (muteButtonImage == null || muteIcon == null || unmuteIcon == null)
        {
            return;
        }


        if (audioSource.volume == 0)
        {
            muteButtonImage.sprite = muteIcon;
        }
        else
        {
            muteButtonImage.sprite = unmuteIcon;
        }
    }
}