namespace SimpleProde.DTOs
{
    public class MatchCreateDTO
    {
        public int ChampionshipId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamScore { get; set; }
        public int AwayTeamScore { get; set; }
    }
}
