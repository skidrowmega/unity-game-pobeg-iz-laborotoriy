using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AmuletSlot
{
    [SerializeField] public AmuletSO amuletdata;
    [SerializeField] public bool isEmpty = true;
}


public class TreasureChestScript : MonoBehaviour
{

    [SerializeField] RectTransform ArtifactList;
    [SerializeField] Text ItemDescription;
    List<Image> SlotImages;
    List<GameObject> SlotObjects;
    AmuletSlot[] artifactSlots;
    [SerializeField] Sprite emptybackground;

#if UNITY_EDITOR
    [SerializeField] bool debug = false;
#endif

    public static TreasureChestScript instance { get; private set; }

    GameObject MenuHolder;



    private void Awake()
    {
        MenuHolder = transform.GetChild(0).gameObject;
        instance = this;
        for (int i = 0; i < ArtifactList.childCount; i++)
        {
            Transform child = ArtifactList.GetChild(i);
            SlotImages.Add(child.GetComponentInChildren<Image>());
            artifactSlots[i] = new AmuletSlot
            {
                amuletdata = null,
                isEmpty = true
            };



            GameObject slotObject = ArtifactList.GetChild(i).gameObject;
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

    void OnPointerDown(PointerEventData eventData)
    {
        AmuletSlot slot = ObjectToSlot(eventData.pointerCurrentRaycast.gameObject);
        AmuletManager.instance.EquipAmulet(slot.amuletdata);
        CloseWindow();
    }
    void OnPointerEnter(PointerEventData eventData)
    {
        AmuletSlot item = ObjectToSlot(eventData.pointerCurrentRaycast.gameObject);
        if (item!=null) ItemDescription.text = item.amuletdata.amuletDescription;
    }


    void OnPointerExit(PointerEventData eventData)
    {
        ItemDescription.text = "";
    }

    AmuletSlot ObjectToSlot(GameObject gobject)
    {
        int index = SlotObjects.IndexOf(gobject.transform.parent.gameObject);
        return artifactSlots[index];
    }


    void OpenWindow()
    {
        MenuHolder.SetActive(true);
    }
    
    void CloseWindow()
    {
        MenuHolder.SetActive(false);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
