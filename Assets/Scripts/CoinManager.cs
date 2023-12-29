using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private List<Color> _coinColorList;

    private Coin _coin;

    private ICoinBuilder _coinBuilder;
    private CoinDirector _coinDirector;
    private List<CoinObject> _newCoinObjects;
    private int _newCoinCount = 2;
    private int _dealCoinIndex;
    private int _dealLevelIndex;
    private int _totalMaxCoinNumber;
    private int _availableSpaceInPack;

    private const int LIST_MAX_COIN_COUNT = 10;

    public static CoinManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _coinBuilder = new CoinBuilder();
        _coinDirector = new CoinDirector(_coinBuilder);
        _newCoinObjects = new List<CoinObject>();
    }
    

    public List<CoinObject> InstantiateCoinAfterMerge(int coinLevel)
    {
        _newCoinObjects.Clear();
        if (coinLevel == _coinColorList.Count) 
        {
            _coinColorList.Add(ProduceNewColor());
        }
        _coin = _coinDirector.CreateCoin(coinLevel, _coinColorList.ElementAt(coinLevel - 1));
        
        for (int i = 0; i < _newCoinCount; i++)
        {
            ObjectPoolManager.SpawnObject(coinPrefab, _coin, ObjectPoolManager.PoolType.CoinObject).TryGetComponent(out CoinObject coinObject);

            coinObject.CoinLevel = _coin.CoinLevel;
            coinObject.Color = _coin.Color;
            _newCoinObjects.Add(coinObject);
        }

        return _newCoinObjects;
    }

    public void InstantiateCoinAfterDeal(List<CoinPackBase> unlockedPacks, int highestPackLevel)
    {
        foreach (CoinPackBase coinPack in unlockedPacks)
        {
            _availableSpaceInPack = coinPack.AvailableSpace;
            if (_availableSpaceInPack != 0)
                _dealCoinIndex = Random.Range(0, _availableSpaceInPack + 1);
            else
                _dealCoinIndex = Random.Range(0, _availableSpaceInPack);

            _dealLevelIndex = Random.Range(1, highestPackLevel);

            _coin = _coinDirector.CreateCoin(_dealLevelIndex, _coinColorList.ElementAt(_dealLevelIndex - 1));
            for (int i = 0; i < _dealCoinIndex; i++)
            {
                GameObject coinGameObject = ObjectPoolManager.SpawnObject(coinPrefab, _coin, ObjectPoolManager.PoolType.CoinObject);
                coinGameObject.TryGetComponent(out CoinObject coinObject);

                coinObject.CoinLevel = _coin.CoinLevel;
                coinObject.Color = _coin.Color;

                coinPack.GetCoinList().Add(coinObject);
            }
            coinPack.OrganizeCoins();
            coinPack.IsPackReadyForMerge();
        }
    }

    private Color ProduceNewColor()
    {
        Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

        return randomColor;
    }
}
