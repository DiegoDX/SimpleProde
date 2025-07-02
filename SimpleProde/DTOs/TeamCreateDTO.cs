namespace SimpleProde.DTOs
{
    public class TeamCreateDTO
    {
        public string? Name { get; set; }
        public string? Country { get; set; }
        public int? FifaRanking { get; set; }
        public string? NickName { get; set; }
        public IFormFile? Flag { get; set; }
        public IFormFile? ShirtColor { get; set; }
    }
}
