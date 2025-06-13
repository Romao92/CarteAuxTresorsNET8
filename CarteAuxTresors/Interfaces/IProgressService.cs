namespace CarteAuxTresors.Interfaces;

public interface IProgressService
{
    Task RunAsync(string inputPath, string outputPath);
}
