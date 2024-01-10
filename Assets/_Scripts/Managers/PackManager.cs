using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PackManager : MonoBehaviour
{
    [SerializeField]
    private List<CoinPackBase> _lockedCoinPacks;

    [SerializeField]
    private List<CoinPackBase> _unlockedCoinPacks = new List<CoinPackBase>();

    [SerializeField]
    private GoldSystemSO _goldSystem;

    private HashSet<CoinPackBase> _fullCoinPackList = new HashSet<CoinPackBase>();

    public int HighestPackLevel { get; private set; } = 3;
    public float Gold { get; set; } = 165f;
    public float UnlockCost { get; private set; } = 200f;

    public event Action OnNewLevelUnlocked;
    public event Action OnGameOver;
    public event Action<float, Vector3> OnNewPackUnlocked;
    public event Action<float> OnReachedMilestoneLevel;
    public event Action<CoinPackBase> OnNewPackBuyable;

    public static PackManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        Setup();
        CoinPack.OnAnyPackFull += CoinPack_OnAnyPackFull;
        MainButtonUI.instance.OnCoinMerge += MainButtonUI_OnCoinMerge;
        MainButtonUI.instance.OnCoinDeal += MainButtonUI_OnCoinDeal;
        SelectionManager.instance.OnNewPackBuyed += SelectionManager_OnNewPackBuyed;
    }

    private void Start()
    {
        OnNewPackUnlocked?.Invoke(UnlockCost, _lockedCoinPacks[0].transform.position);
    }

    private void OnDisable()
    {
        CoinPack.OnAnyPackFull -= CoinPack_OnAnyPackFull;
        MainButtonUI.instance.OnCoinMerge -= MainButtonUI_OnCoinMerge;
        MainButtonUI.instance.OnCoinDeal -= MainButtonUI_OnCoinDeal;
        SelectionManager.instance.OnNewPackBuyed -= SelectionManager_OnNewPackBuyed;
    }

    private void Setup()
    {
        foreach (CoinPackBase coinPack in _unlockedCoinPacks)
        {
            coinPack.IsUnlocked = true;
        }
    }

    private void SelectionManager_OnNewPackBuyed(CoinPackBase obj)
    {  
        obj.IsUnlocked = true;

        _unlockedCoinPacks.Add(obj);
        _lockedCoinPacks.Remove(obj);

        _goldSystem.SpendGoldAfterPackBuy(Gold, UnlockCost, out float newGold, out float newUnlockCost);

        Gold = newGold;
        UnlockCost = newUnlockCost;

        OnNewPackUnlocked?.Invoke(UnlockCost, _lockedCoinPacks[0].transform.position);
    }

    private void CoinPack_OnAnyPackFull(CoinPackBase obj)
    {
        _fullCoinPackList.Add(obj);
    }

    private void MainButtonUI_OnCoinMerge()
    {
        if (_fullCoinPackList.Count == 0) return;
        int newCoinLevel;
        int mergeLevelUpAmount = 1;
        
        foreach (CoinPackBase fullCoinPack in _fullCoinPackList)
        {
            newCoinLevel = fullCoinPack.PackLevel + mergeLevelUpAmount;
            fullCoinPack.ClearCoinList();

            CoinManager.instance.InstantiateCoinAfterMerge(newCoinLevel, fullCoinPack.GetCoinList(), fullCoinPack.SpawnPlace);
            fullCoinPack.OrganizeCoins();

            CheckLevelAfterMerge(newCoinLevel);
        }
        _goldSystem.EarnGold(_fullCoinPackList.Count, Gold, out float newGold);
        Gold = newGold;
        if (Gold >= UnlockCost)
        {
            OnNewPackBuyable?.Invoke(_lockedCoinPacks[0]);
        }
        _fullCoinPackList.Clear();
    }

    private void MainButtonUI_OnCoinDeal()
    {
        int dealCost = 10, minGoldAmount = 0;
        if (Gold - dealCost < minGoldAmount)
        {
            OnGameOver?.Invoke();
            return;
        }

        int DealIndex = 0;
        int randomPackIndex = UnityEngine.Random.Range(0, _unlockedCoinPacks.Count);
        foreach (CoinPackBase unclockedCoinPack in _unlockedCoinPacks)
        {
            if (_unlockedCoinPacks.ElementAt(randomPackIndex) == unclockedCoinPack)
            {
                continue;
            }

            CoinManager.instance.InstantiateCoinAfterDeal(unclockedCoinPack.GetCoinList(), HighestPackLevel,
                                                          unclockedCoinPack.AvailableSpace, _unlockedCoinPacks.Count, out bool isDealDone);
            if (isDealDone)
            {
                unclockedCoinPack.OrganizeCoins();
                unclockedCoinPack.IsPackReadyForMerge();
                DealIndex++;
            }
        }
        _goldSystem.SpendGold(Gold, out float newGold);
        Gold = newGold;
        // If there is no deal and there is no pack to merge, the game is over.
        if (DealIndex == 0 && _fullCoinPackList.Count == 0)
        {
            OnGameOver?.Invoke();
        }
    }

    private void CheckLevelAfterMerge(int newCoinLevel)
    {
        if (newCoinLevel > HighestPackLevel)
        {
            HighestPackLevel = newCoinLevel;
            int levelMilestones = 5;
            int completeDiv = 0;
            if (HighestPackLevel % levelMilestones == completeDiv)
            {
                float milestoneReward = 0.5f;
                UnlockCost = UnlockCost * milestoneReward;
                OnReachedMilestoneLevel?.Invoke(UnlockCost);
            }
        }
    }
}
