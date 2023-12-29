using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoinHolder
{
    public List<CoinObject> GetCoinList();
    public bool IsPackReadyForMerge();
}
