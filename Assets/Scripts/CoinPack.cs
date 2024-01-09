using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CoinPack : CoinPackBase, ICoinHolder
{
    public static event Action<CoinPackBase> OnAnyPackFull;

    [SerializeField]
    private Transform _coinPivotTransform;

    [SerializeField]
    private List<CoinObject> _coinList;

    //[SerializeField]
    //private PackImageHandler _packImageHandler;

    private const int LIST_MAX_COIN_COUNT = 10;
    private const int LIST_MIN_COIN_COUNT = 0;
    private Vector3 _startingPoint = new Vector3(-0.08f, -3.6f, -2.28f);

    private void OnEnable()
    {
        CoinTransporter.instance.OnCoinTranportationAdd += CoinTransporter_OnCoinTransportationAdd;
        CoinTransporter.instance.OnCoinTranportationRemove += CoinTransporter_OnCoinTranportationRemove;
        //_packImageHandler.Setup(transform, IsUnlocked);
    }

    private void OnDisable()
    {
        CoinTransporter.instance.OnCoinTranportationAdd -= CoinTransporter_OnCoinTransportationAdd;
        CoinTransporter.instance.OnCoinTranportationRemove -= CoinTransporter_OnCoinTranportationRemove;
    }

    private void Start()
    {
        OrganizeCoins();
        SetPackLevel();
        SpawnPlace = _coinPivotTransform.position;
    }

    private void CoinTransporter_OnCoinTransportationAdd()
    {
        OrganizeCoins();
        IsPackReadyForMerge();
    }

    private void CoinTransporter_OnCoinTranportationRemove()
    {
        OrganizeCoins();
    }

    public override void OrganizeCoins()
    {
        SetAvailableSpace();

        Vector3 coinPosition = SpawnPlace;
        Vector3 offsetBetweenCoins = new Vector3(0, -0.1f, -0.03f);
        
        foreach (CoinObject coin in _coinList)
        {
            if (Vector3.Distance(coin.transform.position, coinPosition) >= 0.1f)
            {
                coin.transform.DOMove(coinPosition, 0.5f);
            }
            coinPosition += offsetBetweenCoins;
        }
    }

    public override List<CoinObject> GetCoinList()
    {
        return _coinList;
    }

    public override bool IsPackReadyForMerge()
    {
        if (_coinList.Count < LIST_MAX_COIN_COUNT) return false;

        int lastLevel = _coinList[^1].CoinLevel;
        foreach (CoinObject coin in _coinList)
        {
            if (coin.CoinLevel != lastLevel) return false;
        }
        OnAnyPackFull?.Invoke(this);
        SetPackLevel();
        SetAvailableSpace();
        return true;
    }

    public override void ClearCoinList()
    {
        foreach (CoinObject coin in _coinList)
        {
            ObjectPoolManager.ReturnObjectToPool(coin, _startingPoint);
        }
        _coinList.Clear();
    }

    private void SetAvailableSpace()
    {
        AvailableSpace = Mathf.Max(LIST_MAX_COIN_COUNT - _coinList.Count, 0);
    }

    private void SetPackLevel()
    {
        if (_coinList.Count > LIST_MIN_COIN_COUNT)
            PackLevel = _coinList.ElementAt(0).CoinLevel;
        else
            PackLevel = LIST_MIN_COIN_COUNT;
    }
    
}

