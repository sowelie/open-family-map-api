using System.Security.Claims;
using OpenFamilyMapAPI.Entities;
using OpenFamilyMapAPI.Repositories;

namespace OpenFamilyMapAPI.Services;

public class AuthService(
    IHttpContextAccessor httpContextAccessor,
    UserRepository userRepository
)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserRepository _userRepository = userRepository;

    public async Task<User> GetCurrentUser()
    {
        var context = _httpContextAccessor.HttpContext;

        // check to see if a user has already been cached
        var cachedUser = context!.Items["CurrentUser"] as User;
        if (cachedUser != null) return cachedUser;
        else if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            // get the user ID from the claims
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                // retrieve the user from the database
                // Note: You would typically inject a UserRepository or DbContext here to fetch the user
                // For demonstration, let's assume a method GetUserById exists
                var user = _userRepository.FindByLogin(userIdClaim.Value);

                // cache the user in the context for future requests
                context.Items["CurrentUser"] = user;

                return user ?? throw new BadHttpRequestException("Unauthorized", 401);
            }
        }

        throw new BadHttpRequestException("Unauthorized", 401);
    }
}