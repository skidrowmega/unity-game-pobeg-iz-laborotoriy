using UnityEngine;

public class FOLLOWCURSOR : MonoBehaviour
{
    public MeshCollider Plane;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Plane.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,100);
        transform.position = new Vector3 (hit.point.x, 2.242f, hit.point.z);
    }
}
