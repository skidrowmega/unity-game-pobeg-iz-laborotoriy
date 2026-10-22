using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] RenderTexture minimapTexture;
    [SerializeField] Canvas MinimapCanvas;
    [SerializeField] float BaseSize=50;
    [SerializeField] float FullScreenSize = 100;
    bool IsFullInScreen = false;
    Camera minimapcamera;

    private void Awake()
    {
        minimapcamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!IsFullInScreen&&Input.GetKey(KeyCode.M)) EnterFulscreen();
        if (IsFullInScreen&&Input.GetKeyUp(KeyCode.M)) ExitFulscreen();
    }

    void EnterFulscreen()
    {
        minimapcamera.targetTexture = null;
        minimapcamera.orthographicSize = FullScreenSize;
        minimapcamera.targetDisplay = 0;
        IsFullInScreen = true;
        MinimapCanvas.enabled = false;
    }
    void ExitFulscreen()
    {
        minimapcamera.targetTexture = minimapTexture;
        minimapcamera.targetDisplay = -1;
        minimapcamera.orthographicSize = BaseSize;
        IsFullInScreen =false;
        MinimapCanvas.enabled=true;
    }

}
