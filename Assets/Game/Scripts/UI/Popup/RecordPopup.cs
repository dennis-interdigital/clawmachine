using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordPopup : BaseUI
{
    [SerializeField] RectTransform rootItem;
    [SerializeField] RecordItemUI prefabRecordItem;
    [SerializeField] Button buttonClose;

    List<RecordItemUI> recordItemUIs;

    UserData userData;

    public override void Init(GameManager inGameManager)
    {
        base.Init(inGameManager);
        userData = gameManager.userData;

        recordItemUIs = new List<RecordItemUI>();

        buttonClose.onClick.AddListener(OnClickClose);
    }

    public override void Show(params object[] payload)
    {
        int count = recordItemUIs.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(recordItemUIs[i].gameObject);
        }
        recordItemUIs.Clear();

        count = userData.prizeRecordDatas.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            PrizeRecordData data = userData.prizeRecordDatas[i];

            RecordItemUI item = Instantiate(prefabRecordItem, rootItem);
            item.SetItem(gameManager, data, i);

            recordItemUIs.Add(item);
        }

        base.Show(payload);
    }

    void OnClickClose()
    {
        uiManager.HidePopup(PopupState.Record);
    }
}
