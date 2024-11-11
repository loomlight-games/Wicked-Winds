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
    public List<Vector3> tilesPositions = new ();
    
    // List of town tiles
    public List<GameObject> townTiles = new();

    // Start is called before the first frame update
    void Start()
    {
        float currentXpos = -tileSize/2 * (townSize-1);
        float initialZpos = tileSize/2 * (townSize-1);
        float currentZpos = initialZpos;

        // Calculate positions - list size = townSize*townSize
        for (int i = 0; i < townSize; i++){
            for (int j = 0; j < townSize; j++){
                tilesPositions.Add (new (currentXpos, 0f, currentZpos));
                currentZpos -= tileSize;
            }
            currentZpos = initialZpos;
            currentXpos += tileSize;
        }

        // Instantiate a tile in each position
        foreach(Vector3 position in tilesPositions){
            // Random tile
            int randomIndex = Random.Range(0, townTiles.Count);
            GameObject townTile = townTiles[randomIndex];

            // Random rotatio - 0,90,180,270
            float randomRotation = Random.Range(0, 4) * 90f; 

            // Instantiate tile with rotation
            Instantiate(townTile, position, Quaternion.Euler(0, randomRotation, 0), transform);
        }
    }
}