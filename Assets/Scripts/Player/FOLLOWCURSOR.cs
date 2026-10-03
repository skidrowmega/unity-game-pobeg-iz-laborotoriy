using UnityEngine;

public class FOLLOWCURSOR : MonoBehaviour
{
    //public MeshCollider Plane;
    public LineRenderer linerenderer;
    public Entity player;
    //private float Pivot;
    // Update is called once per frame
    private void Start()
    {
        player = PlayerHandler.player;
        //Pivot=Plane.transform.position.y+.02f;
    }
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100);
        if (hit.collider)
            transform.position = new Vector3(hit.point.x, player.transform.position.y-player.transform.localScale.y/2, hit.point.z);
        if (Input.GetMouseButton(1))
        {
            if (!linerenderer.enabled)
                linerenderer.enabled = true;
            linerenderer.SetPosition(0, transform.position);
            Vector3 newposition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            linerenderer.SetPosition(1, newposition);
        }
        else
            linerenderer.enabled = false;
    }
}
