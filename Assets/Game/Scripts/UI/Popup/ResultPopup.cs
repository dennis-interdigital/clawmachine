using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopup : BaseUI
{
    [SerializeField] GameObject rootFailed;
    [SerializeField] GameObject rootSuccess;
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

        string buttonCloseString = success ? "YAY!" : "okay";
        textButtonClose.SetText(buttonCloseString);

        if (success)
        {
            string name = data.name;
            PrizeRarity rarity = data.rarity;

            textPrizeName.SetText(name);
            textPrizeRarity.SetText(rarity.ToString());

            stageManager.SetPrizeResult(true, index);
        }

        base.Show();
    }

    public override void Hide(params object[] payload)
    {
        stageManager.SetPrizeResult(false);
        base.Hide(payload);
    }

    void OnClickClose()
    {
        uiManager.HidePopup(PopupState.Result);
        uiManager.ShowUI(UIState.MainMenu);
        stageManager.prizeFactory.CheckSpawnPrize();
    }

}
