using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestSO))]
public class SOButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestSO testTarget = (TestSO)target;

        if(GUILayout.Button("test click"))
        {
            testTarget.TestButtonClick();
        }
    }
}
