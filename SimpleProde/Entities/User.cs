using Microsoft.AspNetCore.Identity;

namespace SimpleProde.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
}
