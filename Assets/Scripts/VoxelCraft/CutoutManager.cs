using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CutoutManager : MonoBehaviour
{
    [SerializeField] public VoxelCut cuttingboard;
    [SerializeField] GameObject CuttingBoardPrefab;

    [SerializeField] Timer timer;
    [SerializeField] public Camera cutcamera;

    public GameObject finalCutout;
    public float finalScore;
    public static CutoutManager instance { get; private set; }

    List<Transform> children;

    public static bool IsCuttingOut { get; private set; }

    Action callback;
    public Color VoxelColor;
    public Pattern Pattern;


    public bool Debug;

    private void Awake()
    {
        instance = this;
        IsCuttingOut = false;
        children = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }

    }

    private void Start()
    {
        if(!Debug)SwitchEnableValue(false);
        if (Debug)
        {
            cuttingboard.Timer = 100000;
            EditorUtility.SetDirty(Pattern);
            StartCut();
        }
    }


    void SwitchEnableValue(bool value)
    {
        children.ForEach(child => child.gameObject.SetActive(value));
    }

    public void StartCut(Action OnCutEnd=null)
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Craft);
        print("CUTTING STARTED!!!");
        if (OnCutEnd!=null)
            callback =OnCutEnd;
        SwitchEnableValue(true);
        IsCuttingOut = true;
        cuttingboard.CutStart();
        timer.StartTimer();
        SwitchCamera(true);

    }
    public void StopCut()
    {
        IsCuttingOut = false;
        SwitchCamera(false);
        callback();
        callback = null;
        cuttingboard.PrepareForDestroy();
        Destroy(cuttingboard.gameObject);
        VoxelCut newcuttingboard = Instantiate(CuttingBoardPrefab, cuttingboard.transform.parent).GetComponent<VoxelCut>();
        newcuttingboard.SubmitButtonText=cuttingboard.SubmitButtonText;
        newcuttingboard.SubmitResultText=cuttingboard.SubmitResultText;
        newcuttingboard.Awake();
        newcuttingboard.Start();
        cuttingboard = newcuttingboard;
        timer.cuttingboard = cuttingboard;
        SwitchEnableValue(false);
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
    }

    public void SubmitButtonPress()
    {
        cuttingboard.SubmitButtonPressed();
    }

    public void SliderUpdate()
    {
        cuttingboard.SetBrushSize();
    }

    void SwitchCamera(bool state)
    {
        if (state)
            cutcamera.targetDisplay = 0;
        else cutcamera.targetDisplay = -1;
    }



}
