using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
	public int width;
	public int height;
	public string seed;
	public bool useRandomSeed;
	public Tile caveTile;
	public Tilemap tileMap;

	[Range(0, 100)]
	public int randomFillPrecent;
	int[,] map;
	int[,] tempMap;

	void Start()
	{
		GenerateMap();

		for (int r = 0; r < map.GetLength(0); r++)
		{
			for (int c = 0; c < map.GetLength(1); c++)
			{

				if (map[r, c] == 1)
				{
					tileMap.SetTile(new Vector3Int(c, r), caveTile);
				}
			}
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
            tileMap.ClearAllTiles();

            GenerateMap();

            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {

                    if (map[r, c] == 1)
                    {
                        tileMap.SetTile(new Vector3Int(c, r), caveTile);
                    }
                }
            }
        }
	}

	void GenerateMap()
	{

		map = new int[width, height];
		
		RandomFillMap();

		for (int i = 0; i < 5; i++)
		{
			SmoothMap();
		}


	}

	void RandomFillMap()
	{
		if (useRandomSeed)
		{
			seed = Guid.NewGuid().GetHashCode().ToString();
			Guid
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

	/*void OnDrawGizmos()
	{
		if (map != null)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (map[x, y] == 1)
						Gizmos.color = Color.black;
					else
						Gizmos.color = Color.white;
						
					Vector3 pos = new Vector3(-width/2 + x + 0.5f, 0, -height/2 + y + 0.5f);
					Gizmos.DrawCube(pos, Vector3.one);
				}
			}
		}
	}*/
}
