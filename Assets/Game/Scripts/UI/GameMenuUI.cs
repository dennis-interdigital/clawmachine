using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : BaseUI
{
    [SerializeField] TextMeshProUGUI textTimer;
    public FloatingJoystick joystick;
    [SerializeField] Button buttonGrab;
    [SerializeField] Button buttonRotateLeft;
    [SerializeField] Button buttonRotateRight;

    StageManager stageManager;
    GameplayObjects gameplayObjects;

    public override void Init(GameManager inGameManager)
    {
        base.Init(inGameManager);
        stageManager = gameManager.stageManager;
        gameplayObjects = stageManager.gameplayObjects;

        buttonGrab.onClick.AddListener(OnClickGrab);
        buttonRotateLeft.onClick.AddListener(OnClickRotateLeft);
        buttonRotateRight.onClick.AddListener(OnClickRotateRight);

    }

    public override void Show(params object[] payload)
    {
        SetButtonEnable(true);
        RefreshUI();

        base.Show(payload);
    }

    public override void DoUpdate(float dt)
    {
        int timerInt = (int)stageManager.tPlayTimer + 1;
        string timerString = timerInt.ToString("00");
        textTimer.SetText(timerString);
    }

    public void RefreshUI()
    {
        buttonRotateLeft.interactable = gameplayObjects.targetY > gameplayObjects.minY;
        buttonRotateRight.interactable = gameplayObjects.targetY < gameplayObjects.maxY;
    }

    public void SetButtonEnable(bool enable)
    {
        buttonGrab.enabled = enable;
        buttonRotateLeft.enabled = enable;
        buttonRotateRight.enabled = enable;
        joystick.gameObject.SetActive(enable);
    }

    void OnClickGrab()
    {
        SetButtonEnable(false);
        stageManager.StartGrabSequence();
    }

    void OnClickRotateLeft()
    {
        gameplayObjects.RotateLeft();
        RefreshUI();
    }

    void OnClickRotateRight()
    {
        gameplayObjects.RotateRight();
        RefreshUI();
    }
}
