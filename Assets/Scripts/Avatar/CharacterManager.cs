using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [Header("Data")]
    public CharacterDatabase characterDB;

    [Header("Spawn Point (RECOMMENDED: World object outside Canvas)")]
    public Transform previewSpawnPoint;

    [Header("Preview Settings")]
    [Tooltip("Scale applied to the spawned character instance.")]
    public float previewScale = 6f;

    [Tooltip("Force SpriteRenderer sortingOrder so the character stays visible.")]
    public int previewSortingOrder = 100;

    [Header("Optional UI")]
    public Text characterNameText;

    private int selectedIndex = 0;
    private GameObject currentInstance;

    void Start()
    {
        Debug.Log("[CharacterManager] Start");
        ShowCharacter(selectedIndex);
    }

    public void NextCharacter()
    {
        Debug.Log("[CharacterManager] NextCharacter clicked");
        if (!IsDatabaseReady()) return;

        selectedIndex++;
        if (selectedIndex >= characterDB.CharacterCount)
            selectedIndex = 0;

        ShowCharacter(selectedIndex);
    }

    public void BackCharacter()
    {
        Debug.Log("[CharacterManager] BackCharacter clicked");
        if (!IsDatabaseReady()) return;

        selectedIndex--;
        if (selectedIndex < 0)
            selectedIndex = characterDB.CharacterCount - 1;

        ShowCharacter(selectedIndex);
    }

    public int GetSelectedCharacterIndex()
    {
        return selectedIndex;
    }

    public Character GetSelectedCharacter()
    {
        if (characterDB == null) return null;
        return characterDB.GetCharacter(selectedIndex);
    }

    private bool IsDatabaseReady()
    {
        if (characterDB == null)
        {
            Debug.LogError("[CharacterManager] characterDB is NULL. Assign it in Inspector.");
            return false;
        }

        if (characterDB.CharacterCount <= 0)
        {
            Debug.LogError("[CharacterManager] characterDB has 0 characters. Fill the array in the asset.");
            return false;
        }

        if (previewSpawnPoint == null)
        {
            Debug.LogError("[CharacterManager] previewSpawnPoint is NULL. Assign PreviewSpawnPoint (outside Canvas).");
            return false;
        }

        return true;
    }

    private void ShowCharacter(int index)
    {
        if (!IsDatabaseReady()) return;

        Character c = characterDB.GetCharacter(index);
        if (c == null || c.characterPrefab == null)
        {
            Debug.LogError("[CharacterManager] Character or Prefab is missing at index: " + index);
            return;
        }

        // Clear ALL previous children in spawn point
        for (int i = previewSpawnPoint.childCount - 1; i >= 0; i--)
        {
            Destroy(previewSpawnPoint.GetChild(i).gameObject);
        }

        // Spawn as a child of previewSpawnPoint (world)
        currentInstance = Instantiate(c.characterPrefab, previewSpawnPoint);

        // Reset local transform
        currentInstance.transform.localPosition = Vector3.zero;
        currentInstance.transform.localRotation = Quaternion.identity;
        currentInstance.transform.localScale = Vector3.one * previewScale;

        // Optional: force sorting order to keep it visible
        SpriteRenderer[] renderers = currentInstance.GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sortingOrder = previewSortingOrder;
        }

        if (characterNameText != null)
            characterNameText.text = c.characterName;

        Debug.Log("[CharacterManager] Showing index " + index + " prefab: " + c.characterPrefab.name);
    }
}
