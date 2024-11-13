using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownTile : MonoBehaviour
{
    public enum Type{
        Residential, Forest, Park, Market, Swamp
    }
    public Type type;
    public bool isUnique, 
        isCompulsory;
        //isInstantiated = false;
}