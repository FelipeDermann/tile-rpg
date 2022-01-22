using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileDefinitions", menuName = "ScriptableObjects/TileDefinitions", order = 1)]
public class ScriptableTileDefinitions : ScriptableObject
{
    public GameObject allyHexBorder;
    public GameObject enemyHexBorder;
    public GameObject neutralHexBorder;
}