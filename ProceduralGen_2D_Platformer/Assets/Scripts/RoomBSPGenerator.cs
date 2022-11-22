using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

namespace WorldGeneration
{
    public class RoomBSPGenerator
    {
        // TODO improve logic later
        public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
        {
            Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
            List<BoundsInt> roomsList = new List<BoundsInt>();

            //if (spaceToSplit.size.x > 0 && spaceToSplit.size.y > 0)
            roomsQueue.Enqueue(spaceToSplit);
            
            while(roomsQueue.Count > 0)
            {
                var room = roomsQueue.Dequeue();

                // if room is splittable
                //ToDo combine redundent logic
                if(room.size.y >= minHeight && room.size.x >= minWidth)
                {
                    // randomly decide to split the room horozontaly or vertically
                    if (Random.value < 0.5f)
                    {
                        // Splits horizontally first
                        if (room.size.y >= minHeight * 2)
                        {
                            SplitHorizontally(minWidth, roomsQueue, room);
                        }
                       else if(room.size.x >= minWidth * 2)
                        {
                            SplitVertically(minHeight, roomsQueue, room);
                        }
                       else if(room.size.x >= minWidth && room.size.y >= minHeight)
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

    public class Room
    {
        private Vector3Int position { get; set; }
        private BoundsInt bounds { get; set; }
        private Cell[,] cellMap { get; set; }

        public Room(Vector3Int position, BoundsInt roomBounds)
        {
            this.position = position;
            this.bounds = roomBounds;
        }

        public Vector3Int Position
        {
            get { return position; }
            set { position = value; }
        }

        public BoundsInt Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        public Cell[,] CellMap
        {
            get { return cellMap; }
            set { cellMap = value; }
        }
    }
}

