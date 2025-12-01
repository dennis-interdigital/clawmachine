using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClawMachine : MonoBehaviour
{
    [SerializeField] Transform rootXBar;
    [SerializeField] Transform rootZBar;
    [SerializeField] Transform rootClawBase;
    [SerializeField] Transform rootClawHand;
    [SerializeField] Transform[] rootClawFingers;

    [Header("Config")]
    [SerializeField] Vector3 minPos;
    [SerializeField] Vector3 maxPos;
    [SerializeField] float moveSpeed;
    Vector3 moveDirection;

    StageManager stageManager;
    FloatingJoystick joystick;

    public void Init(StageManager inStageManager)
    {
        stageManager = inStageManager;
        joystick = stageManager.joystick;

        Vector3 startPos = new Vector3(minPos.x, rootClawBase.position.y, minPos.z);
        SetPos(startPos);
    }

    public void DoUpdate()
    {
        float dt = Time.deltaTime;

        Vector3 pos = rootClawBase.position;

        moveDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);

        pos += moveDirection * moveSpeed * dt;

        pos.x = Mathf.Clamp(pos.x, minPos.x, maxPos.x);
        pos.z = Mathf.Clamp(pos.z, minPos.z, maxPos.z);

        SetPos(pos);
    }

    void SetPos(Vector3 pos)
    {
        rootClawBase.position = pos;
        rootXBar.position = new Vector3(pos.x, rootXBar.position.y, rootXBar.position.z);
        rootZBar.position = new Vector3(rootZBar.position.x, rootZBar.position.y, pos.z);
    }
}
