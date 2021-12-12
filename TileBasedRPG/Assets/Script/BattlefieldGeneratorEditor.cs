using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileManager))]
public class BattlefieldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileManager tileManager = (TileManager)target;
        if (GUILayout.Button("Create Battlefield"))
        {
            tileManager.BattlefieldGeneratorEditor();
        }
    }
}
