using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class CoinPackBase : MonoBehaviour, ICoinHolder
{
    public bool IsUnlocked { get; set; }
    public int PackLevel { get; protected set; }
    public int AvailableSpace { get; protected set; }
    public Vector3 SpawnPlace {  get; protected set; }

    public virtual List<CoinObject> GetCoinList() => null;

    public virtual void OrganizeCoins() { }

    public virtual bool IsPackReadyForMerge() => false;
    
    public virtual void ClearCoinList() { }
    
}
