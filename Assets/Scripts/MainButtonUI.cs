using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainButtonUI : MonoBehaviour
{
    [SerializeField]
    private Button dealButton;
    [SerializeField]
    private Button mergeButton;

    public event Action OnCoinDeal;
    public event Action OnCoinMerge;

    public static MainButtonUI instance;

    private void Awake()
    {
        instance = this;

        mergeButton.onClick.AddListener(() =>
        {
            OnCoinMerge?.Invoke();
        });
        dealButton.onClick.AddListener(() =>
        {
            Debug.Log("A");
        });
    }
}
