using System.Collections;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    protected GameManager gameManager;
    protected UIManager uiManager;

    public virtual void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;
        uiManager = gameManager.uiManager;
    }

    public virtual void Show(){ }
    public virtual void Hide() { }

}
