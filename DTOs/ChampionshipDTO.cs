using SimpleProde.Entities;

namespace SimpleProde.DTOs
{
    public class ChampionshipDTO : IId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Icon { get; set; }
    }
}