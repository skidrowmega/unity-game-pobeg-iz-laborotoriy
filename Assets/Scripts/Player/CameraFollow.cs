using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Vector2 offset = new Vector2(0,-12);
    [SerializeField] Vector3 RotationalOffset = new Vector3();
    [SerializeField] float Speed = 10;
    void Start()
    {
        player = Player.player;
        print(player);
    }

    // Update is called once per frame
    void Update()
    {
        follow();
    }

    void follow()
    {
        Vector3 newpos = new Vector3(player.transform.position.x + offset.x, transform.position.y, player.transform.position.z + offset.y);
        transform.position = Vector3.MoveTowards(transform.position,newpos,Speed*Time.deltaTime);

    }

}
