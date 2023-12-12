using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CoinInstantiater : MonoBehaviour
{
    [SerializeField]
    private GameObject coinPrefab;

    private Coin coin;

    private ICoinBuilder coinBuilder;
    private CoinDirector coinDirector;

    public static CoinInstantiater instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        coinBuilder = new CoinBuilder();

        coinDirector = new CoinDirector(coinBuilder);
    }

    public List<CoinObject> InstantiateCoinAfterMerge(int coinLevel, Color color, Transform transform)
    {
        coin = coinDirector.CreateCoin(coinLevel, color);
        int newCoinCount = 2;
        List<CoinObject> newCoinObjects = new List<CoinObject>();

        for (int i = 0; i < newCoinCount; i++)
        {
            GameObject coinGameObject = Instantiate(coinPrefab);
            coinGameObject.TryGetComponent(out CoinObject coinObject);

            coinObject.CoinLevel = coin.CoinLevel;
            coinObject.Color = coin.Color;
            newCoinObjects.Add(coinObject);
        }


        return newCoinObjects;
    }

}

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class ObjectPool : MonoBehaviour
//{
//    // Havuzda saklanacak nesnenin prefab�
//    public GameObject objectPrefab;

//    // Havuzun boyutu
//    public int poolSize;

//    // Havuzu tutacak kuyruk
//    private Queue<GameObject> objectPool;

//    // Start metodu
//    void Start()
//    {
//        // Kuyru�u bo� olarak olu�tur
//        objectPool = new Queue<GameObject>();

//        // Kuyru�u istenen boyutta doldur
//        for (int i = 0; i < poolSize; i++)
//        {
//            // Nesne prefab�ndan bir �rnek olu�tur
//            GameObject obj = Instantiate(objectPrefab);

//            // Nesneyi pasif hale getir
//            obj.SetActive(false);

//            // Nesneyi kuyru�a ekle
//            objectPool.Enqueue(obj);
//        }
//    }

//    // Havuzdan bir nesne al
//    public GameObject GetObject()
//    {
//        // Kuyruk bo� de�ilse
//        if (objectPool.Count > 0)
//        {
//            // Kuyru�un ilk eleman�n� al
//            GameObject obj = objectPool.Dequeue();

//            // Nesneyi aktif hale getir
//            obj.SetActive(true);

//            // Nesneyi d�nd�r
//            return obj;
//        }
//        else
//        {
//            // Kuyruk bo�sa, yeni bir nesne olu�tur
//            GameObject obj = Instantiate(objectPrefab);

//            // Nesneyi aktif hale getir
//            obj.SetActive(true);

//            // Nesneyi d�nd�r
//            return obj;
//        }
//    }

//    // Havuza bir nesne b�rak
//    public void ReleaseObject(GameObject obj)
//    {
//        // Nesneyi pasif hale getir
//        obj.SetActive(false);

//        // Nesneyi kuyru�un sonuna ekle
//        objectPool.Enqueue(obj);
//    }
//}

