namespace SimpleProde.Entities
{
    public class Championship: IId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
    }
}
