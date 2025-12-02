using UnityEngine;

public class ClawMachineGrabArea : MonoBehaviour
{
    ClawMachine clawMachine;
    public Collider col;

    public void Init(ClawMachine inClawMachine)
    {
        clawMachine = inClawMachine;
        Reset();
    }

    public void Reset()
    {
        col.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        bool isPrize = tag.Equals(Parameter.TAG.PRIZE);
        if (isPrize)
        {
            clawMachine.GrabPrize(other.gameObject);
            col.enabled = false;
        }
    }
}
