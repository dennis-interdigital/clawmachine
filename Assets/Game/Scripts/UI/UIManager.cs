using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum UIState
{
    TitleMenu,
    MainMenu,
    GameMenu,
    ShopMenu,
    InventoryMenu
}

public enum PopupState
{
    None,
    General,
    PrizeResult
}

[Serializable]
public class UIClass
{
    public UIState state;
    public BaseUI baseUI;
}

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject objBlackUI;
    public OverlayMenuUI overlayMenuUI;
    public List<UIClass> uiList;
    public List<UIClass> popupList;
    
    public UIState currentUI;
    public PopupState currentPopup;

    GameManager gameManager;

    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;

        overlayMenuUI.Init(gameManager);

        int count = uiList.Count;
        for (int i = 0; i < count; i++)
        {
            uiList[i].baseUI.Init(gameManager);
        }

        count = popupList.Count;
        for(int i = 0;i < count; i++)
        {
            popupList[i].baseUI.Init(gameManager);
        }
    }

    public void DoUpdate(float dt)
    {
        int count = uiList.Count;
        for (int i = 0; i < count; i++)
        {
            if (uiList[i].baseUI.gameObject.activeSelf)
            {
                uiList[i].baseUI.DoUpdate(dt);
            }
        }
    }

    public void ShowUI(UIState state)
    {
        UIClass toActive = null;
        int count = uiList.Count;
        foreach (UIClass ui in uiList)
        {
            ui.baseUI.Hide();

            if (ui.state == state)
            {
                toActive = ui;
            }
        }

        toActive.baseUI.Show();
    }

    public void ShowPopup(PopupState state, params object[] payload)
    {

    }

    public void HidePopup(PopupState state, params object[] payload)
    {

    }

    IEnumerator ShowingPopup(PopupState state, params object[] payload)
    {
        yield return null;
    }

    IEnumerator HidingPopup(PopupState state, params object[] payload)
    {
        yield return null;
    }

}
