namespace CarteAuxTresors.Interfaces;

public interface IOrientationService
{
    Orientation TurnLeft(Orientation actuelle);
    Orientation TurnRight(Orientation actuelle);
    (int dx, int dy) ToDelta(Orientation orientation);
}
