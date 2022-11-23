using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace WorldGeneration
{
    public enum TileType
    {
        Wall = 0,
        Background = 1,
        Chest = 2,
        Moss = 3,
    }

    public class Room
    {
        //private TileType[,] tileMap;
        private Cell[,] roomTiles;

        private Vector2Int roomSize;
        private BoundsInt roomBounds;
        /*public Room(TileType[,] tileMap)
        {
            this.tileMap = tileMap;
        }*/

        public Room(BoundsInt roomBounds)
        {
            this.roomBounds = roomBounds;
            roomTiles = ProceduralGenerationAlgorithms.CellularAutomata.GenerateNewCave(roomBounds.size.x, roomBounds.size.y, "6", true, 50, 3);
        }

        public Cell[,] RoomTiles
        {
            get { return roomTiles; }
            set { roomTiles = value; }
        }


        public BoundsInt RoomBounds
        {
            get { return roomBounds;}
            set { roomBounds = value; }
        }

    }
    public class RoomSeperationAlgorithims
    {

        public class TinyKeepDungeonAlgorithim
        {
            List<Room> roomList;
            public List<Room> PlaceRandomRooms(int roomSpawnRadius, Vector2Int maxRoomSize, Vector2Int minRoomSize, int numberOfRooms)
            {
                roomList = new List<Room>();

                if((maxRoomSize.x <= roomSpawnRadius * 2) || (maxRoomSize.y <= roomSpawnRadius * 2))
                {
                    for(int i = 0; i <= numberOfRooms; i++)
                    {
                        // find random room size
                        Vector2Int randomRoomSize = new Vector2Int((int)Random.Range(minRoomSize.x, maxRoomSize.x), (int)Random.Range(minRoomSize.y, maxRoomSize.y));

                        // find random position such that the sides of the room dont go outside the radius
                        Vector2Int randomRoomPosition = new Vector2Int((int)Random.Range(randomRoomSize.x / 2 - (roomSpawnRadius * 2), (roomSpawnRadius * 2) - randomRoomSize.x /2),
                            (int)Random.Range(randomRoomSize.y / 2 - (roomSpawnRadius * 2), (roomSpawnRadius * 2) - randomRoomSize.y / 2));

                        Room newRoom = new Room(new BoundsInt((Vector3Int)randomRoomPosition, (Vector3Int)randomRoomSize));
                        
                        roomList.Add(newRoom);
                        
                    }
                }

                return roomList;
            }
        }

        public class RoomBSPGenerator
        {
            // TODO improve logic later
            public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
            {
                Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
                List<BoundsInt> roomsList = new List<BoundsInt>();

                //if (spaceToSplit.size.x > 0 && spaceToSplit.size.y > 0)
                roomsQueue.Enqueue(spaceToSplit);

                while (roomsQueue.Count > 0)
                {
                    var room = roomsQueue.Dequeue();


                    // if room is splittable
                    //ToDo combine redundent logic
                    if (room.size.y >= minHeight && room.size.x >= minWidth)
                    {
                        // randomly decide to split the room horozontaly or vertically
                        if (Random.value < 0.5f)
                        {
                            // Splits horizontally first
                            if (room.size.y >= minHeight * 2)
                            {
                                SplitHorizontally(minWidth, roomsQueue, room);
                            }
                            else if (room.size.x >= minWidth * 2)
                            {
                                SplitVertically(minHeight, roomsQueue, room);
                            }
                            else if (room.size.x >= minWidth && room.size.y >= minHeight)
                            {
                                roomsList.Add(room);
                            }
                        }
                        else
                        {
                            // Splits vertically first
                            if (room.size.x >= minWidth * 2)
                            {
                                SplitVertically(minWidth, roomsQueue, room);
                            }
                            else if (room.size.y >= minHeight * 2)
                            {
                                SplitHorizontally(minHeight, roomsQueue, room);
                            }

                            else if (room.size.x >= minWidth && room.size.y >= minHeight)
                            {
                                roomsList.Add(room);
                            }
                        }
                    }
                }

                return roomsList;
            }

            private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
            {
                // choose random location on x axis to split room
                int xSplit = Random.Range(1, room.size.x);

                // Create bounds of new rooms
                // room A is left split and room B is the right split
                BoundsInt roomA = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
                BoundsInt roomB = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
                    new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));

                roomsQueue.Enqueue(roomA);
                roomsQueue.Enqueue(roomB);
            }

            private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
            {
                // choose random location on y axis to split room
                int ySplit = Random.Range(1, room.size.y);

                // Create bounds of new rooms
                // room A is bottom split and room B is the top split
                BoundsInt roomA = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
                BoundsInt roomB = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
                    new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));

                roomsQueue.Enqueue(roomA);
                roomsQueue.Enqueue(roomB);


            }
        }
    }
}