using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CoinPack _selectedCoinPack;
    private CoinPack _nextCoinPack;

    [SerializeField]
    private LayerMask mousePlaneLayerMask;

    [SerializeField]
    private List<Color> _coinColorList;

    public event Action OnSelectedPackChoosed;
    public event Action OnNextPackChoosed;
    public event Action OnNewLevelUnlocked;

    public int GameLevel {  get; private set; }

    private bool _isPackSelected;
    private List<CoinObject> _selectedCoinList = new List<CoinObject>();
    [SerializeField]
    private List<CoinPack> _fullCoinPackList = new List<CoinPack>();

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        MainButtonUI.instance.OnCoinMerge += MainButtonUI_OnCoinMerge;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isPackSelected)
            {
                if (_selectedCoinPack != _nextCoinPack)
                {
                    HandleCoinTransportation();
                }
            }
            else
            {
                HandlePackSelection();
            }
        }
    }

    private void HandleCoinTransportation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
        {
            if (raycastHit.transform.TryGetComponent(out CoinPack coinPack))
            {
                SetNextCoinPack(coinPack);
                if (_selectedCoinPack == _nextCoinPack)
                {
                    ResetSelections();
                    return;
                }

                SetSelectedCoinList(_nextCoinPack.AddCoins(_selectedCoinList));
                
                _selectedCoinPack.RemoveCoins(_selectedCoinList);
                

                ResetSelections();
            }
        }
        else if (Physics.Raycast(ray, float.MaxValue, mousePlaneLayerMask))
        {
            ResetSelections();
        }
    }

    private void HandlePackSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
        {
            if (raycastHit.transform.TryGetComponent(out CoinPack coinPack))
            {
                if (_selectedCoinPack == null)
                {
                    SetSelectedCoinPack(coinPack);
                    SetSelectedCoinList(_selectedCoinPack.SelectCoins());

                    if(_selectedCoinList.Count > 0)
                    {
                        _isPackSelected = true;
                    }
                    else
                    {
                        SetSelectedCoinPack(null);
                    }
                }
            }
        }
    }

    private void MainButtonUI_OnCoinMerge()
    {
        foreach (CoinPack fullCoinPack in _fullCoinPackList)
        {
            int newCoinLevel = 0;
            Color newCoinColor;
            if (fullCoinPack.ReturnPackLevel() == GameLevel)
            {
                OnNewLevelUnlocked?.Invoke();
                IncreaseGameLevel();
                newCoinLevel = GameLevel;
                newCoinColor = ProduceNewColor();
            }
            else
            {
                newCoinLevel = fullCoinPack.ReturnPackLevel() + 1;
                newCoinColor = _coinColorList.ElementAt(newCoinLevel - 1);
            }
            foreach (CoinObject coin in fullCoinPack.CoinList)
            {
                coin.gameObject.SetActive(false);
            }
            fullCoinPack.CoinList.Clear();

            List<CoinObject> newCoins = CoinInstantiater.instance.InstantiateCoinAfterMerge(newCoinLevel, newCoinColor, fullCoinPack.transform);
            foreach (CoinObject coin in newCoins)
            {
                fullCoinPack.AddNewCoinsAfterMerge(coin);
            }
            fullCoinPack.OrganizeCoins();
        }
    }

    private void SetSelectedCoinPack(CoinPack coinPack)
    {
        OnSelectedPackChoosed?.Invoke();
        _selectedCoinPack = coinPack;
    }

    private void SetNextCoinPack(CoinPack coinPack)
    {
        OnNextPackChoosed?.Invoke();
        _nextCoinPack = coinPack; 
    }

    private void SetSelectedCoinList(List<CoinObject> coinList)
    {
        _selectedCoinList = coinList;
    }

    private void ResetSelections()
    {
        SetSelectedCoinPack(null);
        SetNextCoinPack(null);
        _isPackSelected = false;
        _selectedCoinList.Clear();
    }

    public void IncreaseGameLevel()
    {
        GameLevel++;
    }

    public void AddNewPackToList(CoinPack coinPack)
    {
        _fullCoinPackList.Add(coinPack);
    }

    private Color ProduceNewColor()
    {
        Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

        return randomColor;
    }
}
