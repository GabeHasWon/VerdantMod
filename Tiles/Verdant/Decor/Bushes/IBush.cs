namespace Verdant.Tiles.Verdant.Decor.Bushes;

internal interface IBush
{
    bool CanBeTrimmed(int x, int y);
    void ChooseTrim(int x, int y);
}
