using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float startTime;
    private bool isRunning = false;

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            timerText.text = "00:00.00";
        }
    }
    public float StopTimer()
    {
        isRunning = false;

        return Time.time - startTime;
    }

    public void ResetTimer()
    {
        isRunning = false;
        if (timerText != null)
        {

            timerText.gameObject.SetActive(false);
            timerText.text = "00:00";
        }
    }

    void Update()
    {

        if (isRunning && timerText != null)
        {
            float t = Time.time - startTime;

            int totalSeconds = Mathf.FloorToInt(t);

            string minutes = (totalSeconds / 60).ToString("00");
            string seconds = (totalSeconds % 60).ToString("00");


            timerText.text = $"{minutes}:{seconds}";

        }
    }
}