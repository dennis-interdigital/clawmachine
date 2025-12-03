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

[Serializable]
public class PrizeData
{
    public string id;
    public string name;
    public PrizeRarity rarity;
}

public class Prize : MonoBehaviour
{
    public Rigidbody rb;
    public Collider col;

    PrizeFactory factory;
    public void Init(PrizeFactory inPrizeFactory)
    {
        factory = inPrizeFactory;
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
