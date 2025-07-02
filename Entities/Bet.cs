using static SimpleProde.Utilities.Enums;

namespace SimpleProde.Entities
{
    public class Bet : IScoreable, IId
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MatchId { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
        public virtual Match Match { get; set; }
        public virtual User User { get; set; }

    }
}
