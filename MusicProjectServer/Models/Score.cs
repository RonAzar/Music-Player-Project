namespace MusicProjectServer.Models
{
    public class Score
    {
        private int id;
        private int userScore;
        private int userId;
        private string userName;

        public Score(int id, int userScore, int userId, string userName)
        {
            Id = id;
            UserScore = userScore;
            UserId = userId;
            UserName = userName;
        }

        public Score()
        {
            Id = 0;
            UserScore = 0;
            UserId = 0;
            UserName = "";
        }

        public int Id { get => id; set => id = value; }
        public int UserScore { get => userScore; set => userScore = value; }
        public int UserId { get => userId; set => userId = value; }
        public string UserName { get => userName; set => userName = value; }

        static DBservices dBservices = new DBservices();

        public static List<Score> GetUserScores(int userId)
        {
            return dBservices.GetUserScores(userId);
        }
        public static List<Score> GetTopFiveScoreBoard()
        {
            return dBservices.GetTopFiveScoreBoard();
        }

        public static bool InsertScore(Score score)
        {
            return dBservices.InsertScore(score);
        }

        public static bool ResetScoreBoard()
        {
            return dBservices.ResetScoreBoard();
        }
    }
}
