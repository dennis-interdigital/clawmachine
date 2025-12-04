using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeFactory : MonoBehaviour
{
    public PrizeSO prizeSO;
    public Prize[] prefabPrizes;

    public List<Prize> activePrizeList;

    public int editorSpawnAmount;

    [HideInInspector] public StageManager stageManager;
    GameManager gameManager;
    [HideInInspector] public InventoryManager inventoryManager;
    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;
        inventoryManager = gameManager.inventoryManager;
        stageManager = gameManager.stageManager;

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
            PrizeData data = prizeSO.prizeDatas[rndIndex];
            Prize prefab = prefabPrizes[rndIndex];
            Prize prize = Instantiate(prefab, transform);
            prize.Init(this, data);
            prize.index = rndIndex;

            activePrizeList.Add(prize);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void ReturnPrize(Prize prize)
    {
        activePrizeList.Remove(prize);
        Destroy(prize.gameObject);
    }
}
