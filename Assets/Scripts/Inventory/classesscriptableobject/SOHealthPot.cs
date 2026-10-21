using UnityEngine;


[CreateAssetMenu(fileName = "HealthPotion", menuName = "Roguelike/Items/Health Potion")]

public class SOHealthPot: ItemScriptableObject
{

    public float HPRestoreOnUse = 50;

    public void Awake()
    {
        type = ItemType.Consumable;
    }

    public override void OnUse()
    {
        Player.player.RestoreHealth(HPRestoreOnUse);
    }
}
