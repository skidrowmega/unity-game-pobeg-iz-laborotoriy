using System;
using UnityEngine;

public class ItemScriptableObject: ScriptableObject
{
    public string itemName;
    public GameObject itemprefab;


    public virtual void OnPickup()
    {

    }

    public virtual void OnUse()
    {

    }
}
