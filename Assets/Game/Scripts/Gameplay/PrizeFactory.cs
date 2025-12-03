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

        SpawnPrize(10);
    }

    public void SpawnPrize(int amount)
    {
        StartCoroutine(SpawningPrize(amount));
    }

    IEnumerator SpawningPrize(int amount)
    {
        int rndIndex;
        for (int i = 0; i < amount; i++)
        {
            rndIndex = Random.Range(0, prefabPrizes.Length);
            Prize prefab = prefabPrizes[rndIndex];
            Instantiate(prefab, transform);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void ReturnPrize(Prize prize)
    {
        activePrizeList.Remove(prize);
        Destroy(prize.gameObject);
    }
}
