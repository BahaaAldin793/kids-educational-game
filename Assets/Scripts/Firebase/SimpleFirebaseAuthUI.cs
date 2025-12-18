using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleFirebaseAuthUI : MonoBehaviour
{
    [Header("UI")]
    public InputField emailInput;
    public InputField passwordInput;
    public Button registerButton;
    public Button loginButton;
    public Text messageText;

    [Header("Scenes")]
    public string avatarSelectSceneName = "AvatarSelect";
    public string mainMenuSceneName = "MainMenu";
    public string loginSceneName = "Login";

    [Header("Options")]
    public bool goToAvatarSelectAfterRegister = true;

    private FirebaseAuth auth;
    private bool firebaseReady = false;

    void Start()
    {
        if (messageText != null) messageText.text = "";

        if (registerButton != null) registerButton.onClick.AddListener(Register);
        if (loginButton != null) loginButton.onClick.AddListener(Login);

        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result != DependencyStatus.Available)
            {
                firebaseReady = false;
                if (messageText != null) messageText.text = "Firebase not ready.";
                Debug.LogError("Firebase dependency error: " + task.Result);
                return;
            }

            auth = FirebaseAuth.DefaultInstance;
            firebaseReady = true;

            // Optional: لو عايز Auto-login لو فيه مستخدم مسجل بالفعل
            // var user = auth.CurrentUser;
            // if (user != null)
            // {
            //     UserData.SetLastUid(user.UserId);
            //     GoNextSceneAfterLogin(user);
            // }
        });
    }

    void Register()
    {
        if (!firebaseReady)
        {
            if (messageText != null) messageText.text = "Firebase loading...";
            return;
        }

        string email = (emailInput != null) ? emailInput.text.Trim() : "";
        string password = (passwordInput != null) ? passwordInput.text : "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            if (messageText != null) messageText.text = "Email and password required.";
            return;
        }

        if (messageText != null) messageText.text = "Creating account...";

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    if (messageText != null) messageText.text = GetErrorMessage(task.Exception);
                    return;
                }

                var user = auth.CurrentUser;
                if (user == null)
                {
                    if (messageText != null) messageText.text = "Account created but user is null.";
                    return;
                }

                // خزّن UID كآخر مستخدم
                UserData.SetLastUid(user.UserId);

                if (goToAvatarSelectAfterRegister)
                {
                    // أول مرة لازم يختار Avatar ويحط Name
                    SceneManager.LoadScene(avatarSelectSceneName);
                }
                else
                {
                    if (messageText != null) messageText.text = "Account created. You can login now.";
                }
            });
    }

    void Login()
    {
        if (!firebaseReady)
        {
            if (messageText != null) messageText.text = "Firebase loading...";
            return;
        }

        string email = (emailInput != null) ? emailInput.text.Trim() : "";
        string password = (passwordInput != null) ? passwordInput.text : "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            if (messageText != null) messageText.text = "Email and password required.";
            return;
        }

        if (messageText != null) messageText.text = "Logging in...";

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    if (messageText != null) messageText.text = GetErrorMessage(task.Exception);
                    return;
                }

                var user = auth.CurrentUser;
                if (user == null)
                {
                    if (messageText != null) messageText.text = "Login failed.";
                    return;
                }

                // خزّن UID كآخر مستخدم
                UserData.SetLastUid(user.UserId);

                // لو عنده Profile محفوظ (name+avatar) -> MainMenu
                // لو مش عنده -> AvatarSelect
                GoNextSceneAfterLogin(user);
            });
    }

    private void GoNextSceneAfterLogin(FirebaseUser user)
    {
        if (user == null)
        {
            SceneManager.LoadScene(loginSceneName);
            return;
        }

        string uid = user.UserId;

        if (UserData.HasProfile(uid))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            SceneManager.LoadScene(avatarSelectSceneName);
        }
    }

    string GetErrorMessage(AggregateException exception)
    {
        FirebaseException fbException = exception.GetBaseException() as FirebaseException;
        if (fbException == null) return "Unknown error.";

        AuthError errorCode = (AuthError)fbException.ErrorCode;

        switch (errorCode)
        {
            case AuthError.InvalidEmail:
                return "Invalid email format.";
            case AuthError.WrongPassword:
                return "Wrong password.";
            case AuthError.UserNotFound:
                return "User not found.";
            case AuthError.EmailAlreadyInUse:
                return "Email already in use.";
            case AuthError.WeakPassword:
                return "Weak password (min 6 chars).";
            case AuthError.NetworkRequestFailed:
                return "Network error.";
            default:
                return errorCode.ToString();
        }
    }
}
