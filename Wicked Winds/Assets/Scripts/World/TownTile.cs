using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownTile : MonoBehaviour
{
    public TownGenerator.TileType type = TownGenerator.TileType.Residential;
    public TownGenerator.MapTheme theme = TownGenerator.MapTheme.Summer;
    public bool isUnique = false;
}