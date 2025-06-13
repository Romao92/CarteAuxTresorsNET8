using System.Text;

namespace CarteAuxTresors.Services;

public class MapVisualizer : IMapVisualizer
{
    // public string DrawMap(Card Card)
    // {
    //     var sb = new StringBuilder();
    //     for (int y = 0; y < Card.Height; y++)
    //     {
    //         for (int x = 0; x < Card.Width; x++)
    //         {
    //             var cell = Card.GetCell(x, y);
    //             sb.Append($"{FormatCell(cell)} ");
    //         }
    //         sb.AppendLine();
    //     }

    //     return sb.ToString();
    // }

    // private string FormatCell(Cell cell)
    // {
    //     if (cell.Aventurer != null)
    //         return $"A({cell.Aventurer.Name[0]})";

    //     if (cell.IsMountain)
    //         return "M";

    //     if (cell.NumberOfTreasures > 0)
    //         return $"T({cell.NumberOfTreasures})";

    //     return ".";
    // }
public string DrawMap(Card card, bool withHeaders = false)
{
    var sb = new StringBuilder();
    int largeur = card.Width;
    int hauteur = card.Height;

    if (withHeaders)
    {
        // En-tête des colonnes
        sb.Append("    "); // Espace pour l'index de ligne
        for (int x = 0; x < largeur; x++)
            sb.Append($"{x}".PadRight(7));
        sb.AppendLine();

        // Ligne de séparation supérieure
        sb.Append("  +" + string.Join("+", Enumerable.Repeat("------", largeur)) + "+");
        sb.AppendLine();
    }

    // Corps de la grille
    for (int y = 0; y < hauteur; y++)
    {
        if (withHeaders)
            sb.Append($"{y} |"); // Index de ligne + séparateur
        for (int x = 0; x < largeur; x++)
        {
            var cell = card.GetCell(x, y);
            sb.Append(FormatCell(cell));
            if (withHeaders)
                sb.Append("|");
            else
                sb.Append(" ");
        }
        sb.AppendLine();
    }

    if (withHeaders)
    {
        // Ligne de séparation inférieure
        sb.Append("  +" + string.Join("+", Enumerable.Repeat("------", largeur)) + "+");
        sb.AppendLine();
    }

    return sb.ToString();
}

    private string FormatCell(Cell cell)
    {
        if (cell.Aventurer != null)
            return $"A({cell.Aventurer.Name[0]})".PadRight(6);

        if (cell.IsMountain)
            return "M".PadRight(6);

        if (cell.NumberOfTreasures > 0)
            return $"T({cell.NumberOfTreasures})".PadRight(6);

        return ".".PadRight(6);
    }
}
