using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownTile : MonoBehaviour
{
    public TownGenerator.TileType type = TownGenerator.TileType.Residential;
    public TownGenerator.Town theme = TownGenerator.Town.Summer;
    public bool isUnique = false;
}