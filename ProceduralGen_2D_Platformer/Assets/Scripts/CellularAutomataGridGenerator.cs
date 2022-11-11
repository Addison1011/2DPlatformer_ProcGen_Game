using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;




public class CellularAutomataGridGenerator : MonoBehaviour
{
    private Cell[,] cellGrid;
    public int sizeX;
    public int sizeY;
    private Cell[,] caveGrid;
    public Tilemap tileMap;
    public Tile caveTile;
    public int density;
    public int smoothing_iterations;
    private void Start()
    {
        Cell[,] cellGrid = new Cell[sizeY, sizeX];

        caveGrid = GetAutomationGrid(sizeX, sizeY, density, smoothing_iterations);
        for (int r = 0; r < caveGrid.GetLength(0); r++)
        {
            for (int c = 0; c < caveGrid.GetLength(1); c++)
            {
                //Debug.Log(caveGrid[r, c]);
                if (caveGrid[r, c] == Cell.Full)
                {
                    tileMap.SetTile(new Vector3Int(c, r), caveTile);
                }
            }
        }
    }

    public enum Cell
    {
        Empty = 0,
        Full = 1,
    }

    public Cell[,] GetAutomationGrid(int sizeX, int sizeY, int density, int iterations)
    {
        cellGrid = MakeNoiseGrid(sizeX, sizeY, density);
        Cell[,] grid = new Cell[sizeX, sizeY];
        grid = ApplyCellularAutomation(cellGrid, iterations);
        return grid;
    }

    private Cell[,] MakeNoiseGrid(int sizeX, int sizeY, int density)
    {
        Cell[,] noiseGrid = new Cell[sizeY, sizeX];
        
        for (int r = 0; r < noiseGrid.GetLength(0); r++)
        {
            Debug.Log("empty");
            for (int c = 0; c < noiseGrid.GetLength(1); c++)
            {
                if (UnityEngine.Random.Range(0, 101) > density)
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

    private Cell[,] ApplyCellularAutomation(Cell[,] grid, int count)
    {
        Cell[,] tempMap;
        tempMap = grid;
        for (int i = 1; i < count; i++)
        {
            tempMap = grid;

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
                            if (IsWithinGridBounds(y, x))
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
            

        return tempMap;
    }

    private bool IsWithinGridBounds(int x, int y)
    {
        // check if cordinate pair is within bounds of cell map grid
        if (x > cellGrid.GetLength(0) - 1 || x < 0 || y > cellGrid.GetLength(1) - 1 || y < 0)
        {
            Debug.Log("false");
            return false;
        }
        else
        {
            Debug.Log("false");
            return true;
        }
    }
}


