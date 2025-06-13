namespace CarteAuxTresors.Interfaces;

public interface IMapVisualizer
{
    string DrawMap(Card Card, bool withHeaders = false);
}
