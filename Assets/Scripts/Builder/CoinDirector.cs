public class CoinDirector : ICoinDirector
{
    private ICoinBuilder _coinBuilder;

    public CoinDirector(ICoinBuilder builder)
    {
        _coinBuilder = builder;
    }

    public ICoin BuildCoin(int level, Colors color)
    {
           return  _coinBuilder.
                        SetLevel(level).
                        SetColor(color).
                        Build();
    }
}

public interface ICoinDirector
{
    ICoin BuildCoin(int level, Colors color);
}

