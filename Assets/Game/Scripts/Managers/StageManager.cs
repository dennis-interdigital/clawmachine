using System;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Config")]
    public float playTimer;

    public FloatingJoystick joystick;
    public GameplayObjects gameplayObjects;
    public ClawMachine clawMachine;
    public PrizeFactory prizeFactory;

    public GameObject objWallresult;
    public GameObject[] objPrizeResults;

    public float tPlayTimer;
    bool hasStartGrabSequence;
    [HideInInspector] public bool isRunningTimer;

    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;
        userData = gameManager.userData;

        joystick.gameObject.SetActive(false);

        int count = (int)PrizeRarity.COUNT;
        if (userData.probabilityDatas == null)
        {
            userData.probabilityDatas = new List<List<bool>>();
            
            for (int i = 0; i < count; i++)
            {
                List<bool> data = new List<bool>();
                userData.probabilityDatas.Add(data);
                GenerateProbabilityDatas((PrizeRarity)i);
            }
        }

        string json = JsonUtility.ToJson(userData.probabilityDatas);
        Debug.Log(json);

        if (userData.prizeRecordDatas == null)
        {
            userData.prizeRecordDatas = new List<PrizeRecordData>();
        }

        RectTransform rtJoystick = joystick.GetComponent<RectTransform>();
        rtJoystick.sizeDelta = new Vector2(Screen.width, Screen.height / 2);

        prizeFactory.Init(gameManager);
        clawMachine.Init(gameManager);
    }

    public void ResetStage()
    {
        isRunningTimer = true;
        hasStartGrabSequence = false;
        tPlayTimer = playTimer;
    }

    public void DoUpdate(float dt)
    {
        gameplayObjects.DoUpdate(dt);
        clawMachine.DoUpdate(dt);

        if (isRunningTimer)
        {
            if (!hasStartGrabSequence)
            {
                if (tPlayTimer <= 0)
                {
                    GameMenuUI gameMenuUI = gameManager.uiManager.currentActiveUI as GameMenuUI;
                    if (gameMenuUI != null)
                    {
                        gameMenuUI.SetButtonEnable(false);
                    }
                    StartGrabSequence();
                }
                else
                {
                    tPlayTimer -= dt;
                }
            }
        }
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

    public void GenerateProbabilityDatas(PrizeRarity rarity)
    {
        int rarityIndex = (int)rarity;
        int successRate =  prizeFactory.prizeSO.successRates[rarityIndex];

        List<bool> probabilityDatas = userData.probabilityDatas[rarityIndex];

        
        for (int i = 0; i < 100; i++)
        {
            bool success = i < successRate;
            userData.probabilityDatas[rarityIndex].Add(success);
            
        }

        userData.probabilityDatas[rarityIndex].Shuffle();

        List<bool> successList = userData.probabilityDatas[rarityIndex];
        string log = $"{rarity}: ";
        int count = successList.Count;
        for (int i = 0; i < count; i++)
        {
            log += $"{successList[i]},";
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

    public void StartGrabSequence()
    {
        clawMachine.StartGrabSequence();
        hasStartGrabSequence = true;
        isRunningTimer = false;
        Debug.Log("START GRAB SEUQNCE");
    }
}
