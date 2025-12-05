using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenuUI : BaseUI
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Button buttonStart;

    [SerializeField] float fadeDuration = 0.4f;
    [SerializeField] float scaleUpAmount = 1.1f;

    bool isPlaying;

    public override void Init(GameManager inGameManager)
    {
        base.Init(inGameManager);
        buttonStart.onClick.AddListener(OnClickStart);
    }

    void OnClickStart()
    {
        if (isPlaying) return;
        isPlaying = true;

        buttonStart.interactable = false;

        Sequence seq = DOTween.Sequence();

        seq.Join(canvasGroup.DOFade(0f, fadeDuration));
        seq.Join(this.transform.DOScale(scaleUpAmount, fadeDuration).SetEase(Ease.OutBack));

        seq.OnComplete(() =>
        {
            uiManager.ShowUI(UIState.MainMenu);
            uiManager.overlayMenuUI.SetTopMenu(true);
        });
    }
}
