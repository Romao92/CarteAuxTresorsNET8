namespace CarteAuxTresors.Models
{
    public class Progress
    {
        public Card Card { get; }
        public List<Aventurer> Aventurers { get; }

        public Progress(Card card, List<Aventurer> aventurers)
        {
            Card = card;
            Aventurers = aventurers;
        }

        public int MaxNumberOfTurns()
        {
            return Aventurers.Max(a => a.RemainingMovements.Count);
        }
    }
}
