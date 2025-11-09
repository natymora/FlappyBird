using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Create Character")]
public class PlayerConfigurations : ScriptableObject
{
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;
}
