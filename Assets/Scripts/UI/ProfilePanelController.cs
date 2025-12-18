using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfilePanelController : MonoBehaviour
{
    [Header("UI")]
    public Text nameText;   // نفس Text اللي مكتوب فيه Name:
    public Text emailText;  // نفس Text اللي مكتوب فيه Email:

    [Header("Scenes")]
    public string avatarSelectSceneName = "AvatarSelect";
    public string loginSceneName = "Login";

    void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        string uid = UserData.GetLastUid();

        if (nameText != null)
            nameText.text = "Name: " + UserData.GetPlayerName(uid);

        // email: لو فاضي في PlayerPrefs حاول من Firebase
        string email = UserData.GetEmail(uid);
        if (string.IsNullOrEmpty(email))
        {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user != null && !string.IsNullOrEmpty(user.Email))
                email = user.Email;
        }

        if (emailText != null)
            emailText.text = "Email: " + email;
    }

    public void OnChangeAvatar()
    {
        SceneManager.LoadScene(avatarSelectSceneName);
    }

    public void OnLogout()
    {
        try { FirebaseAuth.DefaultInstance.SignOut(); } catch { }
        // مهم: مفيش مسح داتا هنا
        SceneManager.LoadScene(loginSceneName);
    }

}
