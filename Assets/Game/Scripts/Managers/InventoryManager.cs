using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    public string id;
    public int amount;

    public InventoryData(string inId, int inAmount)
    {
        id = inId; 
        amount = inAmount;
    }
}

public class InventoryManager
{
    GameManager gameManager;
    UserData userData;

    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;
        userData = gameManager.userData;

        if(userData.inventoryDatas == null)
        {
            userData.inventoryDatas = new List<InventoryData>();
        }
    }

    public bool IsInventorySufficient(string id, int cost)
    {
        InventoryData data = GetInventory(id);

        int amount = data.amount;
        bool result = amount >= cost;

        return result;
    }

    public void SpendInventory(string id, int amount)
    {
        bool exist = IsInventoryExist(id);
        if (exist)
        {
            InventoryData data = GetInventory(id);
            data.amount -= amount;
        }
        else
        {
            Debug.LogError("SpendInventoryError: Inventory does not exist in data!");
        }
    }

    public void AddToInventory(string id, int amount)
    {
        bool exist = IsInventoryExist(id);
        if (exist)
        {
            InventoryData data = GetInventory(id);
            data.amount += amount;
        }
        else
        {
            InventoryData data = new InventoryData(id, amount);
            userData.inventoryDatas.Add(data);
        }        
    }

    bool IsInventoryExist(string id)
    {
        bool result = false;

        int count = userData.inventoryDatas.Count;
        for (int i = 0; i < count; i++)
        {
            InventoryData data = userData.inventoryDatas[i];
            bool sameId = data.id.Equals(id);
            if (sameId)
            {
                result = true;
                break;
            }
        }

        return result;
    }

    InventoryData GetInventory(string id)
    {
        InventoryData result = null;

        int count = userData.inventoryDatas.Count;
        for (int i = 0; i < count; i++)
        {
            InventoryData data = userData.inventoryDatas[i];
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
