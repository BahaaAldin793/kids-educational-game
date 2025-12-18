using UnityEngine;
using UnityEngine.UI;

public class MainMenuHeaderUI : MonoBehaviour
{
    [Header("UI")]
    public Text nameText;

    [Header("Avatar Data")]
    public CharacterDatabase characterDB;

    [Header("World Spawn (must be under BackgroundWorld)")]
    public Transform avatarSpawnPoint;

    [Header("Tuning")]
    public Vector3 avatarLocalScale = new Vector3(1f, 1f, 1f);
    public int avatarSortingOrder = 50;

    private GameObject currentAvatar;

    void Start()
    {
        // 1) Set player name
        string playerName = UserData.GetPlayerName();
        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Player";

        if (nameText != null)
            nameText.text = playerName;

        // 2) Spawn avatar next to the name
        SpawnAvatar();
    }

    private void SpawnAvatar()
    {
        if (characterDB == null || avatarSpawnPoint == null)
            return;

        int avatarId = UserData.GetAvatarId();
        if (avatarId < 0 || avatarId >= characterDB.CharacterCount)
            avatarId = 0;

        Character c = characterDB.GetCharacter(avatarId);
        if (c == null || c.characterPrefab == null)
            return;

        // Clear previous
        if (currentAvatar != null)
            Destroy(currentAvatar);

        currentAvatar = Instantiate(c.characterPrefab, avatarSpawnPoint);

        currentAvatar.transform.localPosition = Vector3.zero;
        currentAvatar.transform.localRotation = Quaternion.identity;
        currentAvatar.transform.localScale = avatarLocalScale;

        // Ensure it renders above background
        var sr = currentAvatar.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = avatarSortingOrder;
        }
    }
}
