namespace CarteAuxTresors.Models;

public class Card
{
    public int Width { get; }
    public int Height { get; }

    private readonly Cell[,] _grille;

    public Card(int width, int height)
    {
        Width = width;
        Height = height;
        _grille = new Cell[height, width];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                _grille[y, x] = new Cell(x, y);
    }

    public bool EstDansLesBornes(int x, int y) =>
        x >= 0 && x < Width && y >= 0 && y < Height;

    public Cell GetCell(int x, int y) => _grille[y, x];

    public IEnumerable<Cell> GetMountainCells() =>
       GetAllCells().Where(c => c.IsMountain);

    public IEnumerable<Cell> GetTreasureCells() =>
        GetAllCells().Where(c => !c.IsMountain && c.NumberOfTreasures > 0);

    public IEnumerable<Cell> GetAllCells()
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                yield return _grille[y, x];
    }

    // public void DrawMapOnConsole()
    // {
    //     for (int y = 0; y < Height; y++)
    //     {
    //         for (int x = 0; x < Width; x++)
    //         {
    //             Console.Write($"{_grille[y, x]} ");
    //         }
    //         Console.WriteLine();
    //     }
    // }
}
