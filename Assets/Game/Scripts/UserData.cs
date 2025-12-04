using System.Collections.Generic;

public class UserData
{
    public int coin;
    public int diamond;

    public List<bool> probabilityDatas;
    public List<InventoryData> inventoryDatas;
    public List<PrizeRecordData> prizeRecordDatas;

    public void Init(bool firstTime)
    {
        if (firstTime)
        {
            coin = 1000;
            diamond = 100;
        }
    }
}
