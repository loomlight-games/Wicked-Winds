using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TownGenerator
{
    public enum TileType {Residential, Forest, Park, Market, Swamp}
    public enum Town {Summer, Autumn, Winter}

    int townSize, randomIdx;
    float tileSize, currentXpos, currentZpos, initialPos, randomRotation;
    Vector3[,] tilesPositions;
    Vector3 currentPosition;
    GameObject townParent, currentTile;
    TownTile tileData;
    Dictionary<TileType, bool> isTypeInstantiated = new();
    List<GameObject> townTiles = new();
    
    public void Start()
    {
        tileSize = GameManager.Instance.tileSize;
        townSize = GameManager.Instance.townSize;

        // Select tiles according to map theme
        townTiles = GameManager.Instance.town switch
        {
            Town.Summer => GameManager.Instance.summerTownTiles,
            Town.Autumn => GameManager.Instance.autumnTownTiles,
            Town.Winter => GameManager.Instance.winterTownTiles,
            _ => GameManager.Instance.summerTownTiles,
        };

        // Initialize all tile types in dictionary as false
        foreach (TileType type in Enum.GetValues(typeof(TileType)))
            isTypeInstantiated.Add(type,false); // Not instantiated yet

        CalculatePositions(); // Fills positions arrays calculating them - from upper left corner

        InstantiateTiles(); // Instantiates a town tile in each position - from center
    }

    /// <summary>
    /// Fills positions arrays calculating them - from upper left corner
    /// </summary>
    void CalculatePositions()
    {
        // Initialize positions array
        tilesPositions = new Vector3 [townSize, townSize];

        // Calculate initial positions in Z and X axis
        initialPos = tileSize/2 * (townSize-1);
        currentZpos = initialPos;
        currentXpos = -initialPos; // from upper left corner

        // Calculate positions
        for (int row = 0; row < townSize; row++){
            for (int column = 0; column < townSize; column++){
                currentPosition = new Vector3(currentXpos, 0f, currentZpos);

                tilesPositions[row,column] = currentPosition;

                currentZpos -= tileSize;
            }
            currentZpos = initialPos; // Returns to first row position
            currentXpos += tileSize;
        }
    }

    /// <summary>
    /// Instantiates a town tile in each position - from center
    /// </summary>
    void InstantiateTiles()
    {
        // Instantiate tiles parent
        townParent = GameManager.Instance.InstantiateGO(GameManager.Instance.townParent, new Vector3(0,0,0), Quaternion.Euler(0, 0, 0));

        // Starting position near center
        int row = townSize / 2 - 1;
        int col = row;

        // Define the directions in the order of Right, Down, Left, Up
        int[] dRow = { 0, 1, 0, -1 };
        int[] dCol = { 1, 0, -1, 0 };

        int steps = 1; // Number of steps in the current direction
        while (steps < townSize) // Continue until we cover the matrix
        {
            for (int direction = 0; direction < 4; direction++)
            {
                // Traverse in the current direction for 'steps' times
                for (int step = 0; step < steps; step++)
                {
                    InstantiateTile(tilesPositions[row,col]);

                    // Ensure we don't go out of bounds
                    if (row >= 0 && row < townSize && col >= 0 && col < townSize)
                    {
                        row += dRow[direction];
                        col += dCol[direction];
                    }
                }

                // After moving left or right, we increase the steps (to move in a wider spiral)
                if (direction == 1 || direction == 3) steps++;
            }
        }
    }

    /// <summary>
    /// Instantiate town tile in given position
    /// </summary>
    void InstantiateTile(Vector3 position)
    {
        // Random tile
        randomIdx = UnityEngine.Random.Range(0, townTiles.Count);
        currentTile = townTiles[randomIdx];
        tileData = currentTile.GetComponent<TownTile>();

        // This type has been already instantiated
        if (tileData.isUnique && isTypeInstantiated[tileData.type])
            InstantiateTile(position); // Finds another random tile - RECURSION
        // Not instantiated yet
        else{
            isTypeInstantiated[tileData.type] = true; // Now is instantiated
            randomRotation = UnityEngine.Random.Range(0, 4) * 90f; // 0,90,180,270

            // Instantiate tile in position with random rotation on Y
            GameManager.Instance.InstantiateGO(currentTile, position, Quaternion.Euler(0, randomRotation, 0), townParent.transform);
        }
    }
}