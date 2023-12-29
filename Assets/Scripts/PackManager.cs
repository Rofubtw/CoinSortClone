using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PackManager : MonoBehaviour
{
    [SerializeField]
    private List<CoinPackBase> _coinPackList = new List<CoinPackBase>();

    [SerializeField]
    private List<CoinPackBase> _unlockedCoinPacks = new List<CoinPackBase>();

    [SerializeField]
    private List<CoinPackBase> _fullCoinPackList = new List<CoinPackBase>();

    public int HighestPackLevel { get; private set; } = 1;

    private int _dealCoinNumber;
    private int _newcoinlevel;

    public event Action OnNewLevelUnlocked;

    public static PackManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        CoinPack.OnAnyPackFull += CoinPack_OnAnyPackFull;
        MainButtonUI.instance.OnCoinMerge += MainButtonUI_OnCoinMerge;
        MainButtonUI.instance.OnCoinDeal += MainButtonUI_OnCoinDeal;
    }

    private void OnDisable()
    {
        CoinPack.OnAnyPackFull -= CoinPack_OnAnyPackFull;
        MainButtonUI.instance.OnCoinMerge -= MainButtonUI_OnCoinMerge;
        MainButtonUI.instance.OnCoinDeal -= MainButtonUI_OnCoinDeal;
    }

    private void CoinPack_OnAnyPackFull(CoinPackBase obj)
    {
        _fullCoinPackList.Add(obj);
    }

    private void MainButtonUI_OnCoinMerge()
    {
        if (_fullCoinPackList.Count == 0) return;

        foreach (CoinPackBase fullcoinpack in _fullCoinPackList)
        {
            _newcoinlevel = fullcoinpack.PackLevel + 1;

            foreach (CoinObject coin in fullcoinpack.GetCoinList())
            {
                ObjectPoolManager.ReturnObjectToPool(coin);
            }
            fullcoinpack.GetCoinList().Clear();

            foreach (CoinObject coin in CoinManager.instance.InstantiateCoinAfterMerge(_newcoinlevel))
            {
                fullcoinpack.GetCoinList().Add(coin);
            }
            fullcoinpack.OrganizeCoins();

            if (_newcoinlevel > HighestPackLevel)
                HighestPackLevel = _newcoinlevel;
        }
        _fullCoinPackList.Clear();
    }

    private void MainButtonUI_OnCoinDeal()
    {
        CoinManager.instance.InstantiateCoinAfterDeal(_unlockedCoinPacks, HighestPackLevel);
    }
}
