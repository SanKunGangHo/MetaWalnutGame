using UnityEngine;

[CreateAssetMenu(fileName = "Specialty_Data", menuName = "ScriptableObj/Specialty_Data", order = int.MaxValue)]
public class SpecialtyData : ScriptableObject
{
    public string SpecialtyName;
    public Sprite SpecialtyImage;
    public int SpecialtyPoint;
}
