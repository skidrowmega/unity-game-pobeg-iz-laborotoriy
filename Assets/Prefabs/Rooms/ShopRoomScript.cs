using UnityEngine;

public class ShopRoomScript : MonoBehaviour
{


    [SerializeField] Vector2Int RoomSize;
    bool HasExited = true;

    [SerializeField] ItemScriptableObject[] itempool;
    Vector2 range1;
    Vector2 range2;

    private void Start()
    {
        if (itempool.Length > 0)
        {
            for (int i = 0; i < Random.Range(5, 10); i++)
            {
                ShopScript.instance.AddItem(itempool[Random.Range(0,itempool.Length)]);
            }
        }
        range1 = new Vector2(transform.position.x - RoomSize.x / 2, transform.position.z - RoomSize.y / 2);
        range2 = new Vector2(transform.position.x + RoomSize.x / 2, transform.position.z + RoomSize.y / 2);
    }

    private void Update()
    {


        



        if (PointInRangeCheck(new Vector2(Player.player.transform.position.x, Player.player.transform.position.z),range1,range2))
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.Shop && HasExited)
            {
                ShopScript.instance.OpenShop();
                HasExited = false;
            }
        }
        else if (!HasExited) HasExited = true;
    }
    bool PointInRangeCheck(Vector2 point, Vector2 range1, Vector2 range2) //ѕровер€ет находитьс€ ли точка( Vector2 ) в пределах диапазона
    {
        return (point.x >= range1.x && point.y >= range1.y) &&
               (point.x <= range2.x && point.y <= range2.y);
    }

}
