using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] QuizManager quizManager;

    [Header("UI")]
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject gamePanel;

    void Start()
    {
        
    }

    public void StartGame()
    {
       
            startPanel.SetActive(false);

       
            gamePanel.SetActive(true);

        
        quizManager.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
   
}
