using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public FloatingJoystick joystick;
    [SerializeField] ClawMachine clawMachine;
    public PrizeFactory prizeFactory;

    [SerializeField] Button buttonGrab;

    void Start()
    {
        RectTransform rtJoystick = joystick.GetComponent<RectTransform>();
        rtJoystick.sizeDelta = new Vector2(Screen.width / 2, Screen.height / 2);

        prizeFactory.Init(this);
        clawMachine.Init(this);

        buttonGrab.onClick.AddListener(clawMachine.OnClickGrab);
    }

    void Update()
    {
        clawMachine.DoUpdate();
    }
}
