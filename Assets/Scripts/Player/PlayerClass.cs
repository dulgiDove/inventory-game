using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    public CharacterClass currentClass = CharacterClass.Warrior;

    public void ChangeClass(CharacterClass newClass)
    {
        currentClass = newClass;
        Debug.Log($"직업 변경: {newClass}");
    }
}