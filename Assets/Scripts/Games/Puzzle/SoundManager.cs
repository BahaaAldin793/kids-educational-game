using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip snapSound;
    [SerializeField] private AudioClip winSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            enabled = false;
        }
    }

    public void PlaySnapSound()
    {


        if (snapSound != null)
        {
            audioSource.PlayOneShot(snapSound);
        }
    }
    public void PlayWinSound()
    {
        if (winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
    }
}