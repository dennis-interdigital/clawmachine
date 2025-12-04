using UnityEngine;
using UnityEngine.UI;

public class ButtonRotateObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotateStep = 90f;
    public float rotateSpeed = 6f;
    public float minY = -90f;
    public float maxY = 90f;

    [Header("UI Buttons")]
    public Button leftButton;
    public Button rightButton;

    private float targetY;

    void Start()
    {
        targetY = NormalizeAngle(transform.localEulerAngles.y);
        targetY = Mathf.Clamp(targetY, minY, maxY);
        SetRotationInstant(targetY);

        leftButton.onClick.AddListener(RotateLeft);
        rightButton.onClick.AddListener(RotateRight);

        UpdateButtonState();
    }

    void Update()
    {
        float currentY = NormalizeAngle(transform.localEulerAngles.y);
        float newY = Mathf.Lerp(currentY, targetY, Time.deltaTime * rotateSpeed);
        transform.localRotation = Quaternion.Euler(0f, newY, 0f);
    }

    public void RotateLeft()
    {
        if (targetY <= minY) return;

        targetY -= rotateStep;
        targetY = Mathf.Clamp(targetY, minY, maxY);
        UpdateButtonState();
    }

    public void RotateRight()
    {
        if (targetY >= maxY) return;

        targetY += rotateStep;
        targetY = Mathf.Clamp(targetY, minY, maxY);
        UpdateButtonState();
    }

    void UpdateButtonState()
    {
        leftButton.interactable = targetY > minY;
        rightButton.interactable = targetY < maxY;
    }

    void SetRotationInstant(float y)
    {
        transform.localRotation = Quaternion.Euler(0f, y, 0f);
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}
