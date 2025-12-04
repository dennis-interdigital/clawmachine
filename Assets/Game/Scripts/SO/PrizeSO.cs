using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrizeData
{
    public string id;
    public string name;
    public PrizeRarity rarity;
}

[CreateAssetMenu(fileName = "PrizeSO", menuName = "SO/PrizeSO")]
public class PrizeSO : ScriptableObject
{
    public List<PrizeData> prizeDatas;
}
