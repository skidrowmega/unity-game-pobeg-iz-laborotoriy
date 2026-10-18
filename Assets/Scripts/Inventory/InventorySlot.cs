using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[Serializable]
public class InventorySlot
{
    [SerializeField] public ItemScriptableObject itemDataSlot;
    [SerializeField] public bool isEmpty = true;
}
