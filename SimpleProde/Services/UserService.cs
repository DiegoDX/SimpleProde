using Microsoft.AspNetCore.Identity;

namespace SimpleProde.Services
{
    public class UserService : IUserService
    {
        public IHttpContextAccessor httpContextAccessor { get; }
        public UserManager<Entities.User> userManager { get; }

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<Entities.User> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<string> GetUserId()
        {
            var email = httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "email")!.Value;
            var user = await userManager.FindByNameAsync(email);
            return user!.Id;
        }

    }
}
