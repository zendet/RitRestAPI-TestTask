using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RitRestAPI.Abstractions;
using RitRestAPI.Entities;
using RitRestAPI.Models.DTOs.HolePointsDTOs;

namespace RitRestAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class HolePointsController : ControllerBase
{
   private readonly IGenericRepositoryService<HolePoints> _repositoryService;
   private readonly IGenericRepositoryService<Hole> _holeRepositoryService;
   private readonly ISingleQueryService _singleQueryService;

   public HolePointsController(
       IGenericRepositoryService<HolePoints> repositoryService, 
       IGenericRepositoryService<Hole> holeRepositoryService,
       ISingleQueryService singleQueryService)
   {
       _repositoryService = repositoryService;
       _holeRepositoryService = holeRepositoryService;
       _singleQueryService = singleQueryService;
   }
   
   [HttpGet("{offset:int}/{size:int}")]
   public async Task<ActionResult<IEnumerable<HolePointsDto>>> GetAllAsync([FromRoute] int offset = 0,
       [FromRoute] int size = 100)
   {
       var holePoints = _repositoryService.GetAll(offset, size).OrderBy(a => a.Id);
       if (holePoints.Any())
           return await holePoints.ProjectToType<HolePointsDto>().ToListAsync();
       
       return NotFound();
   }
   
   [HttpGet("{holePointsId:int}")]
   public async Task<ActionResult<HolePointsDto>> GetById([FromRoute] int holePointsId)
   {
       var holePointsById = await _repositoryService.GetById(holePointsId, false);

       if (holePointsById is null)
           return NotFound();

       return Ok(holePointsById);
   }

   [HttpPost]
   public async Task<ActionResult<HolePointsDto>> AddHolePoints([FromBody] PostHolePointsDto holePointsDto)
   {
       var holePoints = holePointsDto.Adapt<HolePoints>();

       // var hole = await _holeRepositoryService.GetById(holePointsDto.HoleId, true);

       var hole = await _singleQueryService.GetHoleByIdWithDrillBlock(holePointsDto.HoleId, true);

       if (hole is null)
           return StatusCode(400); // Bad Request

       holePoints.Hole = hole;

       if (!ModelState.IsValid) return StatusCode(400); // Bad Request
       
       var addedHolePoints = _repositoryService.Add(holePoints); 
       await _repositoryService.SaveAsync();

       var mappedHolePoints = addedHolePoints.Adapt<HolePointsDto>(); // Yes, DrillBlocks is null
       
       return Ok(mappedHolePoints);
   }
   
   [HttpPut]
   public async Task<ActionResult> EditHolePoints([FromBody] HolePointsDto editHolePointsDto)
   {
       var holePointsById = await _repositoryService.GetById(editHolePointsDto.Id, false);
       
       if (!ModelState.IsValid) return StatusCode(400); // Bad Request

       if (holePointsById is null)
           return NotFound();

       var updateHolePoints = _repositoryService.Update(editHolePointsDto.Adapt<HolePoints>());
       await _repositoryService.SaveAsync();

       var mappedHolePoints = updateHolePoints.Adapt<HolePointsDto>();

       return Ok(mappedHolePoints);
   }

   [HttpDelete("{holePointsId:int}")]
   public async Task<ActionResult> DeleteDrillBlockPoints(int holePointsId)
   {
       var holePointsById = await _repositoryService.GetById(holePointsId, false);
       
       if (holePointsById is null)
           return NotFound();
       
       await _repositoryService.DeleteAsync(holePointsId);
       await _repositoryService.SaveAsync();
       return Ok();
   }
}