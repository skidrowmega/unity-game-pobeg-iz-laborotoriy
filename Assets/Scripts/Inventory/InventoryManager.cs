using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Execution;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager: MonoBehaviour
{

    [Header("Inventory Logic")]
    //public int numberOfSlots = 10;
    private InventorySlot[,] inventorySlots;
    [SerializeField] private GameObject slotsParent;
    Image[,] inventoryImageSlots;

    [SerializeField] private Vector2Int inventorydimensions=new Vector2Int(5,2);


    
    //public LayerMask itemLayer;
    public static float pickupRadius = 3f;
    //public float checkInteval = 0.2f;
    //private float nextCheckTime = 0f;

    public static InventoryManager instance;

    private void Awake()
    {
        instance = this;
        inventorySlots = new InventorySlot[inventorydimensions.x, inventorydimensions.y];
        Image[] tempimage = slotsParent.GetComponentsInChildren<Image>();
        inventoryImageSlots = new  Image[inventorydimensions.x, inventorydimensions.y];
        int count = 0;
        for (int y = 0; y < inventorydimensions.y; y++)
        {
            for (int x = 0; x < inventorydimensions.x; x++)
            {
                inventoryImageSlots[x, y] = tempimage[count];
                count++;
                inventorySlots[x, y] = new InventorySlot();
            }
        }

    }

    private void Start()
    {
        foreach (Image slotImage in inventoryImageSlots)
        {
            GameObject slotObject = slotImage.gameObject;
            EventTrigger trigger = slotObject.GetComponent<EventTrigger>();

            EventTrigger.Entry entryDrag = new EventTrigger.Entry();

            entryDrag.eventID = EventTriggerType.Drag;

            entryDrag.callback.AddListener((data) => OnDrag((PointerEventData)data));

            EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
            entryPointerDown.eventID= EventTriggerType.PointerDown;
            entryPointerDown.callback.AddListener((data) => OnPointerDown((PointerEventData)data));

            trigger.triggers.Add(entryDrag);
            trigger.triggers.Add(entryPointerDown);
        }
    }

    public void AddItem(ItemScriptableObject _item)
    {
        bool itemfound=false;
        for (int y = 0; y < inventorydimensions.y; y++)
        {
            for (int x = 0; x < inventorydimensions.x; x++)
            {
                if (inventorySlots[x, y].isEmpty)
                {
                    inventorySlots[x,y].itemDataSlot= _item;
                    inventorySlots[x, y].isEmpty=false;
                    itemfound=true;
                    break;
                }
            }
            if (itemfound) break;
        }
        UpdateImages();
    }

    private void swapitems(Vector2Int from, Vector2Int to)
    {
        InventorySlot temp = inventorySlots[from.x,from.y];
        inventorySlots[from.x, from.y] = inventorySlots[to.x, to.y];
        inventorySlots[to.x,to.y]= temp;
        UpdateImages();
    }



    bool isDragging = false;
    Vector2Int dragStartIndex;

    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {

        }
        if (data.button == PointerEventData.InputButton.Right)
        {
            isDragging = true;

        }
    }


    public void OnDrag(PointerEventData data)
    {
        if (!isDragging && data.button != PointerEventData.InputButton.Right&& !data.pointerCurrentRaycast.gameObject) return;
        Vector2Int index = SlotObjectToSlotIndex(data.pointerCurrentRaycast.gameObject);
        print(index);
    }

    public void ThrowItem(Vector2Int itemIndex,Vector3 position)
    {
        InventorySlot item = inventorySlots[itemIndex.x,itemIndex.y];
        inventorySlots[itemIndex.x, itemIndex.y] = new InventorySlot();
        Item.SpawnItem(item.itemDataSlot, position);
        UpdateImages();
    }

    private Vector2Int SlotObjectToSlotIndex(GameObject gameObject)
    {
        Image image=null;
        if(gameObject != null)
            image = gameObject.GetComponent<Image>();
        if (image == null) return new Vector2Int(-1, -1);
        for (int y = 0; y < inventorydimensions.y; y++)
        {
            for (int x = 0; x < inventorydimensions.x; x++)
            {
                if (image == inventoryImageSlots[x, y]) return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1,-1);
    }


    void UpdateImages()
    {
        for (int y = 0; y < inventorydimensions.y; y++)
        {
            for (int x = 0; x < inventorydimensions.x; x++)
            {
                InventorySlot slot = inventorySlots[x, y];
                if (!slot.isEmpty)
                {
                    inventoryImageSlots[x, y].color = Color.green;
                }
            }
        }
    }

}
