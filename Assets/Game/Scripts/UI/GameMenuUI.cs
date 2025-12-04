using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : BaseUI
{
    [SerializeField] Button buttonGrab;
    [SerializeField] Button buttonRotateLeft;
    [SerializeField] Button buttonRotateRight;

    StageManager stageManager;
    GameplayObjects gameplayObjects;

    public override void Init(GameManager inGameManager)
    {
        base.Init(inGameManager);
        stageManager = gameManager.stageManager;
        gameplayObjects = stageManager.gameplayObjects;

        buttonGrab.onClick.AddListener(OnClickGrab);
        buttonRotateLeft.onClick.AddListener(OnClickRotateLeft);
        buttonRotateRight.onClick.AddListener(OnClickRotateRight);

        RefreshUI();
    }

    public void RefreshUI()
    {
        buttonRotateLeft.interactable = gameplayObjects.targetY > gameplayObjects.minY;
        buttonRotateRight.interactable = gameplayObjects.targetY < gameplayObjects.maxY;
    }

    void OnClickGrab()
    {
        stageManager.OnClickGrab();
    }

    void OnClickRotateLeft()
    {
        gameplayObjects.RotateLeft();
        RefreshUI();
    }

    void OnClickRotateRight()
    {
        gameplayObjects.RotateRight();
        RefreshUI();
    }
}
