using JKAPortal.API.Q3;

namespace JKAPortal.Model.Q3
{
    public class Q3Player : IQ3Player
    {
        public string Name { get { return Q3Utilities.GetUnformattedName(FormattedName); } }
        public string FormattedName { get; set; }
        public int Score { get; set; }
        public int Ping { get; set; }

        public Q3Player()
        {
            FormattedName = string.Empty;
            Score = 0;
            Ping = -1;
        }

        public Q3Player(string formattedName, int score, int ping)
        {
            FormattedName = formattedName;
            Score = score;
            Ping = ping;
        }
    }
}
