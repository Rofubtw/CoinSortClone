using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinInstantiater : MonoBehaviour
{
    [SerializeField]
    private Coin CoinLevel1;
    [SerializeField]
    private Coin CoinLevel2;
    [SerializeField]
    private Coin CoinLevel3;
    [SerializeField]
    private Coin CoinLevel4;

    public static CoinInstantiater instance;

    private void Awake()
    {
        instance = this;
    }

    public void MergeCoins()
    {

    }
}
