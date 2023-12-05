using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [field: SerializeField]
    public int CoinLevel { get; private set; } = 1;

    private float speed;
    public void Move(Vector3 target)
    {
        transform.position = target;
    }
}
