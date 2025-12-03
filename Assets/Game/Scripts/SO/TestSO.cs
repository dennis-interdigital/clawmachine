using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test_", menuName = "SO/test")]
public class TestSO : ScriptableObject
{
    public void TestButtonClick()
    {
        Debug.Log("CLICKED");
    }
}
