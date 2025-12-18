using UnityEngine;

public static class UserData
{
    private const string KEY_LAST_UID = "LAST_UID";

    private static string NameKey(string uid) => $"PLAYER_NAME_{uid}";
    private static string EmailKey(string uid) => $"PLAYER_EMAIL_{uid}";
    private static string AvatarKey(string uid) => $"AVATAR_ID_{uid}";
    private static string HasKey(string uid) => $"HAS_PROFILE_{uid}";

    // -------------------------
    // UID tracking
    // -------------------------
    public static void SetLastUid(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return;
        PlayerPrefs.SetString(KEY_LAST_UID, uid);
        PlayerPrefs.Save();
    }

    public static string GetLastUid()
    {
        return PlayerPrefs.GetString(KEY_LAST_UID, "");
    }

    // -------------------------
    // Per-user profile (UID)
    // -------------------------
    public static void SaveProfile(string uid, string playerName, string email, int avatarId)
    {
        if (string.IsNullOrEmpty(uid)) return;

        PlayerPrefs.SetString(NameKey(uid), string.IsNullOrEmpty(playerName) ? "Unknown" : playerName);
        PlayerPrefs.SetString(EmailKey(uid), string.IsNullOrEmpty(email) ? "" : email);
        PlayerPrefs.SetInt(AvatarKey(uid), avatarId);
        PlayerPrefs.SetInt(HasKey(uid), 1);
        PlayerPrefs.Save();
    }

    public static bool HasProfile(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return false;
        return PlayerPrefs.GetInt(HasKey(uid), 0) == 1;
    }

    public static string GetPlayerName(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return "Unknown";
        return PlayerPrefs.GetString(NameKey(uid), "Unknown");
    }

    public static string GetEmail(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return "";
        return PlayerPrefs.GetString(EmailKey(uid), "");
    }

    public static int GetAvatarId(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return 0;
        return PlayerPrefs.GetInt(AvatarKey(uid), 0);
    }

    // -------------------------
    // Convenience (use last uid)
    // -------------------------
    public static bool HasProfile()
    {
        return HasProfile(GetLastUid());
    }

    public static string GetPlayerName()
    {
        return GetPlayerName(GetLastUid());
    }

    public static string GetEmail()
    {
        return GetEmail(GetLastUid());
    }

    public static int GetAvatarId()
    {
        return GetAvatarId(GetLastUid());
    }

    // -------------------------
    // Compatibility overloads (لو أي سكربت قديم بيناديها)
    // -------------------------
    public static void SaveProfile(string playerName, int avatarId)
    {
        string uid = GetLastUid();
        if (string.IsNullOrEmpty(uid)) return;

        // email ممكن يبقى متخزن سابقًا
        string email = GetEmail(uid);
        SaveProfile(uid, playerName, email, avatarId);
    }

    // -------------------------
    // Clear (مش للـLogout)
    // -------------------------
    public static void ClearUser(string uid)
    {
        if (string.IsNullOrEmpty(uid)) return;
        PlayerPrefs.DeleteKey(NameKey(uid));
        PlayerPrefs.DeleteKey(EmailKey(uid));
        PlayerPrefs.DeleteKey(AvatarKey(uid));
        PlayerPrefs.DeleteKey(HasKey(uid));
        PlayerPrefs.Save();
    }
}
