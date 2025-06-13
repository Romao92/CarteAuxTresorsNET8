using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using CarteAuxTresors.Utils;

var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..")); // racine du projet
var logDirectory = Path.Combine(rootPath, "logs");

if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

var logFileName = $"log-{DateTime.Now:ddMMyyyy-HHmmss}.txt";
var logFilePath = Path.Combine(logDirectory, logFileName);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(logFilePath)
    .CreateLogger();

try
{
    LogHelper.Info("=== Starting the console app ===");
    var builder = Host.CreateApplicationBuilder(args);

    // Services d'application / Injection de dépendances
    builder.Services.AddSingleton<IOrientationService, OrientationService>();
    builder.Services.AddSingleton<IMovementService, MovementService>();
    builder.Services.AddSingleton<IProgressService, ProgressService>();
    builder.Services.AddSingleton<IFileInputParser, FileInputParser>();
    builder.Services.AddSingleton<IFileOutputWriter, FileOutputWriter>();
    builder.Services.AddSingleton<IMapVisualizer, MapVisualizer>();

    builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

    var app = builder.Build();

    var Progress = app.Services.GetRequiredService<IProgressService>();
    // var inputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\InputFile");
    // var outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\OutputFile");
    var inputFolder = Path.Combine(rootPath, "InputFile");
    var outputFolder = Path.Combine(rootPath, "OutputFile");
    var inputFiles = Directory.GetFiles(inputFolder, "*.txt");

    foreach (var inputPath in inputFiles)
    {
        var fileName = Path.GetFileNameWithoutExtension(inputPath);
        var outputPath = Path.Combine(outputFolder, $"output-{fileName}-{DateTime.Now:ddMMyyyy-HHmmss}.txt");

        LogHelper.Info($"📄 Read the file: {inputPath}");
        await Progress.RunAsync(inputPath, outputPath);
        LogHelper.Success($"✅ Writing results in {outputPath}");
    }

    LogHelper.Info("=== Ending execution ===");
}
catch (Exception ex)
{
    LogHelper.Fatal("Unexpected error: {0}", ex);
}
finally
{
    Log.CloseAndFlush();
}