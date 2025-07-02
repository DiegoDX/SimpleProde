using SimpleProde.Entities;

namespace SimpleProde.DTOs
{
    public class TeamDTO : IId
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public int? FifaRanking { get; set; }
        public string? NickName { get; set; }
        public string? Flag { get; set; }
        public string? ShirtColor { get; set; }
    }
}
