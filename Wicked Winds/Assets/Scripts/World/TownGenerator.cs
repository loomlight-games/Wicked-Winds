using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGenerator : MonoBehaviour
{
    // Tile size
    public float tileSize = 50f;

    // Town size in tiles
    public float townSize = 5f;
    float townSquareSize;

    // List of 3D positions where to spawn town tiles
    public List<Vector3> tilesPositions = new ();
    
    // List of town tiles
    public List<TownTile> townTiles = new();

    

    // Start is called before the first frame update
    void Start()
    {
        townSquareSize = townSize*townSize;

        float currentXpos = -tileSize*2;
        float currentZpos = tileSize*2;

        // Fill positions list - size (townSize*townSize)
        for (int i = 0; i < townSize; i++){
            for (int j = 0; j < townSize; j++){
                tilesPositions.Add (new Vector3 (currentXpos, 0f, currentZpos));
                currentZpos -= tileSize;
            }
            currentZpos = tileSize*2;
            currentXpos += tileSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/// First
///     Find every town tile with addressables
///     Fill every index in matrix of positions with a random* town tile

