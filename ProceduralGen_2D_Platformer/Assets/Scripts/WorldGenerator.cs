using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
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

        [Range(0, 100)]
        public int randomFillPrecent;
        public int smoothAmount;
        public BoundsInt roomBounds;
        private Room room1;


    // Start is called before the first frame update
        void Start()
        {

            room1 = new Room(roomBounds, true, randomFillPrecent, smoothAmount, caveTile);
            room1.PlaceRoomTiles(ref tileMap);

        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.K))
            {

                room1.RemoveRoomTiles(ref tileMap);
                room1 = new Room(roomBounds, true, randomFillPrecent, smoothAmount, caveTile);
                
                room1.PlaceRoomTiles(ref tileMap);
                //tileMap.ClearAllTiles();
                

            }
        }


    }
}
