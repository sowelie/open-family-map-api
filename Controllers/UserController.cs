using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpenFamilyMapAPI.DTO;
using OpenFamilyMapAPI.Repositories;

namespace OpenFamilyMapAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(UserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(
            (await _userRepository.GetAllAsync()).ConvertAll(u => _mapper.Map<UserDTO>(u)));
    }
}