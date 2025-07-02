using SimpleProde.Entities;

namespace SimpleProde.DTOs
{
    public class ScoreDTO : IId
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ChampionshipId { get; set; }
        public int Points { get; set; }
    }
}
