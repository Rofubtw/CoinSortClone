using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GoldSystem")]
public class GoldSystemSO : ScriptableObject
{
    private int _mergeEarnAmount = 15;
    private int _bonusAmount = 10;
    private int _dealCost = 10;

    public event Action<float> OnGoldChanged;

    public void EarnGold(int fullCoinPackCount, float gold, out float newGold)
    {
        gold += _bonusAmount * fullCoinPackCount;
        gold += _mergeEarnAmount * fullCoinPackCount;
        OnGoldChanged?.Invoke(gold);
        newGold = gold;
    }
    
    public void SpendGold(float gold, out float newGold)
    {
        gold -= _dealCost;
        OnGoldChanged?.Invoke(gold);
        newGold = gold;
    }

    public void SpendGoldAfterPackBuy(float gold, float unlockCost, out float newGold, out float newUnlockCost)
    {
        gold -= unlockCost;
        newGold = gold;

        float costMultiplier = 0.2f;
        newUnlockCost = unlockCost + unlockCost * costMultiplier;
        unlockCost = newUnlockCost;
        OnGoldChanged?.Invoke(gold);
    }
}
