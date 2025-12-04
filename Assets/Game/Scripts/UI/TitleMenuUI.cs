using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenuUI : BaseUI
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Button buttonStart;

    public override void Init(GameManager inGameManager)
    {
        base.Init(inGameManager);

        buttonStart.onClick.AddListener(OnClickStart);
    }

    void OnClickStart()
    {
        uiManager.ShowUI(UIState.MainMenu);
        uiManager.overlayMenuUI.SetTopMenu(true);
        uiManager.overlayMenuUI.SetBottomMenu(true);
    }
}
