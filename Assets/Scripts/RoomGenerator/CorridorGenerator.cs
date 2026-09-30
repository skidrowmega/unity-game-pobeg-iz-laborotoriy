using System;
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
    [SerializeField] Room[] roomPrefabs;
    private int maxX;
    private int maxY;

    private Room[,] spawnedRooms;
    private Room[,] spawnedNotOnlyCorridors;
    private HashSet<Vector2Int> CheckedRooms = new HashSet<Vector2Int>();

    private void Start()
    {
        spawnedRooms = new Room[11, 11];
        spawnedNotOnlyCorridors = new Room[11, 11];
        Room startingCorridor = Instantiate(roomPrefabs[0]);
        startingCorridor.transform.position = new Vector3(0, 0, 0);//высота должнга быть ноль
        spawnedRooms[5, 5] = startingCorridor;

        maxX = spawnedRooms.GetLength(0) - 1;
        maxY = spawnedRooms.GetLength(1) - 1;

        for (int i = 0; i < 10; i++)
        {
            PlaceRooms();
        }
        CheckedRooms = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;
                List<float> vacantPlaces = new List<float>();

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(4);
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(3);
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(2);
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(1);

                if (vacantPlaces.Count < 3) ;
            }
        }
        if (CheckedRooms.Count < 3)
        {
            for (int i = 0; i < 3; i++)
            {
                var validPositions = Enumerable.Range(0, spawnedRooms.GetLength(0)).SelectMany(x => Enumerable.Range(0, spawnedRooms.GetLength(1)).Select(y => new Vector2Int(x, y))).Where(pos => spawnedRooms[pos.x, pos.y] != null).ToList();
                Vector2Int randomPos = validPositions[UnityEngine.Random.Range(0, validPositions.Count)];
                if (CheckedRooms.Contains(randomPos)) CheckedRooms.Remove(randomPos);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            PlaceRooms();
        }
        for (int i = 1; i < 4; i++)
        {
            PlaceNotOnlyCorridors(roomPrefabs[i]);
        }
    }

    private void PlaceNotOnlyCorridors(Room prefab)
    {
        HashSet<Vector2Int> vacantPlacesRooms = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;
                if (spawnedNotOnlyCorridors[x, y] != null) continue;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlacesRooms.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlacesRooms.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlacesRooms.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlacesRooms.Add(new Vector2Int(x, y + 1));
            }
        }

        Room newRoom = Instantiate(prefab);
        Vector2Int position = vacantPlacesRooms.ElementAt(UnityEngine.Random.Range(0, vacantPlacesRooms.Count));
        newRoom.transform.position = new Vector3(position.x - 5, 0f / 40, position.y - 5) * 40;//высота должна быть ноль
        ConnectRoom(newRoom, position);
        spawnedRooms[position.x, position.y] = newRoom;
    }

    private void ConnectRoom(Room room, Vector2Int pos)
    {
        if (room.WallR != null && pos.x < maxX && spawnedRooms[pos.x + 1, pos.y]?.WallL != null)
        {
            room.WallR.SetActive(false);
            spawnedRooms[pos.x + 1, pos.y].WallL.SetActive(false);
        }
        else if (room.WallR != null && pos.y > 0 && spawnedRooms[pos.x, pos.y - 1]?.WallU != null)
        {
            room.transform.Rotate(0, 90, 0);
            room.WallR.SetActive(false);
            spawnedRooms[pos.x, pos.y - 1].WallU.SetActive(false);
        }
        else if (room.WallR != null && pos.x > 0 && spawnedRooms[pos.x - 1, pos.y]?.WallR != null)
        {
            room.transform.Rotate(0, 180, 0);
            room.WallR.SetActive(false);
            spawnedRooms[pos.x - 1, pos.y].WallR.SetActive(false);
        }
        else if (room.WallR != null && pos.y < maxY && spawnedRooms[pos.x, pos.y + 1]?.WallD != null)
        {
            room.transform.Rotate(0, 270, 0);
            room.WallR.SetActive(false);
            spawnedRooms[pos.x, pos.y + 1].WallD.SetActive(false);
        }
    }
    private void PlaceRooms()//(поменять название) выбрать комнату из существующих, рядом с ней заспавнить и продолжить ветку вероятности зависят от того какие комнаты рядом, если ветка расходится (я пока не придумал что делать, пусть она не расходится)
    {
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;
                if (CheckedRooms.Contains(new Vector2Int(x, y))) continue;
                List<float> vacantPlaces = new List<float>();

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(4);// Заменить проверку spawnedRooms на метод
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(3);
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(2);
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(1);

                Vector2Int position = Vector2Int.zero;
                Room newCorridor;
                float direction = ChooseDirectionCorridor(vacantPlaces);

                if (direction == 0) continue;//Изменить хуйню на Switch
                else if (direction == 1)
                {
                    newCorridor = Instantiate(roomPrefabs[0]);
                    position = new Vector2Int(x, y + 1);
                }
                else if (direction == 2)
                {
                    newCorridor = Instantiate(roomPrefabs[0]);
                    position = new Vector2Int(x + 1, y);
                    
                }
                else if (direction == 3)
                {
                    newCorridor = Instantiate(roomPrefabs[0]);
                    position = new Vector2Int(x, y - 1);
                }
                else
                {
                    newCorridor = Instantiate(roomPrefabs[0]);
                    position = new Vector2Int(x - 1, y);
                }
                spawnedRooms[position.x, position.y] = newCorridor;
                newCorridor.transform.position = new Vector3(position.x - 5, 0f / 40, position.y - 5) * 40;//волшебство цифор
                ConnectCorridor(newCorridor, position);
                CheckedRooms.Add(new Vector2Int(x, y));
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

/*    private float ChooseDirectionCorridor(List<float> vacPlaces)//тут или не тут выбирать куда спавнить комнату
    {
        float[,] probs = new float[5, 2] { { 1, 0f }, { 2, 0f }, { 3, 0f }, { 4, 0f }, { 0, 2f } };
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
                probs[1, 1] = 0.1f;
                probs[2, 1] = 0.7f;
                probs[3, 1] = 0.1f;
                probs[4, 1] = 0.1f;
            }
            else if (!vacPlaces.Contains(2))
            {
                probs[0, 1] = 0.1f;
                probs[2, 1] = 0.1f;
                probs[3, 1] = 0.7f;
                probs[4, 1] = 0.1f;
            }
            else if (!vacPlaces.Contains(3))
            {
                probs[0, 1] = 0.7f;
                probs[1, 1] = 0.1f;
                probs[3, 1] = 0.1f;
                probs[4, 1] = 0.1f;
            }
            else if (!vacPlaces.Contains(4))
            {
                probs[0, 1] = 0.1f;
                probs[1, 1] = 0.7f;
                probs[2, 1] = 0.1f;
                probs[4, 1] = 0.1f;
            }
        }
        else if (vacPlaces.Count == 2)
        {
            if (!vacPlaces.Contains(1) && !vacPlaces.Contains(2))
            {
                probs[2, 1] = 0.2f;
                probs[3, 1] = 0.2f;
                probs[4, 1] = 0.6f;
            }
            else if (!vacPlaces.Contains(1) && !vacPlaces.Contains(3))
            {
                probs[1, 1] = 0.2f;
                probs[3, 1] = 0.2f;
                probs[4, 1] = 0.6f;
            }
            else if (!vacPlaces.Contains(1) && !vacPlaces.Contains(4))
            {
                probs[1, 1] = 0.2f;
                probs[2, 1] = 0.2f;
                probs[4, 1] = 0.6f;
            }
            else if (!vacPlaces.Contains(3) && !vacPlaces.Contains(2))
            {
                probs[0, 1] = 0.2f;
                probs[3, 1] = 0.2f;
                probs[4, 1] = 0.6f;
            }
            else if (!vacPlaces.Contains(4) && !vacPlaces.Contains(2))
            {
                probs[0, 1] = 0.2f;
                probs[2, 1] = 0.2f;
                probs[4, 1] = 0.6f;
            }
            else if (!vacPlaces.Contains(4) && !vacPlaces.Contains(3))
            {
                probs[0, 1] = 0.2f;
                probs[1, 1] = 0.2f;
                probs[4, 1] = 0.6f;
            }
        }
        else if (vacPlaces.Count == 1)
        {
            if (vacPlaces.Contains(1))
            {
                probs[0, 1] = 0.2f;
                probs[4, 1] = 0.8f;
            }
            else if (vacPlaces.Contains(2))
            {
                probs[1, 1] = 0.2f;
                probs[4, 1] = 0.8f;
            }
            else if (vacPlaces.Contains(3))
            {
                probs[2, 1] = 0.2f;
                probs[4, 1] = 0.8f;
            }
            else if (vacPlaces.Contains(4))
            {
                probs[3, 1] = 0.2f;
                probs[4, 1] = 0.8f;
            }
        }
        else
        {
            probs[4, 1] = 1f;
        }

        float randomPoint = UnityEngine.Random.value;

        for (int i = 0; i < probs.GetLength(0); i++)
        {
            if (randomPoint < probs[i, 1])
            {
                return probs[i, 0];
            }
            else
            {
                randomPoint -= probs[i, 1];
            }
        }

        return 0f;
    */}
}

