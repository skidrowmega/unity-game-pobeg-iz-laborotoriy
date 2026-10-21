using System;
using UnityEngine;
public enum ItemType
{
    Consumable,
    Log,
    Grenade,
    Equipment


}
public abstract class ItemScriptableObject: ScriptableObject
{
    
    public string itemName;
    public ItemType type;
    public GameObject itemprefab;
    public Sprite ItemImage;
    public bool Consumedonuse = true;

    public virtual void OnPickup()
    {

    }

    public virtual void OnUse()
    {
/*        if (itemName == "Grenade")
        {
            Debug.Log("clicked RMB on grenade");
        }*/
    }
}
