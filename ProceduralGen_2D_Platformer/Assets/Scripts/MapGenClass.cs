using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGeneratorTestClass
{
    public int width;
    public int height;
    public int fillPercent;

    public string seed;

    public bool useRandomSeed;
    
    int[,] map;
    int[,] tempMap;

    public MapGeneratorTestClass()
    {

    }


    void GenerateMap(int width, int height, string seed, bool useRandomSeed, int fillPercent)
    {

        this.map = new int[width, height];

        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }


    }

    void RandomFillMap(bool useRandomSeed, string seed)
    {
        if (useRandomSeed)
        {
            seed = Guid.NewGuid().GetHashCode().ToString();
            //print(Guid.NewGuid().GetHashCode());
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        print(seed.GetHashCode());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    if (pseudoRandom.Next(0, 100) < randomFillPrecent)
                        map[x, y] = 1;
                    else
                        map[x, y] = 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        tempMap = map;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);
                // TODO: Experiment with this value
                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;
            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if ((neighbourX >= 0) && (neighbourX < width) && (neighbourY >= 0) && (neighbourY < height))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += tempMap[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }
}
