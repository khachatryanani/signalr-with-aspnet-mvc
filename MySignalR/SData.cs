namespace MySignalR
{
    public static class SData
    {
        public const string Cloak = "cloak";
        public const string Wand = "wand";
        public const string Stone = "stone";

        public static Dictionary<string, int> Voting;

        static SData()
        {
            Voting = new Dictionary<string, int>();

            Voting.Add(Cloak, 0);
            Voting.Add(Wand, 0);
            Voting.Add(Stone, 0);
        }
    }
}
