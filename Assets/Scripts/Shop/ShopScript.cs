using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopSlot
{
    [SerializeField] public ItemScriptableObject itemDataSlot;
    [SerializeField] public bool isEmpty = true;
}

public class ShopScript : MonoBehaviour
{

    [SerializeField] RectTransform WaresList;
    [SerializeField] Text ShopDescription;
    [SerializeField] Text CurrencyText;
    List<Image> SlotImages;
    List<GameObject> SlotObjects;
    ShopSlot[] ItemSlots;
    [SerializeField] Sprite emptybackground;

#if UNITY_EDITOR
    [SerializeField] bool debug = false;
    [SerializeField] ItemScriptableObject itemtoadd;
#endif

    //Тут нужен будет массив с соответствием между слотом и предметом

    private void Awake()
    {
        ItemSlots = new ShopSlot[WaresList.childCount];
        SlotImages = new List<Image>();
        SlotObjects = new List<GameObject>();
        for (int i = 0; i < WaresList.childCount; i++)
        {
            Transform child = WaresList.GetChild(i);
            SlotImages.Add(child.GetComponentInChildren<Image>());
            ItemSlots[i] = new ShopSlot
            {
                itemDataSlot = null,
                isEmpty = true
            };






            GameObject slotObject = WaresList.GetChild(i).gameObject;
            SlotObjects.Add(slotObject);
            EventTrigger trigger = slotObject.GetComponent<EventTrigger>();
            EventTrigger.Entry entryPointerEnter = new EventTrigger.Entry();
            EventTrigger.Entry entryPointerExit = new EventTrigger.Entry();
            EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();

            entryPointerDown.eventID = EventTriggerType.PointerDown;
            entryPointerEnter.eventID = EventTriggerType.PointerEnter;
            entryPointerExit.eventID = EventTriggerType.PointerExit;

            entryPointerDown.callback.AddListener((data) => OnPointerDown((PointerEventData)data));
            entryPointerEnter.callback.AddListener((data) => OnPointerEnter((PointerEventData)data));
            entryPointerExit.callback.AddListener((data) => OnPointerExit((PointerEventData)data));

            trigger.triggers.Add(entryPointerDown);
            trigger.triggers.Add(entryPointerEnter);
            trigger.triggers.Add(entryPointerExit);
        }


    }
#if UNITY_EDITOR
    private void Start()
    {
        if (debug)
        {
            AddItem(itemtoadd);
        }
        UpdateShopCurrency();
    }

#endif
    public void OpenShop()
    {
        UpdateShopCurrency();
        UpdateItems();
    }

    void AddItem(ItemScriptableObject itemdata)
    {
        ShopSlot slot = GetFirstEmptySlot();
        if (slot != null)
        {
            slot.itemDataSlot = itemdata;
            slot.isEmpty = false;
        }
        UpdateItems();
    }

    ShopSlot GetFirstEmptySlot()
    {
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i].isEmpty)
            {
                return ItemSlots[i];
            }
        }
        return null;
    }

    void UpdateItems()
    {
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (!ItemSlots[i].isEmpty)
            {
                SlotImages[i].sprite = ItemSlots[i].itemDataSlot.ItemImage;
            }
            else
            {
                SlotImages[i].sprite = emptybackground;
            }
        }
    }

    void BuyItem(ShopSlot slot)
    {
        if (PlayerCurrency.instance.GetCurrency() >= slot.itemDataSlot.Value)
        {
            PlayerCurrency.instance.SubstractCurrency(slot.itemDataSlot.Value);
            InventoryManager.instance.AddItem(slot.itemDataSlot);
            RemoveItem(slot);
            UpdateShopCurrency();
        }
        
    }

    void RemoveItem(ShopSlot slot)
    {
        slot.isEmpty = true;
        UpdateItems();
    }


    void UpdateShopCurrency()
    {
        CurrencyText.text=PlayerCurrency.instance.GetCurrency().ToString();
    }


    void OnPointerDown(PointerEventData eventData)
    {
        ShopSlot slot = ObjectToSlot(eventData.pointerCurrentRaycast.gameObject);

        if(!slot.isEmpty)BuyItem(slot);
    }
    void OnPointerEnter(PointerEventData eventData)
    {
        ItemScriptableObject item = ObjectToSlot(eventData.pointerCurrentRaycast.gameObject).itemDataSlot;
        if(item)ShopDescription.text=item.ItemDescription+"\n"+$"Стоимость: {item.Value}";
    }
    void OnPointerExit(PointerEventData eventData)
    {
        ShopDescription.text = "";
    }

    ShopSlot ObjectToSlot(GameObject gobject)
    {
        int index = SlotObjects.IndexOf(gobject.transform.parent.gameObject);
        return ItemSlots[index] ;
    }

}

