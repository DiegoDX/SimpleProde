namespace SimpleProde.DTOs
{
    public class ChampionshipCreateDTO
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public IFormFile? Icon { get; set; }
    }
}
