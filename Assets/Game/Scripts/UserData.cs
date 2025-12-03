using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public int coin;

    public List<InventoryData> inventoryDatas;

    public void Init(bool firstTime)
    {
        if (firstTime)
        {
            coin = 0;
        }
    }
}
