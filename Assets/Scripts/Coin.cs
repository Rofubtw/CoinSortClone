using UnityEngine;

public class Coin : MonoBehaviour, ICoin
{
    public int CoinLevel { get; set; }
    public Colors Color { get; set; }

    public void Init(int level, Colors color)
    {
        CoinLevel = level;
        Color = color;
    }
}

public interface ICoin
{
    public int CoinLevel { get; set; }
    public Colors Color { get; set; }
}

public enum Colors
{
    Color1 = 0,
    Color2 = 2,
    Color3 = 4,
}
