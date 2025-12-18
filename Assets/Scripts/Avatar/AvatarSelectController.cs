using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AvatarSelectController : MonoBehaviour
{
    [Header("References")]
    public CharacterManager characterManager;
    public InputField playerNameInput;
    public Button chooseButton;

    [Header("Optional UI")]
    public Text messageText;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        if (messageText != null) messageText.text = "";

        if (chooseButton != null)
            chooseButton.onClick.AddListener(OnChooseClicked);
    }

    void OnChooseClicked()
    {
        if (characterManager == null)
        {
            SetMessage("CharacterManager reference missing.");
            return;
        }

        string playerName = playerNameInput != null ? playerNameInput.text.Trim() : "";
        if (string.IsNullOrEmpty(playerName))
        {
            SetMessage("Please enter your name.");
            return;
        }

        int avatarId = characterManager.GetSelectedCharacterIndex();
        UserData.SaveProfile(playerName, avatarId);

        SceneManager.LoadScene(mainMenuSceneName);
    }

    void SetMessage(string msg)
    {
        if (messageText != null) messageText.text = msg;
    }
}
