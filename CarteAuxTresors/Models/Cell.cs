namespace CarteAuxTresors.Models;

public class Cell
{
    public int X { get; }
    public int Y { get; }
    public bool IsMountain { get; set; }
    public int NumberOfTreasures { get; private set; }
    public void SetTresors(int nb) => NumberOfTreasures = nb;

    public Aventurer? Aventurer { get; set; }

    public Cell(int x, int y, bool isMountain = false, int tresors = 0)
    {
        X = x;
        Y = y;
        IsMountain = isMountain;
        NumberOfTreasures = tresors;
    }

    //public bool IsAccessible() => Aventurer is null && !isMountain;

    public bool PickupTreasure()
    {
        if (NumberOfTreasures > 0)
        {
            NumberOfTreasures--;
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        //old version:
        // if (Aventurer != null) return $"A({Aventurer.Name[0]})";
        // if (IsMountain) return "M";
        // if (NumberOfTreasures > 0) return $"T({NumberOfTreasures})";
        // return ".";
        if (Aventurer != null) return $"A({Aventurer.Name[0]})".PadRight(6);
        if (IsMountain) return "M".PadRight(6);
        if (NumberOfTreasures > 0) return $"T({NumberOfTreasures})".PadRight(6);
        return ".".PadRight(6);
    }

}
