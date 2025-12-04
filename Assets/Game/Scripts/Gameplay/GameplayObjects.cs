using UnityEngine;
using UnityEngine.UI;

public class GameplayObjects : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateStep = 90f;
    public float rotateSpeed = 6f;
    public float minY = -90f;
    public float maxY = 90f;

    [HideInInspector] public float targetY;

    StageManager stageManager;

    public void Init(StageManager inStageManager)
    {
        stageManager = inStageManager;

        targetY = 0;

        targetY = NormalizeAngle(transform.localEulerAngles.y);
        targetY = Mathf.Clamp(targetY, minY, maxY);
        SetRotation(targetY);
    }

    public void DoUpdate(float dt)
    {
        float currentY = NormalizeAngle(transform.localEulerAngles.y);
        float newY = Mathf.Lerp(currentY, targetY, dt * rotateSpeed);
        transform.localRotation = Quaternion.Euler(0f, newY, 0f);
    }

    public void RotateLeft()
    {
        if (targetY <= minY) return;

        targetY -= rotateStep;
        targetY = Mathf.Clamp(targetY, minY, maxY);
    }

    public void RotateRight()
    {
        if (targetY >= maxY) return;

        targetY += rotateStep;
        targetY = Mathf.Clamp(targetY, minY, maxY);
    }

    void SetRotation(float y)
    {
        transform.localRotation = Quaternion.Euler(0f, y, 0f);
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
