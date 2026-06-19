namespace Honbul
{
    public static class GameState
    {
        public static int CluesCollected;
        public static int TotalClues;
        public static bool PuzzleSolved;

        public static void Reset()
        {
            CluesCollected = 0;
            TotalClues = 0;
            PuzzleSolved = false;
        }
    }
}
