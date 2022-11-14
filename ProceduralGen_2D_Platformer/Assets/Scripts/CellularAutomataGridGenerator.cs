using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;




public class CellularAutomataGridGenerator : MonoBehaviour
{
    public int sizeX;
    public int sizeY;
    public Tilemap tileMap;
    public Tile caveTile;
    public int density;
    public int smoothing_iterations;

    private void Start()
    {
        CellularAutomataGrid automata = new CellularAutomataGrid();
        CellularAutomataGrid.Cell[,] noisegrid = automata.MakeNoiseGrid(sizeX, sizeY, density);
        CellularAutomataGrid.Cell[,] cellGrid = automata.ApplyCellularAutomation(noisegrid, smoothing_iterations);
        
        

        
        for (int r = 0; r < cellGrid.GetLength(0); r++)
        {
            for (int c = 0; c < cellGrid.GetLength(1); c++)
            {
                
                if (cellGrid[r, c] == CellularAutomataGrid.Cell.Full)
                {
                    tileMap.SetTile(new Vector3Int(c, r), caveTile);
                }
            }
        }
    }

    public class CellularAutomataGrid
    {
        
        public CellularAutomataGrid()
        {

        }

        public enum Cell
        {
            Empty = 0,
            Full = 1,
        }

        public enum RuleSet
        {
            Cave = 0,
            Ore = 1,
        }

        //Bug free
        // Step 1
        // Create a grid with a size of x length and y heigth, then populate the grid with filled cells based on density
        // density represents percent of filled cells. (density 1 is 1% filled cells etc)
        public Cell[,] MakeNoiseGrid(int sizeX, int sizeY, int density)
        {
            Cell[,] noiseGrid = new Cell[sizeY, sizeX];

            for (int r = 0; r < noiseGrid.GetLength(0); r++)
            {

                for (int c = 0; c < noiseGrid.GetLength(1); c++)
                {
                    if (UnityEngine.Random.Range(0, 101) >= density)
                    {
                        noiseGrid[r, c] = Cell.Empty;

                    }
                    else
                    {
                        noiseGrid[r, c] = Cell.Full;

                    }

                }
            }

            return noiseGrid;
        }

        /*public Cell[,] ApplyCellularAutomation(Cell[,] grid, int smoothAmmount)
        {

            for (int s = 1; s < smoothAmmount; s++)
            {
                Cell[,] tempGrid = grid;
                for (int r = 0; r < grid.GetLength(0); r++)
                {
                    for (int c = 0; c < grid.GetLength(1); c++)
                    {


                            if (GetNeighboringCellSum(grid, x: c, y: r) > 4)
                            {
                                grid[r, c] = Cell.Full;
                            }
                            else
                            {
                                grid[r, c] = Cell.Empty;
                            }
                        

                    }
                }
            }

            return grid;
        }*/

        public Cell[,] ApplyCellularAutomation(Cell[,] grid, int count)
        {

            for (int i = 1; i < count; i++)
            {
                Cell[,] tempMap = grid;

                for (int j = 0; j < grid.GetLength(0); j++)
                {
                    for (int k = 0; k < grid.GetLength(1); k++)
                    {
                        int fullCellCount = 0;

                        // checks cell neighbors on the y axis in a 3x3 area
                        for (int y = j - 1; y <= j + 1; y++)
                        {
                            // checks cell neighbors on the x axis in a 3x3 area
                            for (int x = k - 1; x <= k + 1; x++)
                            {
                                if (IsWithinGridBounds(tempMap, x, y))
                                {
                                    // check if the cell is not the middle cell
                                    if (y != j || x != k)
                                    {
                                        // check if cell is full
                                        if (tempMap[y, x] == Cell.Full)
                                        {
                                            fullCellCount++;
                                        }
                                    }
                                }
                                else
                                {
                                    // cells out of bounds are counted as walls
                                    fullCellCount++;
                                }
                            }
                        }

                        // Cell rules #expierment with these
                        if (fullCellCount > 4)
                        {
                            grid[j, k] = Cell.Full;
                            Debug.Log("Full");
                        }
                        else
                        {
                            Debug.Log("Empty");
                            grid[j, k] = Cell.Empty;
                        }
                    }
                }
            }


            return grid;
        }

        private int GetNeighboringCellSum(Cell[,] grid, int x, int y)
        {
            int neighborCellCount = 0;

            // checks cell neighbors on the y axis in a 3x3 area
            for (int r = y - 1; r <= y + 1; r++)
            {
                // checks cell neighbors on the x axis in a 3x3 area
                for (int c = x - 1; c <= x + 1; c++)
                {
                    if (IsWithinGridBounds(grid, x, y))
                    {
                        // check if the cell is not the middle cell
                        if (r != y && c != x)
                        {
                            // check if cell is full
                            if (grid[y, x] == Cell.Full)
                            {
                                neighborCellCount++;
                            }
                        }
                    }
                    else
                    {
                        // cells out of bounds are counted as walls
                        neighborCellCount++;
                    }
                }
            }
            Debug.Log(neighborCellCount);

            return neighborCellCount;

        }

        private bool IsWithinGridBounds(Cell[,] grid, int x, int y)
        {
            
            // check if cordinate pair is within bounds of cell map grid
            if ((x < grid.GetLength(1) && x >= 0) && (y < grid.GetLength(0) && y >= 0))
            {
                
                return true;
            }
            else
            {
              
                return false;
            }
        }

        /*public Cell[,] ApplyCellularAutomation(Cell[,] grid, int count)
        {

            for (int i = 1; i < count; i++)
            {
                Cell[,] tempMap = grid;

                for (int j = 0; j < grid.GetLength(0); j++)
                {
                    for (int k = 0; k < grid.GetLength(1); k++)
                    {
                        int fullCellCount = 0;

                        // checks cell neighbors on the y axis in a 3x3 area
                        for (int y = j - 1; y <= j + 1; y++)
                        {
                            // checks cell neighbors on the x axis in a 3x3 area
                            for (int x = k - 1; x <= k + 1; x++)
                            {
                                if (IsWithinGridBounds(grid, x, y))
                                {
                                    // check if the cell is not the middle cell
                                    if (y != j || x != k)
                                    {
                                        // check if cell is full
                                        if (tempMap[y, x] == Cell.Full)
                                        {
                                            fullCellCount++;
                                        }
                                    }
                                }
                                else
                                {
                                    // cells out of bounds are counted as walls
                                    fullCellCount++;
                                }
                            }
                        }

                        // Cell rules #expierment with these
                        if (fullCellCount > 4)
                        {
                            grid[j, k] = Cell.Full;
                            Debug.Log("Full");
                        }
                        else
                        {
                            Debug.Log("Empty");
                            grid[j, k] = Cell.Empty;
                        }
                    }
                }
            }


            return grid;
        }*/


    }
}


