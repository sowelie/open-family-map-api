using OpenFamilyMapAPI.Core.Data;
using OpenFamilyMapAPI.Entities;

namespace OpenFamilyMapAPI.Repositories;

public class UserRepository(OpenFamilyMapContext context) : BaseRepository<User>(context)
{
    public User? FindByLogin(string login)
    {
        return _context.Users.FirstOrDefault(user => user.Login == login);
    }
}