using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpenFamilyMapAPI.DTO;
using OpenFamilyMapAPI.Entities;
using OpenFamilyMapAPI.Repositories;

namespace OpenFamilyMapAPI.Controllers;

[ApiController]
[Route("locationHistory")]
public class LcoationHistoryController(LocationDetailRepository repository, IMapper mapper) : ControllerBase
{
    private LocationDetailRepository _repository = repository;

    private readonly IMapper _mapper = mapper;

    [HttpPost("update")]
    public async Task<IActionResult> UpdateLocation([FromBody] LocationDetailDTO detail)
    {
        LocationDetail entity = _mapper.Map<LocationDetail>(detail);

        await _repository.UpdateAsync(entity);

        return Ok(_mapper.Map<LocationDetailDTO>(entity));
    }
}