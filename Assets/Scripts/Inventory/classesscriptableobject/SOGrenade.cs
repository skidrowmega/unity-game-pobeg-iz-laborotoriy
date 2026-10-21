using UnityEngine;


[CreateAssetMenu(fileName = "Grenade", menuName = "Roguelike/Items/Grenade")]

public class SOGrenade: ItemScriptableObject
{
    private void Awake()
    {
        type = ItemType.Grenade;
    }
}
