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
    Result
}

[Serializable]
public class UIClass
{
    public UIState state;
    public BaseUI baseUI;
}

[Serializable]
public class PopupClass
{
    public PopupState state;
    public BaseUI baseUI;
}
public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject objBlackUI;
    public OverlayMenuUI overlayMenuUI;
    public List<UIClass> uiList;
    public List<PopupClass> popupList;
    
    public UIState currentUI;
    public PopupState currentPopup;

    public BaseUI currentActiveUI;
    public BaseUI currentActivePopup;

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
        currentActiveUI = toActive.baseUI;
        currentUI = state;
    }

    public void ShowPopup(PopupState state, params object[] payload)
    {

        PopupClass popup = GetPopup(state);
        if(popup != null)
        {
            popup.baseUI.Show(payload);
            currentActivePopup = popup.baseUI;
            currentPopup = state;
        }
        else
        {
            Debug.LogError($"POPUP {state} DOES NOT EXIST!");
        }
    }

    public void HidePopup(PopupState state, params object[] payload)
    {
        currentActivePopup.Hide(payload);
        currentActivePopup = null;
        currentPopup = PopupState.None;
    }

    PopupClass GetPopup(PopupState state)
    {
        PopupClass result = null;

        int count = popupList.Count;
        for (int i = 0; i < count; i++)
        {
            PopupClass popup = popupList[i];
            if (popup.state.Equals(state))
            {
                result = popup;
            }
        }

        return result;
    }
}
