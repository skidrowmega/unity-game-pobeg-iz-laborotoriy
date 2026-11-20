using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class ShopSlot
{
    [SerializeField] public ItemScriptableObject itemDataSlot;
    [SerializeField] public bool isEmpty = true;
    public int BuyCount = 0;
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

    public static ShopScript instance { get; private set; }

    public bool IsOpen { get; private set; }

    GameObject[] DETI;

    //Тут нужен будет массив с соответствием между слотом и предметом(вроде сделано)

    private void Awake()
    {
        instance = this;
        IsOpen = false;
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
        DETI = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            DETI[i] = transform.GetChild(i).gameObject;
        }
        DisableWindows();


    }

    private void Start()
    {
#if UNITY_EDITOR
        if (debug)
        {
            AddItem(itemtoadd);
        }
#endif
        UpdateShopCurrency();
    }


    public void OpenShop()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Shop);
        }
        IsOpen = true;
        EnableWindows();
        UpdateShopCurrency();
        UpdateItems();
    }

    public void CloseShop()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Shop)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.Playing);
        }
        DisableWindows();
        IsOpen = false;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                OpenShop();
                for (int i = 0; i < 10; i++)
                {
                    AddItem(itemtoadd);
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                CloseShop();
            }
        }
    }
#endif


    void EnableWindows()
    {
        for (int i = 0; i < DETI.Length; i++)
        {
            DETI[i].SetActive(true);
        }
    }

    void DisableWindows()
    {
        for (int i = 0; i < DETI.Length; i++)
        {
            DETI[i].SetActive(false);
        }
    }

    public void AddItem(ItemScriptableObject itemdata)
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
        if (PlayerCurrency.instance.GetCurrency() >= GetModifiedPrice(slot) && InventoryManager.instance.AddItem(slot.itemDataSlot))
        {
            PlayerCurrency.instance.SubstractCurrency(GetModifiedPrice(slot));
            //InventoryManager.instance.AddItem(slot.itemDataSlot);
            //RemoveItem(slot);
            slot.BuyCount += 1;
            UpdateShopCurrency();
            UpdateShopSlotValue(slot);
        }

    }

    /*public void RemoveItem(ShopSlot slot)
    {
        slot.isEmpty = true;
        UpdateItems();
    }

    public void RemoveAllItems()
    {
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i].isEmpty == true)
            {
                break;
            }
            ItemSlots[i].isEmpty = true;
            ItemSlots[i].itemDataSlot = null;
        }
    }*/

    private void UpdateShopSlotValue(ShopSlot slot)
    {
        ItemScriptableObject item = slot.itemDataSlot;
        ShopDescription.text = item.ItemDescription + "\n" + $"Стоимость: {GetModifiedPrice(slot)}";
    }

    void UpdateShopCurrency()
    {
        CurrencyText.text = PlayerCurrency.instance.GetCurrency().ToString();
    }


    void OnPointerDown(PointerEventData eventData)
    {
        ShopSlot slot = ObjectToSlot(eventData.pointerCurrentRaycast.gameObject);

        if (!slot.isEmpty) BuyItem(slot);
    }
    void OnPointerEnter(PointerEventData eventData)
    {
        ShopSlot slot = ObjectToSlot(eventData.pointerCurrentRaycast.gameObject);
        if (!slot.isEmpty) ShopDescription.text = slot.itemDataSlot.ItemDescription + "\n" + $"Стоимость: {GetModifiedPrice(slot)}";
    }
    void OnPointerExit(PointerEventData eventData)
    {
        ShopDescription.text = "";
    }

    ShopSlot ObjectToSlot(GameObject gobject)
    {
        int index = SlotObjects.IndexOf(gobject.transform.parent.gameObject);
        return ItemSlots[index];
    }

    private int GetModifiedPrice(ShopSlot slot)
    {
        return (int)(AmuletManager.instance.GetPriceWithDiscount(slot.itemDataSlot.Value) * Mathf.Pow(1.5f, slot.BuyCount));
    }
}

