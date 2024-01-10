using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldCounterText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private GoldSystemSO _goldSystem;

    private void Awake() => _goldSystem.OnGoldChanged += UpdateText;

    private void OnDestroy() => _goldSystem.OnGoldChanged -= UpdateText;

    private void OnEnable() => UpdateText(PackManager.instance.Gold);

    private void OnValidate() => _text = GetComponentInChildren<TMP_Text>();

    private void UpdateText(float gold) => _text.SetText(gold.ToString());

}
