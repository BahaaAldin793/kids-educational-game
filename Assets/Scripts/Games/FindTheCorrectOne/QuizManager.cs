using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionUIText;
    public TextMeshProUGUI scoreUIText; 
    public Image[] buttonImages;
    public GameObject winPanel;
    public TextMeshProUGUI finalScoreText; 
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI progressText;
    public GameObject GamePanel;
    private Sprite[] allSprites;
    private List<Sprite> unusedSprites;
    private int score = 0; 

    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip correctSound; 
    public AudioClip wrongSound;
    void Start()
    {
        allSprites = Resources.LoadAll<Sprite>("Animals");
        unusedSprites = new List<Sprite>(allSprites);
        if (feedbackText != null) feedbackText.text = "";
        UpdateScoreUI();
        NextQuestion();
    }

    public void NextQuestion()
    {
        if (unusedSprites.Count == 0)
        {
            ShowWinPanel();
            
            return;
        }

        int randomIndex = Random.Range(0, unusedSprites.Count);
        Sprite correctSprite = unusedSprites[randomIndex];
        questionUIText.text = "Find the " + correctSprite.name;
        progressText.text = (allSprites.Length - unusedSprites.Count + 1 ) + " / " + allSprites.Length;
        List<Sprite> wrongSprites = allSprites
            .Where(s => s != correctSprite)
            .OrderBy(x => Random.value)
            .Take(2)
            .ToList();

        List<Sprite> displaySprites = new List<Sprite> { correctSprite };
        displaySprites.AddRange(wrongSprites);
        displaySprites = displaySprites.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < buttonImages.Length; i++)
        {
            buttonImages[i].sprite = displaySprites[i];
            bool isCorrect = (displaySprites[i] == correctSprite);
            SetupButton(buttonImages[i].GetComponent<Button>(), isCorrect);
        }

        unusedSprites.RemoveAt(randomIndex);
    }

    void SetupButton(Button btn, bool isCorrect)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => {
            if (isCorrect)
            {
                score++; 
                if (audioSource != null && correctSound != null)
                    audioSource.PlayOneShot(correctSound);
                if (feedbackText != null)
                {
                    feedbackText.text = "Correct!";
                    feedbackText.color = Color.green; 
                }
            }
            else
            {
                if (audioSource != null && wrongSound != null)
                    audioSource.PlayOneShot(wrongSound);
                if (feedbackText != null)
                {
                    feedbackText.text = "Wrong!";
                    feedbackText.color = Color.red; 
                }
            }

            UpdateScoreUI(); 
            NextQuestion(); 
        });
    }

    void UpdateScoreUI()
    {
        if (scoreUIText != null)
            scoreUIText.text = "Score: " + score;
    }

    void ShowWinPanel()
    {
        
        GamePanel.SetActive(false);
        winPanel.SetActive(true);
        if (finalScoreText != null)
            finalScoreText.text = "Your Total Score: " + score;
    }
}