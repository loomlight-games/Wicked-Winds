using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGenerator : MonoBehaviour
{
    // Tile size
    public float tileSize = 50f;

    // Town size in tiles
    public float townSize = 5f;

    // List of 3D positions where to spawn town tiles
    //public List<List<Vector3>> tilesPositions = new ();
    
    // List of town tiles
    public List<GameObject> townTiles = new();

    public Dictionary<TownTile.Type, bool> isTypeInstantiated = new(){
        {TownTile.Type.Residential, false},
        {TownTile.Type.Forest,false},
        {TownTile.Type.Park,false},
        {TownTile.Type.Market,false},
        {TownTile.Type.Swamp,false}
    };

    // Start is called before the first frame update
    void Start()
    {
        float currentXpos = -tileSize/2 * (townSize-1);
        float initialZpos = tileSize/2 * (townSize-1);
        float currentZpos = initialZpos;
        Vector3 position;

        // Calculate positions - from upper left corner
        for (int column = 0; column < townSize; column++){
            for (int row = 0; row < townSize; row++){
                position = new (currentXpos, 0f, currentZpos);

                //tilesPositions[column].Add(position);

                InstantiateTile(position);

                currentZpos -= tileSize;
            }
            currentZpos = initialZpos;
            currentXpos += tileSize;
        }
    }

    /// <summary>
    /// Instantiate town tile in given position
    /// </summary>
    void InstantiateTile(Vector3 position){
        GameObject townTile = CheckTile();

        // Random rotation - 0,90,180,270
        float randomRotation = Random.Range(0, 4) * 90f; 

        // Instantiate tile in position with random rotation on Y
        Instantiate(townTile, position, Quaternion.Euler(0, randomRotation, 0), transform);
    }

    GameObject CheckTile(){
        // Random tile
        int randomIndex = Random.Range(0, townTiles.Count);
        GameObject townTile = townTiles[randomIndex];
        TownTile tileData = townTile.GetComponent<TownTile>();
        bool isInstantiated = isTypeInstantiated[tileData.type];

        // Check if it's unique but has been already instantiated
        if (tileData.isUnique && isInstantiated)
            townTile = CheckTile();
        
        isTypeInstantiated[tileData.type] = true;

        return townTile;
    }
}