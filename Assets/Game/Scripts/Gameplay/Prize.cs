using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrizeRarity
{
    Common,
    Uncommon,
    Rare,
    VerRare,
    Epic,
    Legend
}

public class Prize : MonoBehaviour
{
    public Rigidbody rb;
    public Collider col;
    PrizeData prizeData;

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

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        bool isPrizeDrop = tag.Equals(Parameter.Tag.PRIZE_DROP);
        if (isPrizeDrop)
        {
            DelayDestroy();

            string id = prizeData.id;
            factory.stageManager.SaveRecord(id, true);
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
