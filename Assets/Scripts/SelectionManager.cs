using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private ICoinHolder _selectedCoinPack, _nextCoinPack;
    private Ray _ray;

    public event Action<ICoinHolder> OnSelectedPackChoosed;
    public event Action<ICoinHolder> OnNextPackChoosed;

    private const int LIST_MAX_COIN_COUNT = 10;
    private const int LIST_MIN_COIN_COUNT = 0;

    public static SelectionManager instance;
    private void Awake()
    {
        instance = this;
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
            if (raycastHit.transform.TryGetComponent(out ICoinHolder coinPack))
            {
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

    private void SetSelectedCoinPack(ICoinHolder coinPack)
    {
        _selectedCoinPack = coinPack;
        OnSelectedPackChoosed?.Invoke(_selectedCoinPack);
    }

    private void SetNextCoinPack(ICoinHolder coinPack)
    {
        _nextCoinPack = coinPack;
        OnNextPackChoosed?.Invoke(_nextCoinPack);
        _selectedCoinPack = null;
    }
}
