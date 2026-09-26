using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class CorridorGenerator: MonoBehaviour
{
    //Доделать это чтобы ещё комнаты помимо коридоров ставить
    public Room[] roomPrefabs;
    //public Room startingCorridor;
    //public Room startingRoom;
    private int maxX;
    private int maxY;

    private Room[,] spawnedRooms;
    //private Room[,] spawnedRooms;

    private void Start()
    {
        spawnedRooms = new Room[11, 11];
        Room startingCorridor = Instantiate(roomPrefabs[1]);
        startingCorridor.transform.position = new Vector3(0, 2.24f, 0);//высота должнга быть ноль
        spawnedRooms[5, 5] = startingCorridor;

        maxX = spawnedRooms.GetLength(0) - 1;
        maxY = spawnedRooms.GetLength(1) - 1;

        for (int i = 0; i < 9; i++)
        {
            PlaceRooms();
        }

        /*foreach (Room cor in spawnedRooms)//это должно быть не здесь... вернуть старый цикл (но он будет делать ветки) а это перенести в метод PlaceOneRoom (поменять название)
        {
            PlaceOneRoom(cor);
        }*/
    }

    private void PlaceRooms()//(поменять название) выбрать комнату из существующих, рядом с ней заспавнить и продолжить ветку вероятности зависят от того какие комнаты рядом, если ветка расходится (я пока не придумал что делать, пусть она не расходится)
    {
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                List<float> vacantPlaces = new List<float>();
                if (spawnedRooms[x, y] == null) continue;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(4);
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(3);
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(2);
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(1);

                Vector2Int position = Vector2Int.zero;
                float direction = ChooseDirectionCorridor(new Vector2Int(x,y), vacantPlaces);

                if (direction == 0) continue;
                else if (direction == 1)
                {
                    Room newCorridor = Instantiate(roomPrefabs[1]);
                    position = new Vector2Int(x, y + 1);
                    ConnectCorridor(newCorridor, position);
                    newCorridor.transform.position = new Vector3(position.x - 5, 2.24f / 40, position.y - 5) * 40;//высота должна быть ноль
                    spawnedRooms[position.x, position.y] = newCorridor;
                }
                else if (direction == 2)
                {
                    Room newCorridor = Instantiate(roomPrefabs[1]);
                    position = new Vector2Int(x + 1, y);
                    ConnectCorridor(newCorridor, position);
                    newCorridor.transform.position = new Vector3(position.x - 5, 2.24f / 40, position.y - 5) * 40;//высота должна быть ноль
                    spawnedRooms[position.x, position.y] = newCorridor;
                }
                else if (direction == 3)
                {
                    Room newCorridor = Instantiate(roomPrefabs[1]);
                    position = new Vector2Int(x, y - 1);
                    ConnectCorridor(newCorridor, position);
                    newCorridor.transform.position = new Vector3(position.x - 5, 2.24f / 40, position.y - 5) * 40;//высота должна быть ноль
                    spawnedRooms[position.x, position.y] = newCorridor;
                }
                else if (direction == 4)
                {
                    Room newCorridor = Instantiate(roomPrefabs[1]);
                    position = new Vector2Int(x - 1, y);
                    ConnectCorridor(newCorridor, position);
                    newCorridor.transform.position = new Vector3(position.x - 5, 2.24f / 40, position.y - 5) * 40;//высота должна быть ноль
                    spawnedRooms[position.x, position.y] = newCorridor;
                }
            }
        }
    }

    private void ConnectCorridor(Room corridor, Vector2Int pos)//пока не трогать
    {
        if (corridor.WallU != null && pos.y < maxY && spawnedRooms[pos.x, pos.y + 1]?.WallD != null)
        {
            corridor.WallU.SetActive(false);
            spawnedRooms[pos.x, pos.y + 1].WallD.SetActive(false);
        }
        if (corridor.WallR != null && pos.x < maxX && spawnedRooms[pos.x + 1, pos.y]?.WallL != null)
        {
            corridor.WallR.SetActive(false);
            spawnedRooms[pos.x + 1, pos.y].WallL.SetActive(false);
        }
        if (corridor.WallD != null && pos.y > 0 && spawnedRooms[pos.x, pos.y - 1]?.WallU != null)
        {
            corridor.WallD.SetActive(false);
            spawnedRooms[pos.x, pos.y - 1].WallU.SetActive(false);
        }
        if (corridor.WallL != null && pos.x > 0 && spawnedRooms[pos.x - 1, pos.y]?.WallR != null)
        {
            corridor.WallL.SetActive(false);
            spawnedRooms[pos.x - 1, pos.y].WallR.SetActive(false);
        }
    }

    private float ChooseDirectionCorridor(Vector2Int xy, List<float> vacPlaces)//тут или не тут выбирать куда спавнить комнату
    {
        float[,] probs = new float[5, 2] { {1, 0f}, {2, 0f} , {3, 0f} , {4, 0f} , {0, 2f} };
        Vector2Int currentRoom = xy;
        if (vacPlaces.Count == 4)
        {
            probs[0, 1] = 0.25f;
            probs[1, 1] = 0.25f;
            probs[2, 1] = 0.25f;
            probs[3, 1] = 0.25f;
        }
        else if (vacPlaces.Count == 3)
        {
            if (!vacPlaces.Contains(1))
            {
                probs[1, 1] = 0.05f;
                probs[2, 1] = 0.7f;
                probs[3, 1] = 0.05f;
                probs[4, 1] = 0.2f;
            }
            else if (!vacPlaces.Contains(2))
            {
                probs[0, 1] = 0.05f;
                probs[2, 1] = 0.05f;
                probs[3, 1] = 0.7f;
                probs[4, 1] = 0.1f;
            }
            else if (!vacPlaces.Contains(3))
            {
                probs[0, 1] = 0.7f;
                probs[1, 1] = 0.05f;
                probs[3, 1] = 0.05f;
                probs[4, 1] = 0.1f;
            }
            else if (!vacPlaces.Contains(4))
            {
                probs[0, 1] = 0.05f;
                probs[1, 1] = 0.7f;
                probs[2, 1] = 0.05f;
                probs[4, 1] = 0.1f;
            }
        }
        else if (vacPlaces.Count == 2)
        {
            if (!vacPlaces.Contains(1) && !vacPlaces.Contains(2))
            {
                probs[2, 1] = 0.1f;
                probs[3, 1] = 0.1f;
                probs[4, 1] = 0.8f;
            }
            else if (!vacPlaces.Contains(1) && !vacPlaces.Contains(3))
            {
                probs[1, 1] = 0.1f;
                probs[3, 1] = 0.1f;
                probs[4, 1] = 0.8f;
            }
            else if (!vacPlaces.Contains(1) && !vacPlaces.Contains(4))
            {
                probs[1, 1] = 0.1f;
                probs[2, 1] = 0.1f;
                probs[4, 1] = 0.8f;
            }
            else if (!vacPlaces.Contains(3) && !vacPlaces.Contains(2))
            {
                probs[0, 1] = 0.1f;
                probs[3, 1] = 0.1f;
                probs[4, 1] = 0.8f;
            }
            else if (!vacPlaces.Contains(4) && !vacPlaces.Contains(2))
            {
                probs[0, 1] = 0.1f;
                probs[2, 1] = 0.1f;
                probs[4, 1] = 0.8f;
            }
            else if (!vacPlaces.Contains(4) && !vacPlaces.Contains(3))
            {
                probs[0, 1] = 0.1f;
                probs[1, 1] = 0.1f;
                probs[4, 1] = 0.8f;
            }
        }
        else if (vacPlaces.Count == 1)
        {
            if (vacPlaces.Contains(1))
            {
                probs[0, 1] = 0.05f;
                probs[4, 1] = 0.95f;
            }
            else if (vacPlaces.Contains(2))
            {
                probs[1, 1] = 0.05f;
                probs[4, 1] = 0.95f;
            }
            else if (vacPlaces.Contains(3))
            {
                probs[2, 1] = 0.05f;
                probs[4, 1] = 0.95f;
            }
            else if (vacPlaces.Contains(4))
            {
                probs[3, 1] = 0.05f;
                probs[4, 1] = 0.95f;
            }
        }
        else
        {
            probs[4, 1] = 1f;
        }

        float randomPoint = UnityEngine.Random.value;
        print(randomPoint);

        for (int i = 0; i < probs.GetLength(0); i++)
        {
            if (randomPoint < probs[i, 1])
            {
                return probs[i,0];
            }
            else
            {
                randomPoint -= probs[i, 1];
            }
        }

        return 0f;
    }
}

