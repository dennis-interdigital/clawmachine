using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    [SerializeField] Button buttonInsertCoin;
    public override void Init(GameManager inGameManager)
    {
        base.Init(inGameManager);

        buttonInsertCoin.onClick.AddListener(OnClickInsertCoin);
    }

    void OnClickInsertCoin()
    {
        uiManager.overlayMenuUI.SetBottomMenu(false);
        uiManager.ShowUI(UIState.GameMenu);
    }
}
