using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RitRestAPI.Abstractions;
using RitRestAPI.Entities;
using RitRestAPI.Models.DTOs.HoleDTOs;

namespace RitRestAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class HoleController : ControllerBase
{
   private readonly IGenericRepositoryService<Hole> _repositoryService;
   private readonly IGenericRepositoryService<DrillBlock> _drillRepositoryService;

   public HoleController(
       IGenericRepositoryService<Hole> repositoryService,
       IGenericRepositoryService<DrillBlock> drillRepositoryService)
   {
       _repositoryService = repositoryService;
       _drillRepositoryService = drillRepositoryService;
   }
   
   [HttpGet("{offset:int}/{size:int}")]
   public async Task<ActionResult<IEnumerable<HoleDto>>> GetAllAsync([FromRoute] int offset = 0,
       [FromRoute] int size = 100)
   {
       var holes = _repositoryService.GetAll(offset, size).OrderBy(a => a.Id);
       if (holes.Any())
           return await holes.ProjectToType<HoleDto>().ToListAsync();
       
       return NotFound();
   }
   
   [HttpGet("{holeId:int}")]
   public async Task<ActionResult<HoleDto>> GetById([FromRoute] int holeId)
   {
       var holeById = await _repositoryService.GetById(holeId, false);

       if (holeById is null)
           return NotFound();

       return Ok(holeById);
   }

   [HttpPost]
   public async Task<ActionResult<HoleDto>> AddHole([FromBody] PostHoleDto holeDto)
   {
       var hole = holeDto.Adapt<Hole>();

       var drillBlock = await _drillRepositoryService.GetById(holeDto.DrillBlockId, true);

       if (drillBlock is null)
           return StatusCode(400); // Bad Request

       hole.DrillBlock = drillBlock;
       
       if (!ModelState.IsValid) return StatusCode(400); // Bad Request
       
       var addedHole = _repositoryService.Add(hole); 
       await _repositoryService.SaveAsync();

       var mappedHole = addedHole.Adapt<HoleDto>();
       
       return Ok(mappedHole);
   }
   
   [HttpPut]
   public async Task<ActionResult> EditHole([FromBody] HoleDto editHoleDto)
   {
       var holeById = await _repositoryService.GetById(editHoleDto.Id, false);
       
       if (!ModelState.IsValid) return StatusCode(400); // Bad Request

       if (holeById is null)
           return NotFound();

       var updateHole = _repositoryService.Update(editHoleDto.Adapt<Hole>());
       await _repositoryService.SaveAsync();

       var mappedHole = updateHole.Adapt<HoleDto>();

       return Ok(mappedHole);
   }

   [HttpDelete("{holeId:int}")]
   public async Task<ActionResult> DeleteDrillBlockPoints(int holeId)
   {
       var holeById = await _repositoryService.GetById(holeId, false);
       
       if (holeById is null)
           return NotFound();
       
       await _repositoryService.DeleteAsync(holeId);
       await _repositoryService.SaveAsync();
       return Ok();
   }
}