using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration {
    public class WorldGenerator : MonoBehaviour
    {
        //public int width;
        //public int height;
        public string seed;
        public bool useRandomSeed;
        public Tile caveTile;
        public Tile otherTile;
        public Tilemap tileMap;
        public int offset;

        public Vector3Int chunkSize;
        public int minRoomWidth;
        public int minRoomHeight;

        [Range(0, 100)]
        public int randomFillPrecent;
        public int smoothAmount;


    // Start is called before the first frame update
        void Start()
        {

            DrawRooms();

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.K))
            {

                tileMap.ClearAllTiles();

                GenerateMap();

            }
        }

        public void GenerateMap()
        {
            Cell[,] map = new Cell[minRoomHeight, minRoomWidth];
            map = ProceduralGenerationAlgorithms.CellularAutomata.GenerateNewCave(minRoomWidth, minRoomHeight, seed, true, randomFillPrecent, smoothAmount);
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if (map[r, c] == Cell.Wall)
                    {
                        tileMap.SetTile(new Vector3Int(c, r, 0), caveTile);
                    }
                }
            }

        }

        public void DrawRooms()
        {
            RoomSeperationAlgorithims.TinyKeepDungeonAlgorithim algorithim = new RoomSeperationAlgorithims.TinyKeepDungeonAlgorithim();
            
            List<Room> rooms = new List<Room>();
            rooms = algorithim.PlaceRandomRooms(10, new Vector2Int(15, 15), new Vector2Int(8, 8), 15);

                foreach (Room room in rooms)
            {
                for (int r = 0; r < room.RoomTiles.GetLength(0); r++)
                {
                    for (int c = 0; c < room.RoomTiles.GetLength(1); c++)
                    {
                        if (room.RoomTiles[r, c] == Cell.Wall)
                            tileMap.SetTile(new Vector3Int(room.RoomBounds.x - room.RoomBounds.size.x / 2  + c, room.RoomBounds.position.y - room.RoomBounds.size.y / 2 + r), caveTile);
                    }
                }
            }
        }


    }
}
