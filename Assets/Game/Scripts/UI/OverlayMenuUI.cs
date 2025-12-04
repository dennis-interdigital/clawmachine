using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverlayMenuUI : MonoBehaviour
{
    [SerializeField] RectTransform rootTopMenu;
    [SerializeField] TextMeshProUGUI textCoin;
    [SerializeField] TextMeshProUGUI textDiamond;

    [SerializeField] Button buttonAddCoin;
    [SerializeField] Button buttonAddDiamond;

    [Space(10f)]
    [SerializeField] RectTransform rootBottomMenu;
    [SerializeField] Button buttonShop;
    [SerializeField] Button buttonMain;
    [SerializeField] Button buttonInventory;

    GameManager gameManager;
    UserData userData;

    Tween tweenBottomMenu;
    Tween tweenTopMenu;

    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;
        userData = gameManager.userData;

        buttonAddCoin.onClick.AddListener(OnClickAddCoin);
        buttonAddDiamond.onClick.AddListener(OnClickAddDiamond);
    
        buttonShop.onClick.AddListener(OnClickShop);
        buttonMain.onClick.AddListener(OnClickMain);
        buttonInventory.onClick.AddListener(OnClickInventory);

        RefreshUI();
    }

    public void RefreshUI()
    {
        int coin = userData.coin;
        int diamond = userData.diamond;

        textCoin.SetText(coin.ToString());
        textDiamond.SetText(diamond.ToString());
    }

    public void SetTopMenu(bool active)
    {
        if(tweenTopMenu != null)
        {
            tweenTopMenu.Kill();
        }

        float yTarget = active ? 0 : 300;
        tweenTopMenu = rootTopMenu.DOAnchorPosY(yTarget, 0.5f, true);
    }

    public void SetBottomMenu(bool active)
    {
        if(tweenBottomMenu != null)
        {
            tweenBottomMenu.Kill();
        }

        float ytarget = active ? 0 : -300;
        tweenBottomMenu = rootBottomMenu.DOAnchorPosY(ytarget, 0.5f, true);
    }

    void SetBottomButtons(int value)
    {
        //1 = shop
        //2 = main
        //3 = inventory
        buttonShop.interactable = value != 1;
        buttonMain.interactable = value != 2;
        buttonInventory.interactable = value != 3;
    }

    void OnClickAddCoin()
    {
        //go to shop
        SetBottomButtons(1);
    }

    void OnClickAddDiamond()
    {
        //go to shop
        SetBottomButtons(1);

    }

    void OnClickShop()
    {
        // go to shop
        SetBottomButtons(1);
    }

    void OnClickMain()
    {
        // go to main
        SetBottomButtons(2);
    }

    void OnClickInventory()
    {
        //go to inventory
        SetBottomButtons(3);
    }
}
