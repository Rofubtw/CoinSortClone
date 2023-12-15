public interface ICoinBuilder
{
    ICoinBuilder SetColor(Colors color);
    ICoinBuilder SetLevel(int level);
    ICoin Build();
}
