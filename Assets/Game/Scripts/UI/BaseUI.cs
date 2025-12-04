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

    public virtual void DoUpdate(float dt) { }

    public virtual void Show(params object[] payload)
    {
        gameObject.SetActive(true);
    }
    public virtual void Hide(params object[] payload) 
    {
        gameObject.SetActive(false);
    }

}
