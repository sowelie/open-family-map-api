using OpenFamilyMapAPI.Core.Data;
using OpenFamilyMapAPI.Entities;

namespace OpenFamilyMapAPI.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(OpenFamilyMapContext context) : base(context)
    {
        
    }
    
    public User? FindByLogin(string login)
    {
        return _context.Users.FirstOrDefault(user => user.Login == login);
    }
}