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
//    // Havuzda saklanacak nesnenin prefabý
//    public GameObject objectPrefab;

//    // Havuzun boyutu
//    public int poolSize;

//    // Havuzu tutacak kuyruk
//    private Queue<GameObject> objectPool;

//    // Start metodu
//    void Start()
//    {
//        // Kuyruðu boþ olarak oluþtur
//        objectPool = new Queue<GameObject>();

//        // Kuyruðu istenen boyutta doldur
//        for (int i = 0; i < poolSize; i++)
//        {
//            // Nesne prefabýndan bir örnek oluþtur
//            GameObject obj = Instantiate(objectPrefab);

//            // Nesneyi pasif hale getir
//            obj.SetActive(false);

//            // Nesneyi kuyruða ekle
//            objectPool.Enqueue(obj);
//        }
//    }

//    // Havuzdan bir nesne al
//    public GameObject GetObject()
//    {
//        // Kuyruk boþ deðilse
//        if (objectPool.Count > 0)
//        {
//            // Kuyruðun ilk elemanýný al
//            GameObject obj = objectPool.Dequeue();

//            // Nesneyi aktif hale getir
//            obj.SetActive(true);

//            // Nesneyi döndür
//            return obj;
//        }
//        else
//        {
//            // Kuyruk boþsa, yeni bir nesne oluþtur
//            GameObject obj = Instantiate(objectPrefab);

//            // Nesneyi aktif hale getir
//            obj.SetActive(true);

//            // Nesneyi döndür
//            return obj;
//        }
//    }

//    // Havuza bir nesne býrak
//    public void ReleaseObject(GameObject obj)
//    {
//        // Nesneyi pasif hale getir
//        obj.SetActive(false);

//        // Nesneyi kuyruðun sonuna ekle
//        objectPool.Enqueue(obj);
//    }
//}

