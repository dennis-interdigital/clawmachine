using System.Collections.Generic;

public class UserData
{
    public int coin;

    public List<bool> probabilityDatas;
    public List<InventoryData> inventoryDatas;
    public List<PrizeRecordData> prizeRecordDatas;

    public void Init(bool firstTime)
    {
        if (firstTime)
        {
            coin = 0;
        }
    }
}
