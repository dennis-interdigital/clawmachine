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

    void Start()
    {
        Vector3 startPos = new Vector3(minPos.x, rootClawBase.position.y, minPos.z);
        SetPos(startPos);
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDirection += Vector3.forward;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            moveDirection -= Vector3.forward;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection -= Vector3.forward;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            moveDirection += Vector3.forward;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection -= Vector3.right;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            moveDirection += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            moveDirection -= Vector3.right;
        }

        Vector3 pos = rootClawBase.position;

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
