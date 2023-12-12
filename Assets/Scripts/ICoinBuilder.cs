using UnityEngine;

public interface ICoinBuilder
{
    ICoinBuilder SetColor(Color color);
    ICoinBuilder SetLevel(int level);
    Coin Build();

}
