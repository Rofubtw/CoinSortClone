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

    public List<Coin> _coinList = new List<Coin>(10);
    private List<Coin> _emptyList = new List<Coin>();

    private void Start()
    {
        CoinDealer.instance.OnCoinDeal += CoinDealer_OnCoinDeal;
        OrganizeCoins();
    }

    private void CoinDealer_OnCoinDeal()
    {
        OrganizeCoins();
    }

    public List<Coin> AddCoins(List<Coin> selectedCoinList)
    {
        
        if (_coinList.Count == 10)
        {
            return _emptyList;
        }

        if (_coinList.Count != 0 && selectedCoinList[^1].CoinLevel != _coinList[^1].CoinLevel) return _emptyList;

        if (_coinList.Count + selectedCoinList.Count > 10)
        {
            int requiredCoinNumber = 10 - _coinList.Count;
            List<Coin> returnedList = new List<Coin>();
            for (int i = 0; i < requiredCoinNumber; i++)
            {
                _coinList.Add(selectedCoinList[i]);
                returnedList.Add(selectedCoinList[i]);
            }
            OrganizeCoins();
            OnPackFull?.Invoke();

            return returnedList;
        }

        foreach (Coin coin in selectedCoinList)
        {
            _coinList.Add(coin);
        }
        OrganizeCoins();

        return selectedCoinList;
    }

    public void RemoveCoins(List<Coin> selectedCoinList)
    {
        if (_coinList.Count - selectedCoinList.Count < 0) return;

        foreach (Coin coin in selectedCoinList)
        {
            _coinList.Remove(coin);
        }
    }

    public List<Coin> SelectCoins()
    {
        List<Coin> selectedCoinList = new List<Coin>();

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

    private void OrganizeCoins()
    {
        Vector3 coinPosition = coinPivotTransform.position;
        Vector3 offsetBetweenCoins = new Vector3(0f, -0.1f, 0f);

        foreach (Coin coin in _coinList)
        {
            coin.Move(coinPosition);
            coinPosition += offsetBetweenCoins;
        }
    }

    private void MergeCoins()
    {
        int level = _coinList[0].CoinLevel;
        _coinList.Clear();

        
    }
}

