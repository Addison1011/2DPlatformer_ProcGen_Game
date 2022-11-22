using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace WorldGeneration
{
    public enum Cell
    {
        Empty = 0,
        Wall = 1,
    }

    public class CaveGenerator
    {
        public static int height;
        public static int width;
        public static int randomFillPercent;
        public static int smoothAmount;

        public string seed;

        public bool useRandomSeed;
        public static Cell[,] map;
        public static Cell[,] tempMap;


        // Generate new different map without creating new object
        public static Cell[,] GenerateMap(int newWidth, int newHeight, string newSeed, bool useRandomSeed, int randomFillPercent, int smoothAmount)
        {
            // reset width and height
            height = newHeight;
            width = newWidth;
            map = new Cell[newHeight, newWidth];

            RandomFillMap(useRandomSeed, newSeed, randomFillPercent);

            for (int i = 0; i < smoothAmount; i++)
            {
                SmoothMap();
            }

            return map;

        }

        // Fills map with random Wall/Empty Cells based on newly generated parameters
        private static void RandomFillMap(bool useRandomSeed, string seed, int randomFillPercent)
        {
            if (useRandomSeed)
            {
                seed = Guid.NewGuid().GetHashCode().ToString();
                //print(Guid.NewGuid().GetHashCode());
            }

            System.Random pseudoRandom = new System.Random(seed.GetHashCode());
            MonoBehaviour.print(seed.GetHashCode());

            // populate grid with eandom amount of wall/empty cells based of fill percent
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                    {
                        map[y, x] = Cell.Wall;
                    }
                    else
                    {
                        // ( higher percent = more walls )
                        if (pseudoRandom.Next(0, 100) < randomFillPercent)
                            map[y, x] = Cell.Wall;
                        else
                            map[y, x] = Cell.Empty;
                    }

                    /* rudimentary path
                    if (y > height/2 - 3 && y < height/2+3)
                    {
                        if (pseudoRandom.Next(0, 100) < 15)
                            map[y,x] = Cell.Wall;
                        else
                            map[y, x] = Cell.Empty;
                    }*/
                }
            }
        }

   
        private static void SmoothMap()
        {
            tempMap = map;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(y, x);
                    // TODO: Experiment with this value
                    if (neighbourWallTiles > 4)
                        map[y, x] = Cell.Wall;
                    else if (neighbourWallTiles < 4)
                        map[y, x] = Cell.Empty;
                }
            }
        }

        private static int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;

            // Itterate through the temporary cell map to look for any walls in a radius of 1.
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    // Check to see if cell is within the bounds of the grid.
                    if ((neighbourX >= 0) && (neighbourX < height) && (neighbourY >= 0) && (neighbourY < width))
                    {
                        // Dont check middle cell, but the cells around the middle.
                        if (neighbourX != gridX || neighbourY != gridY)
                        {

                            if (tempMap[neighbourX, neighbourY] == Cell.Wall)
                            {
                                wallCount++;
                            }
                        }
                    }
                    else
                    {
                        // Cells that are out of bounds count as wall cells. Encourages growth of walls around maps edge.
                        wallCount++;
                    }
                }
            }
            return wallCount;
        }

        public static Vector3Int FindCellLocation(Vector3Int mapOriginPosition, int yRow, int xCol)
        {
            return (new Vector3Int(mapOriginPosition.x + xCol, mapOriginPosition.y + yRow, mapOriginPosition.z));
        }
    }
}
