using System.Drawing;

namespace SimpleProde.Entities
{
    public class Team : IId
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public int? FifaRanking { get; set; }
        public string? NickName { get; set; }
        public string? Flag { get; set; }
        public string? ShirtColor { get; set; }
        public bool IsNationalTeam { get; set; }

        public virtual ICollection<Match> HomeMatches { get; set; }
        public virtual ICollection<Match> AwayMatches { get; set; }
    }
}
