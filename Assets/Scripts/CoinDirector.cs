using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDirector
{
    private ICoinBuilder _coinBuilder;

    public CoinDirector(ICoinBuilder coinBuilder)
    {
        _coinBuilder = coinBuilder;
    }

    public Coin CreateCoin(int coinLevel, Color color)
    {
        return _coinBuilder.SetLevel(coinLevel).SetColor(color).Build();
    }
}
