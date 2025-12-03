using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PrizeRecordData
{
    public string prizeId;
    public bool status;
    public long time;
}

public class StageManager : MonoBehaviour
{
    GameManager gameManager;
    UserData userData;

    public FloatingJoystick joystick;
    [SerializeField] ClawMachine clawMachine;
    public PrizeFactory prizeFactory;

    [SerializeField] Button buttonGrab;

    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;
        userData = gameManager.userData;

        if(userData.prizeRecordDatas == null)
        {
            userData.prizeRecordDatas = new System.Collections.Generic.List<PrizeRecordData>();
        }

        RectTransform rtJoystick = joystick.GetComponent<RectTransform>();
        rtJoystick.sizeDelta = new Vector2(Screen.width / 2, Screen.height / 2);

        prizeFactory.Init(this);
        clawMachine.Init(this);

        buttonGrab.onClick.AddListener(clawMachine.OnClickGrab);
    }

    public void DoUpdate()
    {
        clawMachine.DoUpdate();
    }

    public void SaveRecord(string prizeId, bool status)
    {
        PrizeRecordData data = new PrizeRecordData();

        DateTime dateTimeNow = DateTime.Now;
        DateTime universalNow = dateTimeNow.ToUniversalTime();

        long now = ((DateTimeOffset)dateTimeNow).ToUnixTimeSeconds();

        data.prizeId = prizeId;
        data.status = status;
        data.time = now;

        int count = userData.prizeRecordDatas.Count;
        if (count > 100) //TODO config
        {
            userData.prizeRecordDatas.RemoveAt(0);
        }
        userData.prizeRecordDatas.Add(data);

        string resultLog = status ? "Success" : "Failed";

        string logString = $"ClawMachine Result: {resultLog}\n" +
            $"Id: {prizeId}\n" +
            $"Time: {dateTimeNow}";
    }
}
