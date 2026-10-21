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

    protected virtual void CheckPickup()
    {
        Vector3 newpos = transform.position;
        newpos.y = Player.player.transform.position.y;
        if (Vector3.Distance(Player.player.transform.position, transform.position) < InventoryManager.instance.pickupRadius)
        {
            OnPickup();
        }
    }

    protected virtual void OnPickup()
    {

        //DoPickupAnimation();

        if (inventoryManager.AddItem(itemData))
        {
            print("added item");
            Destroy(gameObject);
        }
        else
            print("Out of inventory space");
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
