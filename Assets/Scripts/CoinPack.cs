using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CoinPack : MonoBehaviour
{
    public event Action OnPackFull;

    [SerializeField]
    private Transform coinPrefab;
    [SerializeField]
    private Transform coinPivotTransform;

    public List<CoinObject> _coinList = new List<CoinObject>(10);
    private List<CoinObject> _emptyList = new List<CoinObject>();

    private void Start()
    {
        CoinDealer.instance.OnCoinDeal += CoinDealer_OnCoinDeal;
        OrganizeCoins();
    }

    private void CoinDealer_OnCoinDeal()
    {
        OrganizeCoins();
    }

    public List<CoinObject> AddCoins(List<CoinObject> selectedCoinList)
    {
        
        //if (_coinList.Count == 10)
        //{
        //    return _emptyList;
        //}

        if (_coinList.Count != 0 && selectedCoinList[^1].CoinLevel != _coinList[^1].CoinLevel) return _emptyList;

        if (_coinList.Count + selectedCoinList.Count > 10)
        {
            int requiredCoinNumber = 10 - _coinList.Count;
            List<CoinObject> returnedList = new List<CoinObject>();
            for (int i = 0; i < requiredCoinNumber; i++)
            {
                _coinList.Add(selectedCoinList[i]);
                returnedList.Add(selectedCoinList[i]);
            }
            OrganizeCoins();

            if (IsPackReadyForMerge())
            {
                OnPackFull?.Invoke();
                Debug.Log("EVENT");
            }

            return returnedList;
        }

        foreach (CoinObject coin in selectedCoinList)
        {
            _coinList.Add(coin);
        }
        OrganizeCoins();

        if (IsPackReadyForMerge())
        {
            OnPackFull?.Invoke();
            Debug.Log("EVENT");
        }

        return selectedCoinList;
    }

    public void RemoveCoins(List<CoinObject> selectedCoinList)
    {
        if (_coinList.Count - selectedCoinList.Count < 0) return;

        foreach (CoinObject coin in selectedCoinList)
        {
            _coinList.Remove(coin);
        }
    }

    public List<CoinObject> SelectCoins()
    {
        List<CoinObject> selectedCoinList = new List<CoinObject>();

        if (_coinList.Count < 1) return selectedCoinList;

        if (_coinList.Count == 10) return selectedCoinList;
        
        int coinLevel = _coinList[^1].CoinLevel;
        for (int i = _coinList.Count - 1; i >= 0; i--)
        {
            if (_coinList[i].CoinLevel != coinLevel)
            {
                break;
            }
            else
            {
                selectedCoinList.Add(_coinList[i]);
            }
        }
        
        return selectedCoinList;
    }

    public void OrganizeCoins()
    {
        Vector3 coinPosition = coinPivotTransform.position;
        Vector3 offsetBetweenCoins = new Vector3(0f, -0.1f, 0f);

        foreach (CoinObject coin in _coinList)
        {
            coin.Move(coinPosition);
            coinPosition += offsetBetweenCoins;
        }
    }

    private bool IsPackReadyForMerge()
    {
        if (_coinList.Count < 10) return false;

        int lastLevel = _coinList[^1].CoinLevel;
        foreach (CoinObject coin in _coinList)
        {
            if (coin.CoinLevel != lastLevel) return false;
        }
        Debug.Log("A");
        GameManager.instance.AddNewPackToList(this);

        return true;
    }

    public int ReturnPackLevel()
    {
        if (_coinList.Count != 10) return 0;

        return _coinList.ElementAt(0).CoinLevel;
    }

    public void AddNewCoinsAfterMerge(CoinObject coinObject)
    {
        _coinList.Add(coinObject);
    }
}

