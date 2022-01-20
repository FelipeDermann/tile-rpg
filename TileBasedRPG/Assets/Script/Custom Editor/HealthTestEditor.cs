using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Unit))]
public class HealthTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Unit unit = (Unit)target;
        if (GUILayout.Button("Change Health"))
        {
            unit.HealthTest();
        }
    }
}
