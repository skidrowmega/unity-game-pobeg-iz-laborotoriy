using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Item: MonoBehaviour
{
    public ItemScriptableObject itemData;

    InventoryManager inventoryManager;

    private void Update()
    {
        CheckPickup();
    }

    private void Start()
    {
        inventoryManager=InventoryManager.instance;
    }

    private void CheckPickup()
    {
        if (Vector3.Distance(Player.player.transform.position, transform.position) < InventoryManager.pickupRadius)
        {
            OnPickup();
        }
    }

    private void OnPickup()
    {
        print("added item");
        //DoPickupAnimation();
        inventoryManager.AddItem(itemData);
        Destroy(gameObject);
    }

    private void DoPickupAnimation()
    {
        throw new NotImplementedException();
    }

    public static Item SpawnItem(ItemScriptableObject itemData, Vector3 position)
    {
        Item _item = Instantiate(itemData.itemprefab,position,Quaternion.identity).GetComponent<Item>();
        print(_item.name);
        _item.itemData=itemData;
        return _item;
    }

}
