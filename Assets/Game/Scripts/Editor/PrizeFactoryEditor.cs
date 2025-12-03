using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PrizeFactory))]
public class PrizeFactoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PrizeFactory prizeFactory = (PrizeFactory)target;

        if (GUILayout.Button("Spawn"))
        {
            prizeFactory.SpawnPrize(prizeFactory.editorSpawnAmount);
        }
    }
}
