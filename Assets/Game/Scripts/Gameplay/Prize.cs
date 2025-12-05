using DG.Tweening;
using UnityEngine;

public enum PrizeRarity
{
    Common,
    Uncommon,
    Rare,
    VeryRare,
    Epic,
    Legend,
    COUNT
}

public class Prize : MonoBehaviour
{
    public Rigidbody rb;
    public Collider col;
    [HideInInspector] public PrizeData prizeData;
    [HideInInspector] public int index;
    PrizeFactory factory;
    public void Init(PrizeFactory inPrizeFactory, PrizeData inData)
    {
        factory = inPrizeFactory;
        prizeData = inData;
    }

    public void SetPhysics(bool enable)
    {
        rb.useGravity = enable;
        col.enabled = enable;

        if (enable)
        {
            float rndTorqueX = Random.Range(-50f, 50f);
            float rndTorqueY = Random.Range(-50f, 50f);
            float rndTorqueZ = Random.Range(-50f, 50f);

            Vector3 rndTorque = new Vector3(rndTorqueX, rndTorqueY, rndTorqueZ);
            rb.AddRelativeTorque(rndTorque, ForceMode.Acceleration);
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        bool isPrizeDrop = tag.Equals(Parameter.Tag.PRIZE_DROP);
        if (isPrizeDrop)
        {
            string id = prizeData.id;
            factory.stageManager.SaveRecord(prizeData, index, true);
            factory.inventoryManager.AddToInventory(id, 1);
            DelayDestroy();
        }
    }

    void DelayDestroy()
    {
        DOVirtual.DelayedCall(3f, () =>
        {
            factory.ReturnPrize(this);
        });
    }
}
