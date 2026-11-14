using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CorridorGenerator : MonoBehaviour
{
    //Доделать это чтобы ещё комнаты помимо коридоров ставить


    [SerializeField] int minRooms = 15;
    [SerializeField] int maxRooms = 20;
    [SerializeField] Room[] roomPrefabs;
    [SerializeField] Room LiftEnter;
    [SerializeField] Room LiftExit;
    int maxX;
    int maxY;


    int roomWidth = 20;
    int roomHeight = 20;

    List<Room> rooms = new List<Room>();
    int roomcount;

    Queue<Vector2Int> roomQueue;

    int gridsizeX = 100;
    int gridsizeY = 100;
    bool[,] Grid;

    bool isFinished = false;

    private void Start()
    {
        StartRoomGeneration();

    }

    void StartRoomGeneration()
    {
        Grid = new bool[gridsizeX, gridsizeY];
        roomQueue = new Queue<Vector2Int>();

        Vector2Int initialroom = new Vector2Int(gridsizeX / 2, gridsizeY / 2);
        StartRoomGenerationFromRoom(initialroom);
        while (!isFinished)
        {
            if (roomQueue.Count > 0 && roomcount < maxRooms && !isFinished)
            {
                Vector2Int roomIndex = roomQueue.Dequeue();
                int gridX = roomIndex.x;
                int gridY = roomIndex.y;

                TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
                TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
                TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
                TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
            }
            else if (roomcount < minRooms)
            {
                Debug.Log("regenerating rooms");
                RegenerateRooms();
            }
            else if (!isFinished)
            {
                print($"Finished Generation, generated {roomcount} rooms");
                isFinished = true;
                GenerateRoomPrefabs();
                GenerateLiftRooms();
                if (roomcount >= minRooms && roomPrefabs.Length > 1)
                    for (int i = 1; i < roomPrefabs.Length; i++)
                    {
                        GenerateSpecialRoom(roomPrefabs[i]);

                    }
            }
        }
    }

    void RegenerateRooms()
    {
        foreach (Room room in rooms)
        {
            Destroy(room.gameObject);
        }
        rooms.Clear();
        Grid = new bool[gridsizeX, gridsizeY];
        roomQueue.Clear();
        roomcount = 0;
        isFinished = false;

        Vector2Int initialroom = new Vector2Int(gridsizeX / 2, gridsizeY / 2);
        StartRoomGenerationFromRoom(initialroom);
    }


    void GenerateRoomPrefabs()
    {
        Vector2Int index;
        Room newroom;
        int count = 0;
        for (int x = 0; x < gridsizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {
                if (Grid[x, y])
                {
                    index = new Vector2Int(x, y);
                    newroom = Instantiate(roomPrefabs[0], GetPosFromIndex(index), Quaternion.identity);
                    newroom.name = $"Room-{count + 1}";
                    newroom.position = index;
                    rooms.Add(newroom);
                    count++;
                }
            }
        }
        for (int x = 0; x < gridsizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {
                if ((x + y) % 2 == 0)
                {
                    index = new Vector2Int(x, y);
                    if (Grid[x, y]) RemoveWalls(GetRoomAt(index));
                }
            }
        }
    }

    private void StartRoomGenerationFromRoom(Vector2Int index)
    {
        roomQueue.Enqueue(index);
        int x = index.x;
        int y = index.y;
        Grid[x, y] = true;
        roomcount++;
        //var initialRoom = Instantiate(roomPrefabs[0], GetPosFromIndex(index), Quaternion.identity);
        //initialRoom.name = $"Room-{roomcount}";
        //initialRoom.GetComponent<Room>().position = index;
        //rooms.Add(initialRoom);
    }

    private bool TryGenerateRoom(Vector2Int index)
    {
        int x = index.x;
        int y = index.y;

        if (roomcount >= maxRooms)
            return false;

        if (x > gridsizeX - 1 || x < 0 || y < 0 || y > gridsizeY - 1) return false;

        if (UnityEngine.Random.value < .5f && index != Vector2Int.zero)
            return false;

        if (Grid[x, y]) return false;
        int adjacentrooms = CountAdjacentRooms(index);
        if (adjacentrooms > 1)
        {
            float random = UnityEngine.Random.value*100;
/*            if (adjacentrooms > 2)
            {
                if (random > .5f) return false;
            }
            if (adjacentrooms > 3)
            {
                if (random < .8f) return false;
            }
            else return false;*/
            if (adjacentrooms == 2 && random > 50) return false;
            if (adjacentrooms == 3 && random > 30) return false;
            if (adjacentrooms == 4 && random>10) return false;
        }

        roomQueue.Enqueue(index);
        Grid[x, y] = true;
        roomcount++;

        return true;
    }

    private void GenerateSpecialRoom(Room specialroomprefab)
    {
        List<Vector2Int> availablespots = new List<Vector2Int>();
        Vector2Int newposition;
        for (int x = 0; x < gridsizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {
                newposition = new Vector2Int(x, y);
                if (!Grid[x, y] && CountAdjacentRooms(newposition) == 1)
                {
                    availablespots.Add(newposition);
                }
            }
        }

        Vector2Int index = availablespots[UnityEngine.Random.Range(0, availablespots.Count - 1)];

        int Xpos = index.x;
        int Ypos = index.y;

        Grid[Xpos, Ypos] = true;

        Room newroom = Instantiate(specialroomprefab, GetPosFromIndex(index), Quaternion.identity);
        newroom.name = $"{specialroomprefab.name}";
        newroom.GetComponent<Room>().position = index;
        rooms.Add(newroom);
        RemoveWalls(newroom);
    }

    private void GenerateLiftRooms()
    {
        List<Vector2Int> availablespots = new List<Vector2Int>();
        Vector2Int newposition;
        for (int x = 0; x < gridsizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {
                newposition = new Vector2Int(x, y);
                if (!Grid[x, y] && CountAdjacentRooms(newposition) == 1)
                {
                    availablespots.Add(newposition);
                }
            }
        }

        Vector2Int index = availablespots[UnityEngine.Random.Range(0, availablespots.Count - 1)];
        Vector2Int furthestspot = availablespots.OrderByDescending(x => Vector2Int.Distance(index, x)).First();
        int Xpos = index.x;
        int Ypos = index.y;

        Grid[Xpos, Ypos] = true;

        Room newroom = Instantiate(LiftEnter.gameObject, GetPosFromIndex(index), Quaternion.identity).GetComponent<Room>();
        newroom.name = $"{LiftEnter.name}";
        newroom.GetComponent<Room>().position = index;
        rooms.Add(newroom);
        RemoveWalls(newroom);
        index = furthestspot;
        Xpos = index.x;
        Ypos = index.y;

        Grid[Xpos, Ypos] = true;

        newroom = Instantiate(LiftExit.gameObject, GetPosFromIndex(index), Quaternion.identity).GetComponent<Room>();
        newroom.name = $"{LiftExit.name}";
        newroom.GetComponent<Room>().position = index;
        rooms.Add(newroom);
        RemoveWalls(newroom);
    }

    private int CountAdjacentRooms(Vector2Int index)
    {
        int x = index.x;
        int y = index.y;
        int count = 0;
        //if (x > 0 && Grid[x - 1, y]) count++;
        //if (x < gridsizeX-1 && Grid[x + 1, y]) count++;
        //if (y > 0 && Grid[x, y - 1]) count++;
        //if (y < gridsizeY-1 && Grid[x, y + 1]) count++;

        //for (int i = -1; i <= 1; i++)
        //{
        //    for (int j = -1; j <= 1; j++)
        //    {
        //        if (i == j) continue;
        //        if (CheckRoomAt(new Vector2Int(x+i, y+j))) count++;
        //    }
        //}

        bool leftroom = CheckRoomAt(new Vector2Int(x - 1, y));
        bool rightroom = CheckRoomAt(new Vector2Int(x + 1, y));
        bool uproom = CheckRoomAt(new Vector2Int(x, y + 1));
        bool downroom = CheckRoomAt(new Vector2Int(x, y - 1));
        if (leftroom) count++;
        if (rightroom) count++;
        if (uproom) count++;
        if (downroom) count++;


        return count;
    }

    private bool IsSpecial(Vector2Int index)
    {
        Room room = GetRoomAt(index);
        return room && room.GetComponent<SpecialRoom>() != null;
    }

    private bool IsSpecial(Room room)
    {
        return room && room.GetComponent<SpecialRoom>() != null;
    }

    void RemoveWalls(Room room)
    {
        int x = room.position.x;
        int y = room.position.y;
        Room leftroom = GetRoomAt(new Vector2Int(x - 1, y));
        Room rightroom = GetRoomAt(new Vector2Int(x + 1, y));
        Room uproom = GetRoomAt(new Vector2Int(x, y + 1));
        Room downroom = GetRoomAt(new Vector2Int(x, y - 1));

        if (leftroom && !IsSpecial(leftroom))
        {

            room.RemoveWall(Vector2Int.left);
            leftroom.RemoveWall(Vector2Int.right);
        }
        if (rightroom && !IsSpecial(rightroom))
        {
            room.RemoveWall(Vector2Int.right);
            rightroom.RemoveWall(Vector2Int.left);
        }
        if (uproom && !IsSpecial(uproom))
        {
            room.RemoveWall(Vector2Int.up);
            uproom.RemoveWall(Vector2Int.down);
        }
        if (downroom && !IsSpecial(downroom))
        {
            room.RemoveWall(Vector2Int.down);
            downroom.RemoveWall(Vector2Int.up);
        }

    }

    Room GetRoomAt(Vector2Int index)
    {
        //print($"x: {index.x};;; y: {index.y}");
        Room room = rooms.Find(x => x.GetComponent<Room>().position == index); // Special Rooms should not count for the adjacent rooms
        if (room == null) return null;
        return room;
    }

    bool CheckRoomAt(Vector2Int index)
    {
        int x = index.x;
        int y = index.y;
        if (x > gridsizeX - 1 || x < 0 || y < 0 || y > gridsizeY - 1) return false;
        Room room = GetRoomAt(index);
        if (IsSpecial(room)) return false;
        return Grid[index.x, index.y];
    }

    private Vector3 GetPosFromIndex(Vector2Int index)
    {
        int gridX = index.x;
        int gridY = index.y;
        return new Vector3(roomWidth * (gridX - gridsizeX / 2),0, roomHeight * (gridY - gridsizeY / 2));
    }

    private void OnDrawGizmos()
    {
        Color gizmocolor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmocolor;

        for (int x = 0; x < gridsizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {
                Vector3 position = GetPosFromIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, 1, roomHeight));
            }
        }
/*        if (Grid!=null)
        {
            for (int x = 0; x < gridsizeX; x++)
            {
                for (int y = 0; y < gridsizeY; y++)
                {
                    if(CountAdjacentRooms(new Vector2Int(x, y)) == 1)
                    {
                        Gizmos.color = Color.red;
                        Vector3 pos = GetPosFromIndex(new Vector2Int(x, y));
                        Gizmos.DrawLine(pos,pos+Vector3.up*20);
                    }
                }
            }
        }*/
    }
}