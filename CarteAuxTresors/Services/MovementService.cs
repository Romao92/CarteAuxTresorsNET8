namespace CarteAuxTresors.Services;

public class MovementService : IMovementService
{
    private readonly IOrientationService _orientationService;

    public MovementService(IOrientationService orientationService)
    {
        _orientationService = orientationService;
    }

    public void Move(Card Card, Aventurer Aventurer)
    {
        var (dx, dy) = _orientationService.ToDelta(Aventurer.Orientation);
        int newX = Aventurer.X + dx;
        int newY = Aventurer.Y + dy;

        if (!Card.EstDansLesBornes(newX, newY))
        {
            LogHelper.Warning("🛑{Nom} is unable to move towards ({X},{Y}) : outside the limits of the map.", Aventurer.Name, newX, newY);
            return;
        }

        var prochaineCell = Card.GetCell(newX, newY);

        if (prochaineCell.IsMountain)
        {
            LogHelper.Warning("🛑{Nom} is unable to move towards ({X},{Y}) : blocked by a mountain.", Aventurer.Name, newX, newY);
            return;
        }

        if (prochaineCell.Aventurer != null)
        {
            LogHelper.Warning("🛑{Nom} is unable to move towards ({X},{Y}) :  the adventurer {Bloqueur} already there.", Aventurer.Name, newX, newY, prochaineCell.Aventurer.Name);
            return;
        }

        //si prochaine Cell ok on se deplace

        var CellActuelle = Card.GetCell(Aventurer.X, Aventurer.Y);
        CellActuelle.Aventurer = null;

        Aventurer.X = newX;
        Aventurer.Y = newY;

        var nouvelleCell = Card.GetCell(newX, newY);
        if (nouvelleCell.PickupTreasure())
        {
            Aventurer.PickupTreasure();
            LogHelper.Info("💰{Nom} picks up a treasure at position ({X},{Y})", Aventurer.Name, newX, newY);
        }

        nouvelleCell.Aventurer = Aventurer;
    }
}
