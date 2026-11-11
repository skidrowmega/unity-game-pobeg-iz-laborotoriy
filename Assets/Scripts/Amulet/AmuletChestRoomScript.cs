using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AmuletChestRoomScript:MonoBehaviour
{
    [SerializeField] Vector2Int RoomSize;
    bool HasExited = true;

    [SerializeField] List<AmuletSO> amuletpool;
    Vector2 range1;
    Vector2 range2;

    private void Start()
    {
        if (amuletpool.Count > 0)
        {
            AmuletSO amuletSO;
            for (int i = 0; i < AmuletManager.instance.maxSlots; i++)
            {
                amuletSO = amuletpool[Random.Range(0, amuletpool.Count)];
                TreasureChestScript.instance.AddAmulet(amuletSO);
                amuletpool.Remove(amuletSO);
            }
        }
        range1 = new Vector2(transform.position.x - RoomSize.x / 2, transform.position.z - RoomSize.y / 2);
        range2 = new Vector2(transform.position.x + RoomSize.x / 2, transform.position.z + RoomSize.y / 2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PointInRangeCheck(new Vector2(Player.player.transform.position.x, Player.player.transform.position.z), range1, range2))
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.AmuletChest && HasExited)
            {
                TreasureChestScript.instance.OpenChest();
                HasExited = false;
            }
        }
        else if (!HasExited) HasExited = true;
    }
    bool PointInRangeCheck(Vector2 point, Vector2 range1, Vector2 range2) //Проверяет находиться ли точка( Vector2 ) в пределах диапазона
    {
        return (point.x >= range1.x && point.y >= range1.y) &&
               (point.x <= range2.x && point.y <= range2.y);
    }

}
