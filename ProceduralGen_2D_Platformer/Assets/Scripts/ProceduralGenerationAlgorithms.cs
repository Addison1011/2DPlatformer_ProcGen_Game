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

    public class ProceduralGenerationAlgorithms
    {
        //public static int height;
       // public static int width;
       // public static int randomFillPercent;
        //public static int smoothAmount;

        //public string seed;

        //public bool useRandomSeed;


      

        public class CellularAutomata
        {
            // Generate new different map without creating new object
            public static Cell[,] GenerateNewCave(int width, int height, string seed, bool useRandomSeed, int randomFillPercent, int smoothAmount)
            {
                // reset width and height
                Cell[,] map = RandomFillMap(width, height, seed, useRandomSeed, randomFillPercent);

                for (int i = 0; i < smoothAmount; i++)
                {
                    SmoothMap(ref map);
                }

                return map;

            }

            public static Cell[,] GenerateNewCave(Cell[,] noiseMap, int smoothAmount)
            {
                Cell[,] map = noiseMap;

                for (int i = 0; i < smoothAmount; i++)
                {
                    SmoothMap(ref map);
                }

                return map;
            }


            // Fills map with random Wall/Empty Cells based on newly generated parameters
            public static Cell[,] RandomFillMap(int width, int height, string seed, bool useRandomSeed, int randomFillPercent)
            {
                Cell[,] randomMap = new Cell[height, width];

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
                            randomMap[y, x] = Cell.Wall;
                        }
                        else
                        {
                            // ( higher percent = more walls )
                            if (pseudoRandom.Next(0, 100) < randomFillPercent)
                                randomMap[y, x] = Cell.Wall;
                            else
                                randomMap[y, x] = Cell.Empty;
                        }
                    }
                }

                return randomMap;
            }


            public static void SmoothMap(ref Cell[,] map)
            {
                Cell[,] tempMap = map;

                int height = tempMap.GetLength(0);
                int width = tempMap.GetLength(1);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int neighbourWallTiles = GetSurroundingWallCount(ref tempMap, y, x);
                        // TODO: Experiment with this value
                        // If amount of neighboring cells that are walls is > 4 then make current cell a wall, else make cell empty
                        if (neighbourWallTiles > 4)
                            map[y, x] = Cell.Wall;
                        else if (neighbourWallTiles < 4)
                            map[y, x] = Cell.Empty;
                    }
                }
            }

            private static int GetSurroundingWallCount(ref Cell[,] tempMap, int gridX, int gridY)
            {
                int wallCount = 0;
                int height = tempMap.GetLength(0);
                int width = tempMap.GetLength(1);

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
}
