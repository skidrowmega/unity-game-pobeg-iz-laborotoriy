using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CorridorGenerator: MonoBehaviour
{
    //Доделать это чтобы ещё комнаты помимо коридоров ставить
    public Corridor[] corridorPrefabs;
    public Corridor startingRoom;
    private int maxX;
    private int maxY;

    private Corridor[,] spawnedCorridors;

    private void Start()
    {
        startingRoom = Instantiate(corridorPrefabs[0]);
        startingRoom.transform.position = new Vector3(0, 2.24f, 0);//высота должнга быть ноль
        spawnedCorridors = new Corridor[11, 11];
        spawnedCorridors[5, 5] = startingRoom;

        maxX = spawnedCorridors.GetLength(0) - 1;
        maxY = spawnedCorridors.GetLength(1) - 1;

        for (int i = 0; i < 12; i++)
        {
            PlaceOneRoom();
        }
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedCorridors.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedCorridors.GetLength(1); y++)
            {
                if (spawnedCorridors[x, y] == null) continue;

                if (x > 0 && spawnedCorridors[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedCorridors[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedCorridors[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedCorridors[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        Corridor newCorridor = Instantiate(corridorPrefabs[Random.Range(0,corridorPrefabs.Length)]);
        Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));

        ConnectCorridor(newCorridor, position);

        newCorridor.transform.position = new Vector3(position.x - 5, 2.24f / 40, position.y - 5) * 40;//высота должна быть ноль
        spawnedCorridors[position.x, position.y] = newCorridor;
    }

    private void ConnectCorridor(Corridor corridor, Vector2Int pos)
    {
        if (corridor.WallU != null && pos.y < maxY && spawnedCorridors[pos.x, pos.y + 1]?.WallD != null)
        {
            corridor.WallU.SetActive(false);
            spawnedCorridors[pos.x, pos.y + 1].WallD.SetActive(false);
        }
        if (corridor.WallR != null && pos.x < maxX && spawnedCorridors[pos.x + 1, pos.y]?.WallL != null)
        {
            corridor.WallR.SetActive(false);
            spawnedCorridors[pos.x + 1, pos.y].WallL.SetActive(false);
        }
        if (corridor.WallD != null && pos.y > 0 && spawnedCorridors[pos.x, pos.y - 1]?.WallU != null)
        {
            corridor.WallD.SetActive(false);
            spawnedCorridors[pos.x, pos.y - 1].WallU.SetActive(false);
        }
        if (corridor.WallL != null && pos.x > 0 && spawnedCorridors[pos.x - 1, pos.y]?.WallR != null)
        {
            corridor.WallL.SetActive(false);
            spawnedCorridors[pos.x - 1, pos.y].WallR.SetActive(false);
        }
    }
}
