using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot: MonoBehaviour
{
    public ItemScriptableObject itemDataSlot;
    //public int amount;
    //public bool isEmpty = true;

    public InventorySlot(ItemScriptableObject _item)
    {
        itemDataSlot = _item;
    }
}
