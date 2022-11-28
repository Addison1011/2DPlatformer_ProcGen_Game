using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace WorldGeneration
{
    public enum TileType
    {
        Empty = 0,
        Wall = 1,
        Background = 2,
        Chest = 3,
        Moss = 4,
    }

    public class Room
    {
        //private TileType[,] tileMap;
        private TileType[,] tileTypeMap;
        private List<Tile> roomTiles;
        private Tile caveTile;
        private Vector2Int roomSize;
        private BoundsInt roomBounds;
        /*public Room(TileType[,] tileMap)
        {
            this.tileMap = tileMap;
        }*/

        public Room(BoundsInt roomBounds, bool useRandomSeed, int randomFillPercent, int smoothValue, Tile caveTile)
        {
            this.roomBounds = roomBounds;
            Cell[,] map = ProceduralGenerationAlgorithms.CellularAutomata.GenerateNewCave(roomBounds.size.x, roomBounds.size.y, "6", useRandomSeed, randomFillPercent, smoothValue);
            tileTypeMap = new TileType[roomBounds.size.y, roomBounds.size.x];
            this.caveTile = caveTile;
            AssignCaveTileLayout(ref tileTypeMap, map);

            
        }

        private void AssignCaveTileLayout(ref TileType[,] roomTileTypes, Cell[,] cellMap)
        {
            for(int r = 0; r < cellMap.GetLength(0); r++)
            {
                for (int c = 0; c < cellMap.GetLength(1); c++)
                {
                    if (cellMap[r,c] == Cell.Empty)
                    {
                        roomTileTypes[r, c] = TileType.Empty;
                    }
                    else if (cellMap[r, c] == Cell.Wall)
                    {
                        roomTileTypes[r, c] = TileType.Wall;
                    }
                }
            }
        }

        public void RemoveRoomTiles(ref Tilemap tileMap)
        {
            for (int r = 0; r < tileTypeMap.GetLength(0); r++)
            {
                for (int c = 0; c < tileTypeMap.GetLength(1); c++)
                {
                    if (tileTypeMap[r, c] == TileType.Wall)
                        tileMap.SetTile(new Vector3Int(roomBounds.x - roomBounds.size.x / 2 + c, roomBounds.position.y - roomBounds.size.y / 2 + r), null);
                }
            }
        }

        public void PlaceRoomTiles(ref Tilemap tileMap)
        {
            for (int r = 0; r < tileTypeMap.GetLength(0); r++)
            {
                for (int c = 0; c < tileTypeMap.GetLength(1); c++)
                {
                    if (tileTypeMap[r, c] == TileType.Wall)
                        tileMap.SetTile(new Vector3Int(roomBounds.x - roomBounds.size.x / 2 + c, roomBounds.position.y - roomBounds.size.y / 2 + r), caveTile);
                }
            }
        }

        private void CreateRandomPath()
        {
            //Make random pathway through cave
        }

        public TileType[,] RoomTileMap
        {
            get { return tileTypeMap; }
            set { tileTypeMap = value; }
        }


        public BoundsInt RoomBounds
        {
            get { return roomBounds;}
            set { roomBounds = value; }
        }

        

    }
}