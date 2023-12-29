using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinTransporter : MonoBehaviour
{
    private List<CoinObject> _selectedCoinObjects;
    private List<CoinObject> selectedPacksObjects;
    private int _neededLevel;

    private const int  LIST_MAX_COIN_COUNT = 10;
    private const int LIST_MIN_COIN_COUNT = 0;

    public event Action OnCoinTranportationAdd;
    public event Action OnCoinTranportationRemove;
    

    public static CoinTransporter instance;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        SelectionManager.instance.OnSelectedPackChoosed += SelectionManager_OnSelectedPackChoosed;
        SelectionManager.instance.OnNextPackChoosed += SelectionManager_OnNextPackChoosed;
    }

    private void Start()
    {
        _selectedCoinObjects = new List<CoinObject>();
    }

    private void OnDisable()
    {
        SelectionManager.instance.OnSelectedPackChoosed -= SelectionManager_OnSelectedPackChoosed;
        SelectionManager.instance.OnNextPackChoosed -= SelectionManager_OnNextPackChoosed;
    }

    private void SelectionManager_OnNextPackChoosed(ICoinHolder nextCoinPack)
    {
        if (AddCoins(nextCoinPack, out int neededCoinNumber))
            RemoveCoins(neededCoinNumber, selectedPacksObjects);
    }

    private void SelectionManager_OnSelectedPackChoosed(ICoinHolder selectedCoinPack)
    {
        SelectCoins(selectedCoinPack);
    }

    private void SelectCoins(ICoinHolder selectedCoinPack)
    {
        selectedPacksObjects = selectedCoinPack.GetCoinList();
        _selectedCoinObjects.Clear();
        int coinLevel = selectedCoinPack.GetCoinList()[^1].CoinLevel;
        
        for (int i = selectedCoinPack.GetCoinList().Count - 1; i >= 0; i--)
        {
            if (selectedCoinPack.GetCoinList()[i].CoinLevel != coinLevel)
            {
                break;
            }
            else
            {
                _selectedCoinObjects.Add(selectedCoinPack.GetCoinList()[i]);
            }
        }
    }

    private bool AddCoins(ICoinHolder nextCoinPack, out int neededCoinNumber)
    {
        _neededLevel = _selectedCoinObjects[^1].CoinLevel;
        
        neededCoinNumber = Mathf.Min(LIST_MAX_COIN_COUNT - nextCoinPack.GetCoinList().Count, _selectedCoinObjects.Count);
        
        if (neededCoinNumber == LIST_MIN_COIN_COUNT) return false;

        if (nextCoinPack.GetCoinList().Count > LIST_MIN_COIN_COUNT)
            if (_neededLevel != nextCoinPack.GetCoinList()[^1].CoinLevel) return false;
       
        for (int i = neededCoinNumber - 1; i >= 0; i--)
        {
            nextCoinPack.GetCoinList().Add(_selectedCoinObjects[i]);
        }

        OnCoinTranportationAdd?.Invoke();
        return true;
    }

    private void RemoveCoins(int removeCoinNumber, List<CoinObject> selectedPacksObjects)
    {
        for (int i = removeCoinNumber - 1; i >= 0; i--)
        {
            selectedPacksObjects.Remove(_selectedCoinObjects[i]);
        }
        selectedPacksObjects = null;
        OnCoinTranportationRemove?.Invoke();
    }
}
