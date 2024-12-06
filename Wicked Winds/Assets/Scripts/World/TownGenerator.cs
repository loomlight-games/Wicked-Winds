using System;
using System.Collections.Generic;
using UnityEngine;

public class TownGenerator
{
    public enum TileType { Residential, Forest, Park, Market, Swamp }
    public enum Town { StardustTown, SandyLandy, FrostpeakHollow }

    int townSize, randomIdx;
    float tileSize, currentXpos, currentZpos, initialPos, randomRotation;
    Vector3[,] tilesPositions;
    Vector3 currentPosition;
    GameObject townParent, currentTile, landscape;
    TownTile tileData;
    Dictionary<TileType, bool> isTypeInstantiated = new();
    List<GameObject> townTiles = new();
    public GameObject fogTriggerPrefab;

    public void GenerateTown()
    {
        tileSize = GameManager.Instance.tileSize;
        townSize = GameManager.Instance.townSize;

        fogTriggerPrefab = FogManager.Instance.FogTriggerPrefab;

        // Select tiles according to map theme
        townTiles = GameManager.Instance.town switch
        {
            Town.StardustTown => GameManager.Instance.townTiles1,
            Town.SandyLandy => GameManager.Instance.townTiles2,
            Town.FrostpeakHollow => GameManager.Instance.townTiles3,
            _ => GameManager.Instance.townTiles1,
        };

        // Select landscape according to map theme
        landscape = GameManager.Instance.town switch
        {
            Town.StardustTown => GameManager.Instance.landscape1,
            Town.SandyLandy => GameManager.Instance.landscape2,
            Town.FrostpeakHollow => GameManager.Instance.landscape3,
            _ => GameManager.Instance.landscape1,
        };

        // Reset all tile types in dictionary as false - not instantiated yet
        foreach (TileType type in Enum.GetValues(typeof(TileType)))
        {
            // Adds new types if aren't considered already
            if (!isTypeInstantiated.ContainsKey(type))
                isTypeInstantiated.Add(type, false);

            // Not instantiated yet
            isTypeInstantiated[type] = false;
        }

        // Instantiate town parent
        townParent = GameObject.Find("TownParent") ?? new GameObject("TownParent");

        // Instantiate landscape as child of town
        GameManager.Instance.InstantiateGO(landscape, townParent.transform.position, townParent.transform.rotation, townParent.transform);

        CalculatePositions(); // Fills positions arrays calculating them - from upper left corner

        InstantiateTiles(); // Instantiates a town tile in each position - from center
    }

    /// <summary>
    /// Fills positions arrays calculating them - from upper left corner
    /// </summary>
    void CalculatePositions()
    {
        // Initialize positions array
        tilesPositions = new Vector3[townSize, townSize];

        // Calculate initial positions in Z and X axis
        initialPos = tileSize / 2 * (townSize - 1);
        currentZpos = initialPos;
        currentXpos = -initialPos; // from upper left corner

        // Calculate positions
        for (int row = 0; row < townSize; row++)
        {
            for (int column = 0; column < townSize; column++)
            {
                currentPosition = new Vector3(currentXpos, 0f, currentZpos);

                tilesPositions[row, column] = currentPosition;

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
                    if (row >= 0 && row < townSize &&
                        col >= 0 && col < townSize)
                    {
                        InstantiateTile(tilesPositions[row, col]);

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
        else
        {
            isTypeInstantiated[tileData.type] = true; // Now is instantiated
            randomRotation = UnityEngine.Random.Range(0, 4) * 90f; // 0,90,180,270

            // Instantiate tile in position with random rotation on Y
            GameObject instantiatedTile = GameManager.Instance.InstantiateGO(currentTile, position, Quaternion.Euler(0, randomRotation, 0), townParent.transform);

            AddFogTriggerRandomly(instantiatedTile, tileData);
        }
    }

    /// <summary>
    /// Adds fog trigger randomly
    /// </summary>
    void AddFogTriggerRandomly(GameObject tile, TownTile tileData)
    {
        float fogChance = 0.2f;
        if (UnityEngine.Random.value < fogChance)
        {
            tileData.hasFog = true;
            GameObject fogTrigger = GameObject.Instantiate(fogTriggerPrefab, tile.transform.position, tile.transform.rotation, tile.transform);
        }
    }
}