namespace CarteAuxTresors.Services;

public class FileInputParser : IFileInputParser
{
    public Progress Parse(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        Card? Card = null;
        var Aventurers = new List<Aventurer>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            var parts = line.Split(" - ").Select(p => p.Trim()).ToArray();
            switch (parts[0])
            {
                case "C":
                    var largeur = int.Parse(parts[1]);
                    var hauteur = int.Parse(parts[2]);
                    Card = new Card(largeur, hauteur);
                    break;

                case "M":
                    int mx = int.Parse(parts[1]);
                    int my = int.Parse(parts[2]);
                    if (Card != null)
                    {
                        Card.GetCell(mx, my).IsMountain = true;
                    }
                    break;

                case "T":
                    int tx = int.Parse(parts[1]);
                    int ty = int.Parse(parts[2]);
                    int nbTresors = int.Parse(parts[3]);
                    Card?.GetCell(tx, ty).SetTresors(nbTresors);
                    break;

                case "A":
                    string nom = parts[1];
                    int ax = int.Parse(parts[2]);
                    int ay = int.Parse(parts[3]);
                    Orientation orientation = Enum.Parse<Orientation>(parts[4]);
                    var Movements = parts[5]
                        .ToCharArray()
                        .Select(c => Enum.Parse<Movement>(c.ToString()))
                        .ToList();

                    var Aventurer = new Aventurer(nom, ax, ay, orientation, Movements);
                    Aventurers.Add(Aventurer);

                    if (Card != null)
                    {
                        Card.GetCell(ax, ay).Aventurer = Aventurer;
                    }
                    break;
            }
        }

        if (Card == null)
        {
            LogHelper.Error("No map defined in input file.");
            throw new InvalidOperationException("No map defined in input file.");
        }
        return new Progress(Card, Aventurers);
    }
}
