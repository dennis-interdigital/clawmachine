using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    public FloatingJoystick joystick;
    [SerializeField] ClawMachine clawMachine;

    void Start()
    {
        clawMachine.Init(this);
    }

    void Update()
    {
        clawMachine.DoUpdate();
    }
}
