using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : MonoBehaviour
{
    [field: SerializeField]
    public int CoinLevel { get; set; }
    [field: SerializeField]
    public Color Color { get; set; }

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = Color;
    }

    public void Move(Vector3 target)
    {
        transform.position = target;
    }
}
