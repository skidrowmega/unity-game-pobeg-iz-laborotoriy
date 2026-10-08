using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] RenderTexture minimapTexture;
    [SerializeField] Canvas MinimapCanvas;
    [SerializeField] float BaseSize=50;
    [SerializeField] float FullScreenSize = 100;
    bool IsFullInScreen = false;
    Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!IsFullInScreen&&Input.GetKey(KeyCode.M)) EnterFulscreen();
        if (IsFullInScreen&&Input.GetKeyUp(KeyCode.M)) ExitFulscreen();
    }

    void EnterFulscreen()
    {
        camera.targetTexture = null;
        camera.orthographicSize = FullScreenSize;
        camera.targetDisplay = 0;
        IsFullInScreen = true;
        MinimapCanvas.enabled = false;
    }
    void ExitFulscreen()
    {
        camera.targetTexture = minimapTexture;
        camera.targetDisplay = -1;
        camera.orthographicSize = BaseSize;
        IsFullInScreen =false;
        MinimapCanvas.enabled=true;
    }

}
