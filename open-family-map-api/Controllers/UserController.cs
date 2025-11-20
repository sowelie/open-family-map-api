using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenFamilyMapAPI.DTO;
using OpenFamilyMapAPI.Repositories;

namespace OpenFamilyMapAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class UserController(UserRepository userRepository, IMapper mapper) : ControllerBase
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        int pageNumber = 0, 
        int pageSize = 10,
        string sortBy = "DisplayName",
        bool sortDescending = false
    )
    {
        return Ok(
            (await _userRepository.GetPagedAsync(pageNumber, pageSize, sortBy, sortDescending))
                .Select(u => _mapper.Map<UserDTO>(u)));
    }
}