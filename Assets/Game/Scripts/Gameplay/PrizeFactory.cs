using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeFactory : MonoBehaviour
{
    public Prize[] prefabPrizes;

    public List<Prize> activePrizeList;

    StageManager stageManager;

    public void Init(StageManager inStageManager)
    {
        stageManager = inStageManager;
        int count = activePrizeList.Count;
        for (int i = 0; i < count; i++)
        {
            activePrizeList[i].Init(this);
        }
    }

    public void ReturnPrize(Prize prize)
    {
        activePrizeList.Remove(prize);
        Destroy(prize.gameObject);
    }
}
