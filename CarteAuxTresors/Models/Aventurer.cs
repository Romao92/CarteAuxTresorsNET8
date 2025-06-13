namespace CarteAuxTresors.Models;

public class Aventurer
{
    private readonly Queue<Movement> _Movements;

    public string Name { get; init; }
    public int X { get; set; }
    public int Y { get; set; }
    public Orientation Orientation { get; set; }
    public int RecoveredTreasures { get; private set; }
    public IReadOnlyCollection<Movement> RemainingMovements => _Movements;

    public Aventurer(string name, int x, int y, Orientation orientation, IEnumerable<Movement> movements)
    {
        Name = name;
        X = x;
        Y = y;
        Orientation = orientation;
        _Movements = new Queue<Movement>(movements ?? []);
    }
    public bool CanMoveAgain() => _Movements.Count > 0;
    public Movement ExtractNextMove() => _Movements.Dequeue();
    public void PickupTreasure() => RecoveredTreasures++; 
}
