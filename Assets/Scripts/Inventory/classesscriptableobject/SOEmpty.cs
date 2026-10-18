using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Empty", menuName = "Roguelike/Items/EmptyItem")]
public class SOEmpty : ItemScriptableObject
{
    SOEmpty()
    {
        itemName= "None";
        itemprefab=null;
    }
}
