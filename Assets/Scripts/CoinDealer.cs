using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDealer : MonoBehaviour
{
    public static CoinDealer instance { get; private set; }

    public event Action OnCoinDeal;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnCoinDeal?.Invoke();
        }
    }
}
