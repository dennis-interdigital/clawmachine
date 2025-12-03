using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    GameManager gameManager;

    public FloatingJoystick joystick;
    [SerializeField] ClawMachine clawMachine;
    public PrizeFactory prizeFactory;

    [SerializeField] Button buttonGrab;

    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;

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
}
