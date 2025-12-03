using UnityEngine;

public class ClawMachineGrabArea : MonoBehaviour
{
    ClawMachine clawMachine;
    public Collider col;

    public void Init(ClawMachine inClawMachine)
    {
        clawMachine = inClawMachine;
        SetColliderEnable(false);
    }

    public void SetColliderEnable(bool enable)
    {
        col.enabled = enable;
    }

    void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        bool isPrize = tag.Equals(Parameter.Tag.PRIZE);
        if (isPrize)
        {
            clawMachine.GrabPrize(other.gameObject);
            SetColliderEnable(false);
        }
    }
}
