namespace BP.Kingdoms.Core
{
    public static class GameInitializer
    {
        public static GameState Create(int seed)
        {
            var rng = new System.Random(seed);
            var first = rng.Next(0, 2) == 0 ? PlayerId.P1 : PlayerId.P2;

            var state = new GameState();
            state.FirstTurn(first);

            state.Seed = seed;
            return state;
        }
    }
}
