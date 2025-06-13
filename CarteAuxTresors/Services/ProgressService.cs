using CarteAuxTresors.Utils;
using Microsoft.Extensions.Logging;

namespace CarteAuxTresors.Services;

public class ProgressService : IProgressService
{
    private readonly IFileInputParser _fileInputParser;
    private readonly IFileOutputWriter _fileOutputWriter;
    private readonly IMapVisualizer _mapVisualizer;
    private readonly IMovementService _MovementService;
    private readonly IOrientationService _orientationService = new OrientationService();

    private readonly ILogger<ProgressService> _logger;

    public ProgressService(
        IFileInputParser fileInputParser,
        IFileOutputWriter fileOutputWriter,
        IMapVisualizer mapVisualizer,
        IMovementService MovementService,
        IOrientationService orientationService,
        ILogger<ProgressService> logger)
    {
        _fileInputParser = fileInputParser;
        _fileOutputWriter = fileOutputWriter;
        _mapVisualizer = mapVisualizer;
        _MovementService = MovementService;
        _orientationService = orientationService;
        _logger = logger;
    }
    
    public async Task RunAsync(string inputPath, string outputPath)
    {
        LogHelper.Info("Reading input file: {Input}", inputPath);
        var Progress = _fileInputParser.Parse(inputPath);
         LogHelper.Info("🚩---- Starting map -----🚩:\n" + _mapVisualizer.DrawMap(Progress.Card, true));
        // exécution tour par tour
        for (int i = 0; i < Progress.MaxNumberOfTurns(); i++)
        {
            foreach (var Aventurer in Progress.Aventurers)
            {
                if (Aventurer.CanMoveAgain())
                {
                    var Movement = Aventurer.ExtractNextMove();

                    switch (Movement)
                    {
                        case Movement.A:
                            _MovementService.Move(Progress.Card, Aventurer);
                            break;
                        case Movement.D:
                            Aventurer.Orientation = _orientationService.TurnRight(Aventurer.Orientation);
                            break;
                        case Movement.G:
                            Aventurer.Orientation = _orientationService.TurnLeft(Aventurer.Orientation);
                            break;
                    }

                    LogHelper.Info("After movement {M} by {Aventurer}, new orientation {O}, actual position =({X},{Y})",
                        Movement, Aventurer.Name, Aventurer.Orientation, Aventurer.X, Aventurer.Y);

                    LogHelper.Info("Map after this turn :\n" + _mapVisualizer.DrawMap(Progress.Card, true));
                }
            }
        }

        LogHelper.Info("End of simulation. Writing the result in {Output}", outputPath);
        await _fileOutputWriter.WriteAsync(outputPath, Progress);
    }
}
