namespace SimpleProde.Entities
{
    public class Score : IId
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ChampionshipId { get; set; }
        public int Points { get; set; }

        public Championship Championship { get; set; }
        public  User User { get; set; }
    }
}
