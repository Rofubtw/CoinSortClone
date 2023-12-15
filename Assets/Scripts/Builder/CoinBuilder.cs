using UnityEngine;

public class CoinBuilder : ICoinBuilder
{
    ICoin _coin;

    public CoinBuilder()
    {
        _coin = new Coin();
    }
    
    public ICoinBuilder SetColor(Colors color)
    {
        _coin.Color = color;
        return this;
    }

    public ICoinBuilder SetLevel(int level)
    {
        _coin.CoinLevel = level;
        return this;
    }

    public ICoin Build()
    {
        return _coin;
    }
}
   
