using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CoinBuilder : ICoinBuilder
{
    private Coin _coin;

    public CoinBuilder()
    {
        _coin = new Coin();
    }

    public ICoinBuilder SetColor(Color color)
    {
        _coin.Color = color;
        return this;
    }

    public ICoinBuilder SetLevel(int level)
    {
        _coin.CoinLevel = level;
        return this;
    }

    public Coin Build()
    {
        return _coin;
    }
}
   
