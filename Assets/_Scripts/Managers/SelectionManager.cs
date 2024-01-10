using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private CoinPackBase _selectedCoinPack, _nextCoinPack, _buyableCoinPack;
    private Ray _ray;

    public event Action<CoinPackBase> OnSelectedPackChoosed;
    public event Action<CoinPackBase> OnNextPackChoosed;
    public event Action<CoinPackBase> OnNewPackBuyed;

    private const int LIST_MIN_COIN_COUNT = 0;

    private bool _isNewPackBuyable;

    public static SelectionManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        
    }
    private void Start()
    {
        PackManager.instance.OnNewPackBuyable += PackManager_OnNewPackBuyable;
    }

    private void OnDisable()
    {
        PackManager.instance.OnNewPackBuyable -= PackManager_OnNewPackBuyable;
    }

    private void PackManager_OnNewPackBuyable(CoinPackBase buyableCoinPack)
    {
        _isNewPackBuyable = true;
        _buyableCoinPack = buyableCoinPack;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandlePackSelection();
        }
    }

    private void HandlePackSelection()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out RaycastHit raycastHit, float.MaxValue))
        {
            if (raycastHit.transform.TryGetComponent(out CoinPackBase coinPack))
            {
                if (coinPack == _buyableCoinPack && _isNewPackBuyable)
                {
                    OnNewPackBuyed?.Invoke(coinPack);
                    _isNewPackBuyable = false;
                    return;
                }

                if (!coinPack.IsUnlocked) return;

                if (coinPack.IsPackReadyForMerge()) return;

                if (_selectedCoinPack == null)
                {
                    if (coinPack.GetCoinList().Count > LIST_MIN_COIN_COUNT)
                        SetSelectedCoinPack(coinPack);
                }
                else if (_selectedCoinPack != coinPack)
                {
                    SetNextCoinPack(coinPack);
                }
            }
            else
            {
                _selectedCoinPack = null;
            }
        }
    }

    private void SetSelectedCoinPack(CoinPackBase coinPack)
    {
        _selectedCoinPack = coinPack;
        OnSelectedPackChoosed?.Invoke(_selectedCoinPack);
    }

    private void SetNextCoinPack(CoinPackBase coinPack)
    {
        _nextCoinPack = coinPack;
        OnNextPackChoosed?.Invoke(_nextCoinPack);
        _selectedCoinPack = null;
    }
}
