using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGenerator : MonoBehaviour
{
    public enum TileType {Residential, Forest, Park, Market, Swamp}

    public float tileSize = 50f;
    public int townSize = 4; // In tiles

    // List of 3D positions where to spawn town tiles
    //public List<List<Vector3>> tilesPositions = new ();
    
    // List of town tiles
    public List<GameObject> townTiles = new();

    int randomIdx;
    float currentXpos, currentZpos, initialPos, randomRotation;
    Vector3 currentPosition;
    GameObject currentTile;
    TownTile tileData;

    public Dictionary<TileType, bool> isTypeInstantiated = new();

    // Start is called before the first frame update
    void Start()
    {
        // Initialize all tile types in dictionary as false
        foreach (TileType type in Enum.GetValues(typeof(TileType)))
            isTypeInstantiated.Add(type,false); // Not instantiated yet

        // Calculate initial positions in Z and X axis
        initialPos = tileSize/2 * (townSize-1);
        currentZpos = initialPos;
        currentXpos = -initialPos; // from upper left corner

        // Calculate positions
        for (int column = 0; column < townSize; column++){
            for (int row = 0; row < townSize; row++){
                currentPosition = new Vector3(currentXpos, 0f, currentZpos);

                //tilesPositions[column].Add(currentPosition);

                InstantiateTile(currentPosition);

                currentZpos -= tileSize;
            }
            currentZpos = initialPos;
            currentXpos += tileSize;
        }
    }

    /// <summary>
    /// Instantiate town tile in given position
    /// </summary>
    void InstantiateTile(Vector3 position){
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
            Instantiate(currentTile, position, Quaternion.Euler(0, randomRotation, 0), transform);
        }
    }
}