using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileDefinitions", menuName = "ScriptableObjects/TileDefinitions", order = 1)]
public class ScriptTileDefinitions : ScriptableObject
{
    public Color allyTileColor;
    public Color enemyTileColor;
    public Color neutralTileColor;
}