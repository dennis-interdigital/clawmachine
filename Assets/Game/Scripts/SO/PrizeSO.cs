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
    public List<int> successRates;
    public List<PrizeData> prizeDatas;

    public PrizeRarity GetRarity(string id)
    {
        PrizeRarity result = PrizeRarity.Common;

        int count = prizeDatas.Count;
        for (int i = 0; i < count; i++)
        {
            PrizeData data = prizeDatas[i];
            bool sameId = data.id.Equals(id);
            if (sameId)
            {
                result = data.rarity;
                break;
            }
        }

        return result;
    }

    public PrizeData GetData(string id)
    {
        PrizeData result = null;
        int count = prizeDatas.Count;
        for (int i = 0; i < count; i++)
        {
            PrizeData data = prizeDatas[i];
            bool sameId = data.id.Equals(id);
            if (sameId)
            {
                result = data;
                break;
            }
        }

        return result;
    }
}
