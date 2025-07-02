using static SimpleProde.Utilities.Enums;

namespace SimpleProde.Entities
{
    public interface IScoreable
    {
        int HomeTeamScore { get; }
        int AwayTeamScore { get; }
        MatchResult Result => HomeTeamScore > AwayTeamScore
            ? MatchResult.HomeTeamWin
            : HomeTeamScore == AwayTeamScore
                ? MatchResult.Tied
                : MatchResult.AwayTeamWin;
    }
}
