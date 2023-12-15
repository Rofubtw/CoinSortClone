using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] Coin coinPrefab;

    ICoinDirector _coinDirector;
    
    List<ICoin> _newCoinObjects;
    
    public static CoinManager Instance;
   
    void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        _coinDirector = new CoinDirector(new CoinBuilder());
        _newCoinObjects = new();
    }

    ICoin BuildCoin(int level, Colors colors)
    {
        ICoin coin = _coinDirector.BuildCoin(level, colors);

        Coin createdCoin = Instantiate(coinPrefab);
        //Pool
        createdCoin.Init(coin.CoinLevel, coin.Color);

        return createdCoin;
    }
    
    
    public List<ICoin> InstantiateCoinAfterMerge(int coinLevel, Colors color)
    {
        const int requiredCoinCount = 2;
        _newCoinObjects.Clear();
        
        for (int i = 0; i < requiredCoinCount; i++)
        {
            ICoin coin = BuildCoin(coinLevel, color);
            _newCoinObjects.Add(coin);
        }

        return _newCoinObjects;
    }
}
