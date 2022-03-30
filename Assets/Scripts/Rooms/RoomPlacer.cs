using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomPlacer : MonoBehaviour
{
    [SerializeField] private Room[] RoomPrefabs;
    [SerializeField] private Room[] BossRooms;

    [SerializeField] private Room StartingRoom;

    public Room[,] spawnedRooms;

    [SerializeField] private int roomsWidth;
    [SerializeField] private int roomsHeight;

    private void Start()
    {
        roomsWidth = Random.Range(5, 9);
        roomsHeight = Random.Range(3, 9);

        spawnedRooms = new Room[roomsWidth, roomsHeight];
        spawnedRooms[roomsWidth / 2, roomsHeight / 2] = StartingRoom;

        for (int i = 0; i < (roomsWidth + roomsHeight) / 2; i++)
        {
            bool isBossRoom = i == ((roomsHeight + roomsWidth) / 2) - 1;

            PlaceOneRoom(isBossRoom);
        }
    }
    private void PlaceOneRoom(bool isBossRoom)
    {
        HashSet<Vector2> vacantPlaces = new HashSet<Vector2>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2(x, y + 1));
            }
        }

        Room newRoom = null;
        if (!isBossRoom)
            newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)]);
        else
        {

            newRoom = Instantiate(BossRooms[Random.Range(0, BossRooms.Length)]);
            newRoom.isBoss = true;
        }

        newRoom.transform.SetParent(transform);


        Vector2 position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));

        newRoom.transform.position = new Vector3(((int)position.x - roomsWidth / 2) * 50, ((int)position.y - roomsHeight / 2) * 28); //28/50
        spawnedRooms[(int)position.x, (int)position.y] = newRoom;


        ConnectToRandomRoom(newRoom, position);
    }

    private bool ConnectToRandomRoom(Room room, Vector2 p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2> neighbours = new List<Vector2>();

        try
        {
            if (room.DoorU != null && (int)p.y < maxY && spawnedRooms[(int)p.x, (int)p.y + 1].DoorD != null) neighbours.Add(Vector2.up);
        } catch { }
        try
        {
            if (room.DoorD != null && (int)p.y > 0 && spawnedRooms[(int)p.x, (int)p.y - 1].DoorU != null) neighbours.Add(Vector2.down);
        } catch { }
        try
        {
            if (room.DoorR != null && (int)p.x < maxX && spawnedRooms[(int)p.x + 1, (int)p.y].DoorL != null) neighbours.Add(Vector2.right);
        }
        catch { }
        try
        {
            if (room.DoorL != null && (int)p.x > 0 && spawnedRooms[(int)p.x - 1, (int)p.y].DoorR != null) neighbours.Add(Vector2.left);
        }
        catch { }

        if (neighbours.Count == 0)
            return false;

        Vector2 selectedDir = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[(int)p.x + (int)selectedDir.x, (int)p.y + (int)selectedDir.y];

        if (selectedDir == Vector2.up)
        {
            room.DoorU.GetComponent<Locks>().isDoor = true;
            selectedRoom.DoorD.GetComponent<Locks>().isDoor = true;

            room.DoorU.SetActive(false);
            selectedRoom.DoorD.SetActive(false);
        }
        else if (selectedDir == Vector2.down)
        {
            room.DoorD.GetComponent<Locks>().isDoor = true;
            selectedRoom.DoorU.GetComponent<Locks>().isDoor = true;

            room.DoorD.SetActive(false);
            selectedRoom.DoorU.SetActive(false);
        }
        else if (selectedDir == Vector2.right)
        {
            room.DoorR.GetComponent<Locks>().isDoor = true;
            selectedRoom.DoorL.GetComponent<Locks>().isDoor = true;

            room.DoorR.SetActive(false);
            selectedRoom.DoorL.SetActive(false);
        }
        else if (selectedDir == Vector2.left)
        {
            room.DoorL.GetComponent<Locks>().isDoor = true;
            selectedRoom.DoorR.GetComponent<Locks>().isDoor = true;

            room.DoorL.SetActive(false);
            selectedRoom.DoorR.SetActive(false);
        }
        return true;
    }
}