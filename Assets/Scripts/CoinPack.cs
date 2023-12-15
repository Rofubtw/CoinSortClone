using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinPack : MonoBehaviour
{
    [SerializeField] Transform coinPivotTransform;

    int _capacity = 10;
    public int EmptySlots => _capacity - _coinList.Count();
    
    private List<ICoin> _coinList;
    
    public event Action OnPackFull;

    void Start()
    {
        Init();
        OrganizeCoins();    
    }

    void Init()
    {
        _coinList = new List<ICoin>(_capacity);
    }

    public void AddingProcess(CoinPack senderPack)
    {
        AddCoinToPackage(senderPack);
        OrganizeCoins();
        MergeHandler();
    }
    
    private void AddCoinToPackage(CoinPack senderPack)
    {
        List<ICoin> selectedCoinList = senderPack.SelectCoins();
      
        if (_coinList.Count != 0)
        {
            if (selectedCoinList[^1].CoinLevel != _coinList[^1].CoinLevel) return;
        }
        
        int afterMergeCount = Mathf.Min(_coinList.Count + selectedCoinList.Count, _capacity);
      
        int requiredCoinNumber = afterMergeCount - _coinList.Count;
        for (int i = 0; i < requiredCoinNumber; i++)
        {
            ICoin coin = selectedCoinList[i];
            AddItemToCoinList(coin);
            senderPack.RemoveItemToCoinList(coin);
        }
    }

    private void MergeHandler()
    {
        bool isReady = IsPackReadyForMerge();
        if (isReady)
        {
            OnPackFull?.Invoke();
            Debug.Log("EVENT");
        }
    }

    public void RemoveCoins(List<ICoin> selectedCoinList)
    {
        if (_coinList.Count - selectedCoinList.Count < 0) return;

        foreach (ICoin coin in selectedCoinList)
        {
            _coinList.Remove(coin);
        }
    }

    public List<ICoin> SelectCoins()
    {
        List<ICoin> selectedCoinList = new List<ICoin>();

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

        foreach (ICoin coin in _coinList)
        {
            //coin.Move(coinPosition);
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
        //GameManager.instance.AddNewPackToList(this);

        return true;
    }

    public int ReturnPackLevel()
    {
        if (_coinList.Count != 10) return 0;

        return _coinList.ElementAt(0).CoinLevel;
    }

    public void AddNewCoinsAfterMerge(ICoin coin)
    {
        _coinList.Add(coin);
    }

    private void AddItemToCoinList(ICoin coin)
    {
        _coinList.Add(coin);
    }

    public void RemoveItemToCoinList(ICoin coin)
    {
        _coinList.Remove(coin);
    }
}

