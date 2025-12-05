using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultPopup : BaseUI
{
    [SerializeField] GameObject rootFailed;
    [SerializeField] GameObject rootSuccess;
    [SerializeField] CanvasGroup cgFailed;
    [SerializeField] CanvasGroup cgSuccess;
    [SerializeField] TextMeshProUGUI textPrizeName;
    [SerializeField] TextMeshProUGUI textPrizeRarity;
    [SerializeField] Button buttonClose;
    [SerializeField] TextMeshProUGUI textButtonClose;

    StageManager stageManager;

    public override void Init(GameManager inGameManager)
    {
        base.Init(inGameManager);
        stageManager = gameManager.stageManager;

        buttonClose.onClick.AddListener(OnClickClose);
    }

    public override void Show(params object[] payload)
    {
        bool success = (bool)payload[0];
        PrizeData data = payload[1] as PrizeData;
        int index = (int)payload[2];

        rootSuccess.SetActive(success);
        rootFailed.SetActive(!success);

        PlayShowAnim(success);

        string buttonCloseString = success ? "OK" : "OK";
        textButtonClose.SetText(buttonCloseString);

        if (success)
        {
            textPrizeName.SetText(data.name);
            textPrizeRarity.SetText(data.rarity.ToString());
            stageManager.SetPrizeResult(true, index);
        }

        base.Show();
    }

    public override void Hide(params object[] payload)
    {
        PlayHideAnim(rootSuccess.activeSelf);
        stageManager.SetPrizeResult(false);
        base.Hide(payload);
    }

    void PlayShowAnim(bool success)
    {
        CanvasGroup cg = success ? cgSuccess : cgFailed;
        Transform root = success ? rootSuccess.transform : rootFailed.transform;

        cg.DOKill();
        root.DOKill();

        cg.alpha = 0f;
        root.localScale = Vector3.zero;

        cg.DOFade(1f, 0.25f);
        root.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }

    void PlayHideAnim(bool success)
    {
        CanvasGroup cg = success ? cgSuccess : cgFailed;
        Transform root = success ? rootSuccess.transform : rootFailed.transform;

        cg.DOKill();
        root.DOKill();

        cg.DOFade(0f, 0.2f);
        root.DOScale(0f, 0.25f).SetEase(Ease.InBack);
    }

    void OnClickClose()
    {
        uiManager.HidePopup(PopupState.Result);
        uiManager.ShowUI(UIState.MainMenu);
    }
}
