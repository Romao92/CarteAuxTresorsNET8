using CarteAuxTresors.Models;
using CarteAuxTresors.Models.Enums;
using CarteAuxTresors.Services;
using FluentAssertions;

namespace CarteAuxTresors.Tests;

public class ProgressTests
{
    [Fact]
    public void Aventurer_devrait_avancer_vers_le_sud()
    {
        // Arrange
        var Card = new Card(3, 3);
        var orientationService = new OrientationService();
        var MovementService = new MovementService(orientationService);
        var Aventurer = new Aventurer("Bob", 1, 1, Orientation.S, new List<Movement> { Movement.A });

        Card.GetCell(1, 1).Aventurer = Aventurer;

        // Act
        MovementService.Move(Card, Aventurer);

        // Assert
        Aventurer.X.Should().Be(1);
        Aventurer.Y.Should().Be(2);
        Card.GetCell(1, 2).Aventurer.Should().Be(Aventurer);
    }

    [Fact]
    public void Aventurer_devrait_tourner_a_droite()
    {
        // Arrange
        var orientationService = new OrientationService();
        var Aventurer = new Aventurer("Alice", 1, 1, Orientation.N, new List<Movement> { Movement.D });

        // Act
        var nouvelleOrientation = orientationService.TurnRight(Aventurer.Orientation);
        Aventurer.Orientation = nouvelleOrientation;

        // Assert
        Aventurer.Orientation.Should().Be(Orientation.E);
    }

    [Fact]
    public void Aventurer_devrait_tourner_a_gauche()
    {
        // Arrange
        var orientationService = new OrientationService();
        var Aventurer = new Aventurer("Alice", 1, 1, Orientation.N, new List<Movement> { Movement.G });

        // Act
        var nouvelleOrientation = orientationService.TurnLeft(Aventurer.Orientation);
        Aventurer.Orientation = nouvelleOrientation;

        // Assert
        Aventurer.Orientation.Should().Be(Orientation.O);
    }

    [Theory]
    [InlineData(Orientation.N, Orientation.E)]
    [InlineData(Orientation.E, Orientation.S)]
    [InlineData(Orientation.S, Orientation.O)]
    [InlineData(Orientation.O, Orientation.N)]
    public void Tourner_droite_doit_donner_bonne_orientation(Orientation initial, Orientation attendu)
    {
        var orientationService = new OrientationService();
        orientationService.TurnRight(initial).Should().Be(attendu);
    }

    [Fact]
    public void Aventurer_ne_doit_pas_avancer_si_montagne_devant()
    {
        // Arrange
        var Card = new Card(3, 3);
        var orientationService = new OrientationService();
        var MovementService = new MovementService(orientationService);

        // Position de l'Aventurer
        var Aventurer = new Aventurer("Tom", 1, 1, Orientation.N, new List<Movement> { Movement.A });

        // Position de la montagne (devant lui au nord)
        Card.GetCell(1, 0).IsMountain = true;

        Card.GetCell(1, 1).Aventurer = Aventurer;

        // Act
        MovementService.Move(Card, Aventurer);

        // Assert
        Aventurer.X.Should().Be(1);
        Aventurer.Y.Should().Be(1);
        Card.GetCell(1, 0).Aventurer.Should().BeNull();
        Card.GetCell(1, 1).Aventurer.Should().Be(Aventurer);
    }


    [Fact]
    public void Aventurer_ne_doit_pas_avancer_si_Cell_occupee_par_un_autre_Aventurer()
    {
        // Arrange
        var Card = new Card(3, 3);
        var orientationService = new OrientationService();
        var MovementService = new MovementService(orientationService);

        var tom = new Aventurer("Tom", 1, 1, Orientation.S, new List<Movement> { Movement.A });
        var bob = new Aventurer("Bob", 1, 2, Orientation.N, new List<Movement>());

        // Tom regarde vers le sud, où se trouve déjà Bob
        Card.GetCell(1, 1).Aventurer = tom;
        Card.GetCell(1, 2).Aventurer = bob;

        // Act
        MovementService.Move(Card, tom);

        // Assert
        tom.X.Should().Be(1);
        tom.Y.Should().Be(1); // Il ne bouge pas
        Card.GetCell(1, 2).Aventurer.Should().Be(bob);
        Card.GetCell(1, 1).Aventurer.Should().Be(tom);
    }

    [Fact]
    public void Aventurer_doit_ramasser_un_tresor()
    {
        // Arrange
        var Card = new Card(3, 3);
        var orientationService = new OrientationService();
        var MovementService = new MovementService(orientationService);

        var Aventurer = new Aventurer("Lara", 1, 1, Orientation.S, new List<Movement> { Movement.A });

        // Place un trésor sur la Cell en dessous
        var CellTresor = Card.GetCell(1, 2);
        CellTresor.SetTresors(1); // méthode d'encapsulation que tu avais ajoutée

        Card.GetCell(1, 1).Aventurer = Aventurer;

        // Act
        MovementService.Move(Card, Aventurer);

        // Assert
        Aventurer.X.Should().Be(1);
        Aventurer.Y.Should().Be(2);
        Aventurer.RecoveredTreasures.Should().Be(1);
        CellTresor.NumberOfTreasures.Should().Be(0);
    }

    [Fact]
    public void Aventurer_doit_tourner_et_ramasser_un_tresor_apres_deplacement()
    {
        // Arrange
        var Card = new Card(3, 3);
        var orientationService = new OrientationService();
        var MovementService = new MovementService(orientationService);

        // Lara commence tournée vers le nord, mais devra aller à l'est
        var lara = new Aventurer("Lara", 1, 1, Orientation.N, new List<Movement> { Movement.D, Movement.A });

        // Met un trésor à l'est
        var CellTresor = Card.GetCell(2, 1);
        CellTresor.SetTresors(1);

        Card.GetCell(1, 1).Aventurer = lara;

        // Act
        // Movement 1 : tourner à droite
        lara.Orientation = orientationService.TurnRight(lara.Orientation);

        // Movement 2 : avancer
        MovementService.Move(Card, lara);

        // Assert
        lara.X.Should().Be(2);
        lara.Y.Should().Be(1);
        lara.Orientation.Should().Be(Orientation.E);
        lara.RecoveredTreasures.Should().Be(1);
        CellTresor.NumberOfTreasures.Should().Be(0);
    }

    [Fact]
    public void Quatre_rotations_droite_ramene_a_l_orientation_initiale()
    {
        // Arrange
        var orientationService = new OrientationService();
        var orientationInitiale = Orientation.N;
        var orientation = orientationInitiale;

        // Act
        for (int i = 0; i < 4; i++)
        {
            orientation = orientationService.TurnRight(orientation);
        }

        // Assert
        orientation.Should().Be(orientationInitiale);
    }

    [Fact]
    public void Aventurer_bloque_ne_bouge_pas()
    {
        // Arrange
        var Card = new Card(3, 3);
        var orientationService = new OrientationService();
        var MovementService = new MovementService(orientationService);

        var lara = new Aventurer("Lara", 1, 1, Orientation.N, new List<Movement> { Movement.A });

        // Cell devant = montagne
        Card.GetCell(1, 0).IsMountain = true;

        // Autres directions aussi bloquées :
        Card.GetCell(0, 1).IsMountain = true; // ouest
        Card.GetCell(2, 1).Aventurer = new Aventurer("Bob", 2, 1, Orientation.O, new List<Movement>()); // est
        Card.GetCell(1, 2).IsMountain = true; // sud

        Card.GetCell(1, 1).Aventurer = lara;

        // Act
        MovementService.Move(Card, lara); // tente vers le nord

        // Assert
        lara.X.Should().Be(1);
        lara.Y.Should().Be(1);
        Card.GetCell(1, 1).Aventurer.Should().Be(lara);
    }

    [Fact]
    public void Simulation_complete_multi_Aventurers()
    {
        // Arrange
        var Card = new Card(4, 4);
        var orientationService = new OrientationService();
        var MovementService = new MovementService(orientationService);

        var lara = new Aventurer("Lara", 1, 1, Orientation.S, new List<Movement> { Movement.A, Movement.A });
        var bob = new Aventurer("Bob", 0, 0, Orientation.E, new List<Movement> { Movement.A });
        var eve = new Aventurer("Eve", 3, 3, Orientation.N, new List<Movement> { Movement.A });

        Card.GetCell(3, 0).SetTresors(1);     // en haut droite
        Card.GetCell(1, 3).SetTresors(2);     // bas centre

        Card.GetCell(1, 1).Aventurer = lara;
        Card.GetCell(0, 0).Aventurer = bob;
        Card.GetCell(3, 3).Aventurer = eve;

        var Aventurers = new List<Aventurer> { lara, bob, eve };

        // Simuler chaque tour (2 max ici)
        for (int i = 0; i < 2; i++)
        {
            foreach (var a in Aventurers)
            {
                if (a.RemainingMovements.Any())
                {
                    var Movement = a.ExtractNextMove();

                    if (Movement == Movement.A)
                        MovementService.Move(Card, a);
                    else if (Movement == Movement.D)
                        a.Orientation = orientationService.TurnRight(a.Orientation);
                    else if (Movement == Movement.G)
                        a.Orientation = orientationService.TurnLeft(a.Orientation);
                }
            }
        }

        // Assert positions finales
        lara.X.Should().Be(1);
        lara.Y.Should().Be(3);
        lara.RecoveredTreasures.Should().Be(1);

        bob.X.Should().Be(1);
        bob.Y.Should().Be(0);
        bob.RecoveredTreasures.Should().Be(0);

        eve.X.Should().Be(3);
        eve.Y.Should().Be(2);
        eve.RecoveredTreasures.Should().Be(0);

        Card.GetCell(1, 3).NumberOfTreasures.Should().Be(1);
    }

}
