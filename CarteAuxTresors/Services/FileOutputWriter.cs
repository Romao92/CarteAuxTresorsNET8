namespace CarteAuxTresors.Services;

public class FileOutputWriter : IFileOutputWriter
{
    private readonly IMapVisualizer _mapVisualizer;
    public FileOutputWriter(IMapVisualizer mapVisualizer)
    {
        _mapVisualizer = mapVisualizer;
    }
    public async Task WriteAsync(string path, Progress Progress)
    {
       var outputDir = Path.GetDirectoryName(path);
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir!);

        using var writer = new StreamWriter(path, false);

        await writer.WriteLineAsync($"C - {Progress.Card.Width} - {Progress.Card.Height}");

        foreach (var montagne in Progress.Card.GetMountainCells())
            await writer.WriteLineAsync($"M - {montagne.X} - {montagne.Y}");

        foreach (var tresor in Progress.Card.GetTreasureCells())
            await writer.WriteLineAsync($"T - {tresor.X} - {tresor.Y} - {tresor.NumberOfTreasures}");

        foreach (var Aventurer in Progress.Aventurers)
        {
            await writer.WriteLineAsync(
                $"A - {Aventurer.Name} - {Aventurer.X} - {Aventurer.Y} - {Aventurer.Orientation} - {Aventurer.RecoveredTreasures}");
        }

        await writer.WriteLineAsync();
        await writer.WriteLineAsync("🚩---- Final map -----🚩");
        await writer.WriteLineAsync(_mapVisualizer.DrawMap(Progress.Card, true));
    }
}
