using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CutoutManager : MonoBehaviour
{
    [SerializeField] VoxelCut cuttingboard;
    [SerializeField] Timer timer;
    [SerializeField] public Camera cutcamera;

    public GameObject finalCutout;
    public float finalScore;
    public static CutoutManager instance { get; private set; }

    List<Transform> children;

    public static bool IsCuttingOut { get; private set; }

    Action callback;
    public Color VoxelColor;

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
        SwitchEnableValue(false);
    }


    void SwitchEnableValue(bool value)
    {
        children.ForEach(child => child.gameObject.SetActive(value));
    }

    public void StartCut(Action OnCutEnd)
    {
        print("CUTTING STARTED!!!");
        callback =OnCutEnd;
        
        SwitchEnableValue(true);
        IsCuttingOut = true;
        timer.StartTimer();
        SwitchCamera(true);

    }
    public void StopCut()
    {
        SwitchEnableValue(false);
        IsCuttingOut = false;
        SwitchCamera(false);
        callback();
        callback = null;
    }

    void SwitchCamera(bool state)
    {
        if (state)
            cutcamera.targetDisplay = 0;
        else cutcamera.targetDisplay = -1;
    }



}
