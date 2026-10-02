using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject WallU;
    public GameObject WallR;
    public GameObject WallD;
    public GameObject WallL;

    public Vector2Int position;

    public virtual void RemoveWall(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            WallU.SetActive(false);
        }
        if (direction == Vector2Int.right)
        {
            WallR.SetActive(false);
        }
        if (direction == Vector2Int.left)
        {
            WallL.SetActive(false);
        }
        if (direction == Vector2Int.down)
        {
            WallD.SetActive(false);
        }
    }

}
