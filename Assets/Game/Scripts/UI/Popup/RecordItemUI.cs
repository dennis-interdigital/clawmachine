using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordItemUI : MonoBehaviour
{
    [SerializeField] Image imgBG;
    [SerializeField] TextMeshProUGUI textNo;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textDateTime;
    [SerializeField] Color colorSuccess;
    [SerializeField] Color colorFailed;

    int index;

    public void SetItem(GameManager inGameManager, PrizeRecordData inData, int inIndex)
    {
        PrizeFactory prizeFactory = inGameManager.stageManager.prizeFactory;

        index = inIndex;

        string id = inData.prizeId;
        long time = inData.time;
        bool status = inData.status;

        PrizeData prizeData = prizeFactory.prizeSO.GetData(id);
        string name = prizeData.name;

        DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(time).DateTime;

        int no = index + 1;

        textNo.SetText(no.ToString());
        textName.SetText(name);

        string timeString = $"{dateTime.Day}/{dateTime.Month}/{dateTime.Year}\n{dateTime.Hour}:{dateTime.Minute}";

        Color bgColor = status ? colorSuccess : colorFailed;
        imgBG.color = bgColor;
    }
}
