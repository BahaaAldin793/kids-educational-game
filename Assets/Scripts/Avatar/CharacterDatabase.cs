using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "KidsGame/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    public Character[] characters;

    public int CharacterCount
    {
        get { return characters != null ? characters.Length : 0; }
    }

    public Character GetCharacter(int index)
    {
        if (characters == null || characters.Length == 0) return null;
        if (index < 0 || index >= characters.Length) return null;
        return characters[index];
    }
}
