using System.Collections.Generic;
using UnityEngine;

public class InventoryManager: MonoBehaviour
{
    [Header("UI Panels")]
    public Transform inventoryPanel;
    public GameObject uiPanel; 

    [Header("Inventory Logic")]
    public List<InventorySlot> slots = new List<InventorySlot>();

    private bool isOpen = false;

    private void Awake()
    {
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            InventorySlot slot = inventoryPanel.GetChild(i).GetComponent<InventorySlot>();
            if (slot != null)
            {
                slots.Add(slot);
            }
        }

        uiPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            uiPanel.SetActive(isOpen);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Item itemScript = other.GetComponent<Item>();

        if (itemScript != null)
        {
            AddItem(itemScript.item, itemScript.amount);

            Destroy(other.gameObject);

            Debug.Log("Предмет успешно автоподобран: " + itemScript.item.itemName);
        }
    }
    private void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                slot.amount += _amount;
                return;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;

                return;
            }
        }
    }
}
