namespace BP.Kingdoms.Core
{
    public sealed class PlaceMarker : IGameAction
    { 
        public ActionKind Kind => ActionKind.PlacePiece;
        public PlayerId By { get; }
        public Coord At { get; }
        public PlaceMarker(PlayerId by, Coord at) { By = by; At = at; }
    }
}
