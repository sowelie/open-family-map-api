using System.Threading.Tasks;
using AutoMapper;
using OpenFamilyMapAPI.Entities;
using OpenFamilyMapAPI.Repositories;

public class UserResolver<TSource, TDest>(UserRepository userRepository) : IMemberValueResolver<TSource, TDest, int, User>
{
    private UserRepository _userRepository = userRepository;

    public User Resolve(TSource source, TDest destination, int sourceMember, User destMember, ResolutionContext context)
    {
        return _userRepository.GetById(sourceMember)
            ?? throw new InvalidOperationException($"The specified userId {sourceMember} is invalid.");
    }
}