using System;

namespace ACE.Server.Entity
{
    public static class ArenaRanking
    {
        public static float GetProbabilityWinning(float ratingPlayer1, float ratingPlayer2)
        {
            return 1f / (1f + MathF.Pow(10f, (ratingPlayer2 - ratingPlayer1) / 400f));
        }

        public static int GetRankChange(uint winnerCurrentRank, uint loserCurrentRank, int multiplier)
        {
            float probabilityWinPlayer1 = GetProbabilityWinning(winnerCurrentRank, loserCurrentRank);

            return Convert.ToInt32(Math.Round(multiplier * (1 - probabilityWinPlayer1)));
        }
    }
}
