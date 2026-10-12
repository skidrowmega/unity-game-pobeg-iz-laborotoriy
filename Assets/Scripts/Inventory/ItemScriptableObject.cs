using UnityEngine;

public enum ItemType { WeaponMeleeLog, WeaponRangedLog, HealLog}

[CreateAssetMenu(fileName = "Item", menuName = "Roguelike/Items")]

public class ItemScriptableObject: ScriptableObject
{
    public ItemType itemType;
    public string itemName;
}
