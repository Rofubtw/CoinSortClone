using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CoinPack : CoinPackBase, ICoinHolder
{
    public static event Action<CoinPackBase> OnAnyPackFull;

    [SerializeField]
    private Transform coinPivotTransform;

    [SerializeField]
    private List<CoinObject> _coinList;

    private const int LIST_MAX_COIN_COUNT = 10;
    private const int LIST_MIN_COIN_COUNT = 0;

    private void OnEnable()
    {
        CoinTransporter.instance.OnCoinTranportationAdd += CoinTransporter_OnCoinTransportationAdd;
        CoinTransporter.instance.OnCoinTranportationRemove += CoinTransporter_OnCoinTranportationRemove;
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
    }

    private void CoinTransporter_OnCoinTransportationAdd()
    {
        OrganizeCoins();
        if (IsPackReadyForMerge())
        {
            SetPackLevel();
        }
    }

    private void CoinTransporter_OnCoinTranportationRemove()
    {
        OrganizeCoins();
    }

    public override void OrganizeCoins()
    {
        SetAvailableSpace();
        Vector3 coinPosition = coinPivotTransform.position;
        Vector3 offsetBetweenCoins = new Vector3(0f, -0.1f, 0f);
        
        foreach (CoinObject coin in _coinList)
        {
            coin.transform.position = coinPosition;
            coinPosition += offsetBetweenCoins;
        }
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
        return true;
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

    public override List<CoinObject> GetCoinList()
    {
        return _coinList;
    }
}

