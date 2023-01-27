using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domian;

namespace NZWalks.Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public UserRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await nZWalksDbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower() && 
            x.Password.ToLower() == password.ToLower());

            if(user == null)
            {
                return null;
            }

            var user_Roles = await nZWalksDbContext.User_Roles.Where(x => x.UserId == user.Id).ToListAsync();

            if(user_Roles.Any())
            {
                user.Roles = new List<string>();
                foreach(var user_Role in user_Roles)
                {
                    var role = await nZWalksDbContext.Roles.FirstOrDefaultAsync(x => x.Id == user_Role.RoleId);
                    if(role != null)
                    {
                        user.Roles.Add(role.Name);
                    }
                }
            }
            user.Password = null;

            return user;
        }

    }
}
