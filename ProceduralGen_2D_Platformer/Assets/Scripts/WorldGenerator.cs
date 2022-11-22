using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration {
    public class WorldGenerator : MonoBehaviour
    {
        private RoomBSPGenerator roomBSPGenerator;
        //public int width;
        //public int height;
        public string seed;
        public bool useRandomSeed;
        public Tile caveTile;
        public Tilemap tileMap;
        public int offset;

        public Vector3Int chunkSize;
        public int minRoomWidth;
        public int minRoomHeight;

        [Range(0, 100)]
        public int randomFillPrecent;
        public int smoothAmount;
        Cell[,] caveMap;

    // Start is called before the first frame update
        void Start()
        {






            CreateRooms();

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.K))
            {

                tileMap.ClearAllTiles();

                CreateRooms();

            }
        }

        
        public void CreateRooms()
        {
            List<Room> roomList = GenerateRoomList();
            foreach(Room room in roomList)
            {
                room.CellMap = CaveGenerator.GenerateMap(room.Bounds.x, room.Bounds.y, seed, useRandomSeed, randomFillPrecent, smoothAmount);
            }

            

            foreach (Room room in roomList)
            {

                for(int r = 0; r < room.CellMap.GetLength(0); r++)
                {
                    for (int c = 0; c < room.CellMap.GetLength(1); c++)
                    {
                        if (room.CellMap[r, c] == Cell.Wall)
                        {
                            tileMap.SetTile(new Vector3Int(room.Position.x + c, room.Position.y + r), caveTile);
                        }
                    }
                }
            }

        }

        private List<Room> GenerateRoomList()
        {
            // create room list with position and size defined
            List<BoundsInt> roomListBSP = RoomBSPGenerator.BinarySpacePartitioning(new BoundsInt(new Vector3Int(0, 0, 0), chunkSize), minRoomWidth, minRoomHeight);
            List<Room> rooms = new List<Room>();
            int x = minRoomWidth;
            int y = minRoomHeight;
            foreach (BoundsInt i in roomListBSP)
            {
                
                rooms.Add(new Room(new Vector3Int(x, y, 0), i));
                x += minRoomWidth;
                y += minRoomHeight;
            }

            return rooms;
        }
    }
}
