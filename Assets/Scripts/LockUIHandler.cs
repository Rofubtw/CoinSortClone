using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockUIHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _costText;

    [SerializeField]
    private Transform _template;

    private void OnEnable()
    {
        PackManager.instance.OnNewPackUnlocked += PackManager_OnNewPackUnlocked;
        PackManager.instance.OnReachedMilestoneLevel += PackManager_OnReachedMilestoneLevel;
    }

    private void PackManager_OnReachedMilestoneLevel(float newUnlockCost) => UpdateUnlockCost(newUnlockCost);

    private void PackManager_OnNewPackUnlocked(float newUnlockCost, Vector3 newPosition)
    {
        UpdateUnlockCost(newUnlockCost);
        UpdatePosition(newPosition);
    }

    private void UpdateUnlockCost(float newUnlockCost) => _costText.SetText(newUnlockCost.ToString());

    private void UpdatePosition(Vector3 newPosition) => _template.position = newPosition + new Vector3(0, 0, -0.6f);
}
