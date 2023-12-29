using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoinPackBase : MonoBehaviour, ICoinHolder
{
    public bool IsUnlocked { get; protected set; }
    public int PackLevel { get; protected set; }
    public int AvailableSpace { get; protected set; }

    public virtual List<CoinObject> GetCoinList()
    {
        return null;
    }

    public virtual void OrganizeCoins()
    {

    }

    public virtual bool IsPackReadyForMerge()
    {
        return false;
    }
}
