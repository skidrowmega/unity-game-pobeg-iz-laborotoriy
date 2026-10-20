using System;
using UnityEngine;

public class ItemScriptableObject: ScriptableObject
{
    public string itemName;
    public GameObject itemprefab;
    public Sprite ItemImage;
    public bool Consumedonuse = true;

    public virtual void OnPickup()
    {

    }

    public virtual void OnUse()
    {
        if (itemName == "Grenade")
        {
            Debug.Log("clicked RMB on grenade");
        }
    }
}
