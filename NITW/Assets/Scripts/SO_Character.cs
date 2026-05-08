using UnityEngine;

[CreateAssetMenu(fileName = "SO_Character", menuName = "Scriptable Objects/SO_Character")]
public class SO_Character : ScriptableObject
{
    public string characterName;

    public Sprite characterHead, characterFull;
}
