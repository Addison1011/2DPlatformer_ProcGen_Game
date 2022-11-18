using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration {
    public class WorldGenerator : MonoBehaviour
    {
        private CaveGenerator caveGenerator;
        public int width;
        public int height;
        public string seed;
        public bool useRandomSeed;
        public Tile caveTile;
        public Tilemap tileMap;

        [Range(0, 100)]
        public int randomFillPrecent;
        public int smoothAmount;
        Cell[,] caveMap;

    // Start is called before the first frame update
    void Start()
        {
            caveGenerator = new CaveGenerator(width, height, seed, useRandomSeed, randomFillPrecent, smoothAmount);
            caveMap = caveGenerator.GenerateMap();
            for (int r = 0; r < caveMap.GetLength(0); r++)
            {
                for (int c = 0; c < caveMap.GetLength(1); c++)
                {

                    if (caveMap[r, c] == Cell.Wall)
                    {
                        tileMap.SetTile(new Vector3Int(c, r), caveTile);
                    }
                }
            }

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.K))
            {

                tileMap.ClearAllTiles();

                caveMap = caveGenerator.GenerateMap(width, height, seed, useRandomSeed, randomFillPrecent, smoothAmount);

                for (int r = 0; r < caveMap.GetLength(0); r++)
                {
                    for (int c = 0; c < caveMap.GetLength(1); c++)
                    {

                        if (caveMap[r, c] == Cell.Wall)
                        {
                            tileMap.SetTile(new Vector3Int(c, r), caveTile);
                        }
                    }
                }
            }
        }
    }
}
