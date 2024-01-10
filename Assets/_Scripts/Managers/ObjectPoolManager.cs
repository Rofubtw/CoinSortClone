using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolEmptyHolder;

    private static GameObject _coinObjectsEmpty;

    public enum PoolType
    {
        CoinObject,
        None
    }

    private void Awake()
    {
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _coinObjectsEmpty = new GameObject("Coin Objects");
        _coinObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 startingPoint, Coin coinInfo, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = objectPools.Find(p => p.lookupInt == coinInfo.CoinLevel);

        //PooledObjectInfo poolS = null;
        //foreach (PooledObjectInfo p in objectPools)
        //{
        //    if (p.lookupString == objectToSpawn.name)
        //    {
        //        pool = p;
        //        break;
        //    }
        //}

        // if pool doesn't exist, create it
        if (pool == null)
        {
            pool = new PooledObjectInfo { lookupInt = coinInfo.CoinLevel };
            objectPools.Add(pool);
        }

        // Check if there are any inactive objects in the pool
        GameObject spawnableObj = pool.inactiveObjects.FirstOrDefault();

        //GameObject spawnableObj = null;
        //foreach (GameObject obj in pool.inactiveObjects)
        //{
        //    if( obj != null)
        //    {
        //        spawnableObj = obj;
        //        break;
        //    }
        //}

        if (spawnableObj == null)
        {
            // Find the parent of the empty object
            GameObject parentObject = SetParentObject(poolType);

            // If there are no inactive objects, create a new one
            spawnableObj = Instantiate(objectToSpawn, startingPoint, Quaternion.identity);

            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            // If there is a inactive object, reactive it
            pool.inactiveObjects.Remove(spawnableObj);
            spawnableObj.transform.position = startingPoint;
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(CoinObject coinObj, Vector3 startingPoint)
    {
        PooledObjectInfo pool = objectPools.Find(p => p.lookupInt == coinObj.CoinLevel);

        if (pool == null)
        {
            Debug.Log("Trying to release an object that is not pooled: " + coinObj.gameObject.name);
        }
        else
        {
            coinObj.gameObject.SetActive(false);
            pool.inactiveObjects.Add(coinObj.gameObject);
            coinObj.transform.position = startingPoint;
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.CoinObject:
                return _coinObjectsEmpty;
            case PoolType.None:
                return null;
            default:
                return null;
        }
    }
}

public class PooledObjectInfo
{
    public int lookupInt;
    public List<GameObject> inactiveObjects = new List<GameObject>();
}
