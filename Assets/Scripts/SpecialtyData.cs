using UnityEngine;

[CreateAssetMenu(fileName = "Specialty_Data", menuName = "ScriptableObj/Specialty_Data", order = int.MaxValue)]
public class SpecialtyData : ScriptableObject
{
    [Header("코드에 쓸 거")]
    public string SpecialtyName;
    public int SpecialtyPoint;

    [Header("설명에 쓸 거")]
    public Sprite SpecialtyImage;
    public string SpecialtyName_Korean;
    [TextArea(3, 5)]
    public string SpecialtyExplain;

    
}
