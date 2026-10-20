using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Execution;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
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



    Vector3[] slotpositions;
    float slotheight=-1;
    float slotwidth=-1;


    Canvas canvas;

    private void Awake()
    {
        canvas =gameObject.transform.parent.GetComponent<Canvas>();
        instance = this;
        inventorySlots = new InventorySlot[inventorydimensions.x, inventorydimensions.y];
        Image[] tempimage = slotsParent.GetComponentsInChildren<Image>();
        emptybackground = tempimage[0].sprite;
        slotpositions = new Vector3[tempimage.Length];
        inventoryImageSlots = new  Image[inventorydimensions.x, inventorydimensions.y];
        int count = 0;
        for (int y = 0; y < inventorydimensions.y; y++)
        {
            for (int x = 0; x < inventorydimensions.x; x++)
            {
                inventoryImageSlots[x, y] = tempimage[count];
                inventorySlots[x, y] = new InventorySlot();
                slotpositions[count] = inventoryImageSlots[x, y].GetComponent<RectTransform>().position;
                if (slotheight < 0)
                {
                    RectTransform rectTransform = inventoryImageSlots[x, y].GetComponent<RectTransform>();
                    slotheight = rectTransform.sizeDelta.y;
                    slotwidth = rectTransform.sizeDelta.x;
                }
                count++;
            }
        }
    }

    private void Start()
    {
        foreach (Image slotImage in inventoryImageSlots)
        {
            GameObject slotObject = slotImage.gameObject;
            EventTrigger trigger = slotObject.GetComponent<EventTrigger>();

            //EventTrigger.Entry entryDrag = new EventTrigger.Entry();

            //entryDrag.eventID = EventTriggerType.Move;

            //entryDrag.callback.AddListener((data) => OnHover((PointerEventData)data));

            EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
            entryPointerDown.eventID= EventTriggerType.PointerDown;
            entryPointerDown.callback.AddListener((data) => OnPointerDown((PointerEventData)data));

            //trigger.triggers.Add(entryDrag);
            trigger.triggers.Add(entryPointerDown);
        }
    }

    private void Update()
    {
        DoDrag();
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
    RectTransform beingdragged = null;
    Vector3 startposition;
    private Sprite emptybackground;

    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {

            if (!isDragging)
            {
                beingdragged = data.pointerCurrentRaycast.gameObject.GetComponent<RectTransform>();
                dragStartIndex = SlotObjectToSlotIndex(beingdragged.gameObject);
                if (inventorySlots[dragStartIndex.x,dragStartIndex.y].isEmpty) return;
                beingdragged.transform.SetAsLastSibling();
                if (dragStartIndex != new Vector2Int(-1, -1))
                {
                    isDragging = true;
                    startposition = beingdragged.GetComponent<RectTransform>().position;
                }
            }
            else
            {

                Vector2Int swapindex = PosToSlotObject(data.position);
                
                if (swapindex != dragStartIndex)
                {
                    isDragging = false;
                    beingdragged.position = startposition;
                    swapitems(dragStartIndex,swapindex);
                }
                else if(swapindex==dragStartIndex)
                {
                    isDragging =false;
                    beingdragged.position =startposition;
                }
            }
            
        }
        if (data.button == PointerEventData.InputButton.Right)
        {
            
        }
    }

    void DoDrag()
    {
        if (!isDragging) return;
        Vector3 draggingto= new Vector3(0,0,-10) + Input.mousePosition;
        beingdragged.position = draggingto;
    }

/*    public void OnHover(PointerEventData data)
    {
        if (!isDragging) return;
        beingdragged.position = Input.mousePosition;
    }*/

    public void ThrowItem(Vector2Int itemIndex,Vector3 position)
    {
        InventorySlot item = inventorySlots[itemIndex.x,itemIndex.y];
        inventorySlots[itemIndex.x, itemIndex.y] = new InventorySlot();
        Item.SpawnItem(item.itemDataSlot, position);
        UpdateImages();
    }

    private Vector2Int PosToSlotObject(Vector3 position)
    {
        int slotsnumber = inventorydimensions.x * inventorydimensions.y;
        Vector2 pointrange1 = new Vector2(-slotwidth/2,-slotheight/2);
        Vector2 pointrange2 = new Vector2(slotwidth/2,slotheight/2);
        int count = 0;
        for (int y = 0; y < inventorydimensions.y; y++)
        {
            for (int x = 0; x < inventorydimensions.x; x++)
            {
                if (PointInRangeCheck(position, (Vector2)slotpositions[count] + pointrange1, (Vector2)slotpositions[count] + pointrange2))
                {
                    return new Vector2Int(x, y);
                }
                count++;
            }
        }
        return new Vector2Int(-1, -1);
    }
    bool PointInRangeCheck(Vector2 point, Vector2 range1, Vector2 range2) //ѕровер€ет находитьс€ ли точка( Vector2 ) в пределах диапазона
    {
        //print(point);
        //print(range1);
        //print(range2);
        return (point.x >= range1.x && point.y >= range1.y) &&
               (point.x <= range2.x && point.y <= range2.y);
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
                    inventoryImageSlots[x, y].sprite = inventorySlots[x, y].itemDataSlot.ItemImage;
                }
                else
                {
                    inventoryImageSlots[x, y].sprite = emptybackground;
                    //inventoryImageSlots[x, y].color = new Color(0,0,0, 0.3921569f);
                }
            }
        }
    }

}
