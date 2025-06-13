namespace CarteAuxTresors.Services;

public class OrientationService : IOrientationService
{
    public Orientation TurnLeft(Orientation actuelle) =>
        actuelle switch
        {
            Orientation.N => Orientation.O,
            Orientation.O => Orientation.S,
            Orientation.S => Orientation.E,
            Orientation.E => Orientation.N,
            _ => throw new ArgumentOutOfRangeException()
        };

    public Orientation TurnRight(Orientation actuelle) =>
        actuelle switch
        {
            Orientation.N => Orientation.E,
            Orientation.E => Orientation.S,
            Orientation.S => Orientation.O,
            Orientation.O => Orientation.N,
            _ => throw new ArgumentOutOfRangeException()
        };

    public (int dx, int dy) ToDelta(Orientation orientation) =>
        orientation switch
        {
            Orientation.N => (0, -1),
            Orientation.S => (0, 1),
            Orientation.E => (1, 0),
            Orientation.O => (-1, 0),
            _ => (0, 0)
        };
}
