using System;
using System.Collections.Generic;
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

    [Header("Probability Config")]
    public int successRate;

    public FloatingJoystick joystick;
    [SerializeField] ClawMachine clawMachine;
    public PrizeFactory prizeFactory;

    [SerializeField] Button buttonGrab;

    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;
        userData = gameManager.userData;

        if(userData.probabilityDatas == null)
        {
            userData.probabilityDatas = new List<bool>();
        }
        GenerateProbabilityDatas();

        if (userData.prizeRecordDatas == null)
        {
            userData.prizeRecordDatas = new List<PrizeRecordData>();
        }

        RectTransform rtJoystick = joystick.GetComponent<RectTransform>();
        rtJoystick.sizeDelta = new Vector2(Screen.width, Screen.height / 2);

        prizeFactory.Init(gameManager);
        clawMachine.Init(this);

        buttonGrab.onClick.AddListener(OnClickGrab);
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

        Debug.Log(logString);
    }

    void GenerateProbabilityDatas()
    {
        string log = string.Empty;

        for (int i = 0;i < 100; i++)
        {
            bool success = i < successRate;
            userData.probabilityDatas.Add(success);
        }

        userData.probabilityDatas.Shuffle();

        int count = userData.probabilityDatas.Count;
        for (int i = 0; i < count; i++)
        {
            log += $"{userData.probabilityDatas[i]},";
        }
        Debug.Log(log);
    }

    void OnClickGrab()
    {
        if(userData.probabilityDatas.Count == 0)
        {
            GenerateProbabilityDatas();
        }

        bool success = userData.probabilityDatas[0];
        userData.probabilityDatas.RemoveAt(0);

        clawMachine.StartGrabSequence(success);

        Debug.Log($"START GRAB SEUQNCE: {success}");
    }
}
