using static SimpleProde.Utilities.Enums;

namespace SimpleProde.Entities
{
    public class Match : IScoreable, IId
    {
        public int Id { get; set; }
        public int ChampionshipId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }

        public Championship Championship { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }

    }
}
