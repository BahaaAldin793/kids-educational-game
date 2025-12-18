using UnityEngine;
using UnityEngine.SceneManagement;

public class GamesPanelController : MonoBehaviour
{
    [Header("Scenes Names (must match Build Settings)")]
    public string puzzleSceneName = "PuzzleGame";
    public string memorySceneName = "MemoryGame";
    public string findItSceneName = "FindTheCorrectOne"; 
    public void OpenPuzzleGame()
    {
        SceneManager.LoadScene(puzzleSceneName);
    }

    public void OpenMemoryMatch()
    {
        SceneManager.LoadScene(memorySceneName);
    }

    public void OpenFindIt()
    {
        SceneManager.LoadScene(findItSceneName);
    }
}
