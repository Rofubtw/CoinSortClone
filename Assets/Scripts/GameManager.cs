using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform coinPackPrefab;

    [SerializeField]
    private CoinPack _selectedCoinPack;
    [SerializeField]
    private CoinPack _nextCoinPack;

    [SerializeField]
    private LayerMask coinPackLayerMask;
    [SerializeField]
    private LayerMask mousePlaneLayerMask;

    public event Action OnSelectedPackChoosed;
    public event Action OnNextPackChoosed;

    private bool _isPackSelected;
    private List<Coin> _selectedCoins = new List<Coin>();

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isPackSelected)
            {
                if (_selectedCoinPack != _nextCoinPack)
                {
                    HandleCoinTransportation();
                }
            }
            else
            {
                HandlePackSelection();
            }
        }
    }

    private void HandleCoinTransportation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, coinPackLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out CoinPack coinPack))
            {
                SetNextCoinPack(coinPack);
                if (_selectedCoinPack == _nextCoinPack)
                {
                    ResetSelections();
                    return;
                }

                SetSelectedCoinList(_nextCoinPack.AddCoins(_selectedCoins));
                
                _selectedCoinPack.RemoveCoins(_selectedCoins);
                

                ResetSelections();
            }
        }
        else if (Physics.Raycast(ray, float.MaxValue, mousePlaneLayerMask))
        {
            ResetSelections();
        }
    }

    private void HandlePackSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, coinPackLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out CoinPack coinPack))
            {
                if (_selectedCoinPack == null)
                {
                    SetSelectedCoinPack(coinPack);
                    SetSelectedCoinList(_selectedCoinPack.SelectCoins());

                    if(_selectedCoins.Count > 0)
                    {
                        _isPackSelected = true;
                    }
                    else
                    {
                        SetSelectedCoinPack(null);
                    }
                }
            }
        }
    }

    private void SetSelectedCoinPack(CoinPack coinPack)
    {
        OnSelectedPackChoosed?.Invoke();
        _selectedCoinPack = coinPack;
    }

    private void SetNextCoinPack(CoinPack coinPack)
    {
        OnNextPackChoosed?.Invoke();
        _nextCoinPack = coinPack; 
    }

    private void ResetSelections()
    {
        SetSelectedCoinPack(null);
        SetNextCoinPack(null);
        _isPackSelected = false;
        _selectedCoins.Clear();
    }

    private void SetSelectedCoinList(List<Coin> coinList)
    {
        _selectedCoins = coinList;
    }
}
