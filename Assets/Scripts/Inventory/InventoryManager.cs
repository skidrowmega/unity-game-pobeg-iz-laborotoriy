using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Execution;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager: MonoBehaviour
{

    [Header("Inventory Logic")]
    public int numberOfSlots = 10;
    private List<InventorySlot> slots = new List<InventorySlot>();
    public float pickupRadius = 1f;
    public LayerMask itemLayer;
    public float checkInteval = 0.2f;
    private float nextCheckTime = 0f;

    private void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInteval;
            Collider[] itemsCount = Physics.OverlapSphere(transform.position, pickupRadius, itemLayer);

            foreach (Collider item in itemsCount)
            {
                if (item.TryGetComponent<Item>(out var pickupableItem))
                {
                    AddItem(pickupableItem.itemData);
                    Destroy(pickupableItem.gameObject);
                }
            }
        }
    }

    private void AddItem(ItemScriptableObject _item)
    {
        if (slots.Count < numberOfSlots)
        {
            slots.Add(new InventorySlot(_item));
        }
    }
}
