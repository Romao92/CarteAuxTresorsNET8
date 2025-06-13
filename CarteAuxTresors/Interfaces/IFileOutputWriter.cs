namespace CarteAuxTresors.Interfaces;

public interface IFileOutputWriter
{
    Task WriteAsync(string filePath, Progress Progress);
}
