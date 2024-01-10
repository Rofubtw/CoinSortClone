using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _coinPrefab;

    [SerializeField]
    private List<Color> _coinColorList;

    private Coin _coin;

    private ICoinBuilder _coinBuilder;
    private CoinDirector _coinDirector;
    private Vector3 _startingPoint = new Vector3(-0.08f, -3.6f, -2.28f);

    public static CoinManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _coinBuilder = new CoinBuilder();
        _coinDirector = new CoinDirector(_coinBuilder);
    }
    

    public void InstantiateCoinAfterMerge(int coinLevel, List<CoinObject> coinObjects, Vector3 spawnPlace)
    {
        if (coinLevel == _coinColorList.Count) 
        {
            _coinColorList.Add(ProduceNewColor());
        }
        _coin = _coinDirector.CreateCoin(coinLevel, _coinColorList.ElementAt(coinLevel - 1));

        int newCoinCount = 2;
        for (int i = 0; i < newCoinCount; i++)
        {
            ObjectPoolManager.SpawnObject(_coinPrefab, spawnPlace, _coin, ObjectPoolManager.PoolType.CoinObject).TryGetComponent(out CoinObject coinObject);

            coinObject.CoinLevel = _coin.CoinLevel;
            coinObject.Color = _coin.Color;
            coinObjects.Add(coinObject);
        }
    }

    public void InstantiateCoinAfterDeal(List<CoinObject> coinObjects, int highestPackLevel, int availableSpaceInPack, int unlockedCoinPackCount, out bool isDealDone)
    {
        if (availableSpaceInPack == 0)
        {
            isDealDone = false;
            return;
        }

        int dealCoinIndex = Random.Range(1, Mathf.Min(availableSpaceInPack, unlockedCoinPackCount + 2));
        int dealLevelIndex = Random.Range(1, highestPackLevel);

        _coin = _coinDirector.CreateCoin(dealLevelIndex, _coinColorList.ElementAt(dealLevelIndex - 1));
        
        for (int i = 0; i < dealCoinIndex; i++)
        {
            GameObject coinGameObject = ObjectPoolManager.SpawnObject(_coinPrefab, _startingPoint, _coin, ObjectPoolManager.PoolType.CoinObject);
            coinGameObject.TryGetComponent(out CoinObject coinObject);

            coinObject.CoinLevel = _coin.CoinLevel;
            coinObject.Color = _coin.Color;

            coinObjects.Add(coinObject);
        }
        isDealDone = true;
    }

    private Color ProduceNewColor()
    {
        Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

        return randomColor;
    }
}
