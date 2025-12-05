using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : BaseUI
{
    public FloatingJoystick joystick;
    [SerializeField] Button buttonGrab;
    [SerializeField] Button buttonRotateLeft;
    [SerializeField] Button buttonRotateRight;
    [SerializeField] CanvasGroup cg;

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

    public void RefreshUI()
    {
        buttonRotateLeft.interactable = gameplayObjects.targetY > gameplayObjects.minY;
        buttonRotateRight.interactable = gameplayObjects.targetY < gameplayObjects.maxY;
    }

    public void SetButtonEnable(bool enable)
    {
        cg.interactable = enable;
        joystick.gameObject.SetActive(enable);
        (enable ? (System.Action)PlayEnableAnim : PlayDisableAnim)();
    }

    void PlayEnableAnim()
    {
        cg.DOKill();
        transform.DOKill();

        cg.alpha = 0f;

        Vector3 startPos = transform.localPosition;
        transform.localPosition = new Vector3(startPos.x, startPos.y-300f, startPos.z);

        transform.DOLocalMoveY(-670, 0.3f).SetEase(Ease.OutBack);
        cg.DOFade(1f, 0.25f);
    }

    void PlayDisableAnim()
    {
        cg.DOKill();
        transform.DOKill();

        transform.DOLocalMoveY(-700, 0.25f).SetEase(Ease.InBack);
        cg.DOFade(0f, 0.2f);
    }


    void OnClickGrab()
    {
        SetButtonEnable(false);
        stageManager.OnClickGrab();
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
