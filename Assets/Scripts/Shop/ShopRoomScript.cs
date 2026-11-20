using UnityEngine;

public class ShopRoomScript : MonoBehaviour
{
    [SerializeField] Vector2Int RoomSize;
    bool HasExited = true;

    [SerializeField] public ItemScriptableObject[] itempool;
    Vector2 range1;
    Vector2 range2;
    public static ShopRoomScript instance { get; private set; }

    private void Start()
    {
        instance = this;
        if (itempool.Length > 0  && !GameManager.Instance.isShopGeneratedOnce)
        {
            for (int i = 0; i < itempool.Length; i++)
            {
                ShopScript.instance.AddItem(itempool[i]);
            }
            GameManager.Instance.isShopGeneratedOnce = true;
        }
        range1 = new Vector2(transform.position.x - RoomSize.x / 2, transform.position.z - RoomSize.y / 2);
        range2 = new Vector2(transform.position.x + RoomSize.x / 2, transform.position.z + RoomSize.y / 2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PointInRangeCheck(new Vector2(Player.player.transform.position.x, Player.player.transform.position.z),range1,range2))
        {
            if (GameManager.Instance.isGameState(GameManager.GameState.Playing) && HasExited)
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
