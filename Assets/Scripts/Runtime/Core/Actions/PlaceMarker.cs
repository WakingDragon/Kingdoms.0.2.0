namespace BP.Kingdoms.Core
{
    public sealed class PlaceMarker : IGameAction
    { // Example atomic action for P1
        public PlayerId By { get; }
        public Coord At { get; }
        public PlaceMarker(PlayerId by, Coord at) { By = by; At = at; }
    }
}
