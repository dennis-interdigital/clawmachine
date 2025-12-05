using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    [SerializeField] Button buttonInsertCoin;

    StageManager stageManager;

    public override void Init(GameManager inGameManager)
    {
        base.Init(inGameManager);
        stageManager = gameManager.stageManager;

        buttonInsertCoin.onClick.AddListener(OnClickInsertCoin);
    }

    public override void Show(params object[] payload)
    {
        uiManager.overlayMenuUI.SetBottomMenu(true);
        base.Show(payload);
    }

    public override void Hide(params object[] payload)
    {
        uiManager.overlayMenuUI.SetBottomMenu(false);
        base.Hide(payload);
    }

    void OnClickInsertCoin()
    {
        uiManager.ShowUI(UIState.GameMenu);
        stageManager.ResetStage();
    }
}
