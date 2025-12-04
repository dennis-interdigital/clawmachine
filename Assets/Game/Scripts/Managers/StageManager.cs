using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public GameplayObjects gameplayObjects;
    public ClawMachine clawMachine;
    public PrizeFactory prizeFactory;

    public GameObject objWallresult;
    public GameObject[] objPrizeResults;

    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;
        userData = gameManager.userData;

        joystick.gameObject.SetActive(false);

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
    }

    public void DoUpdate(float dt)
    {
        gameplayObjects.DoUpdate(dt);
        clawMachine.DoUpdate(dt);
    }

    public void SaveRecord(PrizeData prizeData, int prizeIndex, bool status)
    {
        PrizeRecordData prizeRecordData = new PrizeRecordData();

        DateTime dateTimeNow = DateTime.Now;
        DateTime universalNow = dateTimeNow.ToUniversalTime();

        long now = ((DateTimeOffset)dateTimeNow).ToUnixTimeSeconds();

        if (prizeData != null) prizeRecordData.prizeId = prizeData.id;
        prizeRecordData.status = status;
        prizeRecordData.time = now;

        int count = userData.prizeRecordDatas.Count;
        if (count > 100) //TODO config
        {
            userData.prizeRecordDatas.RemoveAt(0);
        }
        userData.prizeRecordDatas.Add(prizeRecordData);

        string resultLog = status ? "Success" : "Failed";

        gameManager.uiManager.ShowPopup(PopupState.Result, status, prizeData, prizeIndex);

        string id = prizeData != null ? prizeData.id : "-";

        string logString = $"ClawMachine Result: {resultLog}\n" +
            $"Id: {id}\n" +
            $"Time: {dateTimeNow}";

        Debug.Log(logString);
    }

    void GenerateProbabilityDatas()
    {
        string log = string.Empty;

        for (int i = 0; i < 100; i++)
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

    public void SetPrizeResult(bool active, int index = 0)
    {
        foreach (GameObject obj in objPrizeResults)
        {
            obj.SetActive(false);
        }

        objWallresult.SetActive(active);
        if (active)
        {
            objPrizeResults[index].SetActive(true);
        }
    }

    public void OnClickGrab()
    {
        if (userData.probabilityDatas.Count == 0)
        {
            GenerateProbabilityDatas();
        }

        bool success = userData.probabilityDatas[0];
        userData.probabilityDatas.RemoveAt(0);

        clawMachine.StartGrabSequence(success);

        Debug.Log($"START GRAB SEUQNCE: {success}");
    }
}
