using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RitRestAPI.Abstractions;
using RitRestAPI.Entities;
using RitRestAPI.Models.DTOs.DrillBlockPointsDTOs;

namespace RitRestAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class DrillBlockPointsController : ControllerBase
{
   private readonly IGenericRepositoryService<DrillBlockPoints> _repositoryService;
   private readonly IGenericRepositoryService<DrillBlock> _drillRepositoryService;

   public DrillBlockPointsController(
       IGenericRepositoryService<DrillBlockPoints> repositoryService,
       IGenericRepositoryService<DrillBlock> drillRepositoryService)
   {
       _repositoryService = repositoryService;
       _drillRepositoryService = drillRepositoryService;
   }
   
   [HttpGet("{offset:int}/{size:int}")]
   public async Task<ActionResult<IEnumerable<DrillBlockPointsDto>>> GetAllAsync([FromRoute] int offset = 0,
       [FromRoute] int size = 100)
   {
       var drillBlocksPoints = _repositoryService.GetAll(offset, size).OrderBy(a => a.Id);
       if (drillBlocksPoints.Any())
           return await drillBlocksPoints.ProjectToType<DrillBlockPointsDto>().ToListAsync();
       
       return NotFound();
   }
   
   [HttpGet("{drillBlockPointsId:int}")]
   public async Task<ActionResult<DrillBlockPointsDto>> GetById([FromRoute] int drillBlockPointsId)
   {
       var drillBlockPointsById = await _repositoryService.GetById(drillBlockPointsId, false);

       if (drillBlockPointsById is null)
           return NotFound();

       return Ok(drillBlockPointsById);
   }

   [HttpPost]
   public async Task<ActionResult<DrillBlockPointsDto>> AddDrillBlockPoints([FromBody] PostDrillBlockPointsDto drillBlockPointsDto)
   {
       var drillBlockPoints = drillBlockPointsDto.Adapt<DrillBlockPoints>();

       var drillBlock = await _drillRepositoryService.GetById(drillBlockPointsDto.HoleId, true);

       if (drillBlock is null)
           return StatusCode(400); // Bad Request

       drillBlockPoints.DrillBlock = drillBlock;
       
       if (!ModelState.IsValid) return StatusCode(400); // Bad Request
       
       var addedDrillBlockPoints = _repositoryService.Add(drillBlockPoints); 
       await _repositoryService.SaveAsync();

       var mappedDrillBlockPoints = addedDrillBlockPoints.Adapt<DrillBlockPointsDto>();
       
       return Ok(mappedDrillBlockPoints); // Yes, return of domain-model. It's very bad.
   }
   
   [HttpPut]
   public async Task<ActionResult> EditDrillBlockPoints([FromBody] DrillBlockPointsDto editDrillBlockPointsDto)
   {
       var drillBlockPointsById = await _repositoryService.GetById(editDrillBlockPointsDto.Id, false);
       
       if (!ModelState.IsValid) return StatusCode(400); // Bad Request

       if (drillBlockPointsById is null)
           return NotFound();

       var updatedDrillBlockPoints = _repositoryService.Update(editDrillBlockPointsDto.Adapt<DrillBlockPoints>());
       await _repositoryService.SaveAsync();

       return Ok(updatedDrillBlockPoints);
   }

   [HttpDelete("{drillBlockPointsId:int}")]
   public async Task<ActionResult> DeleteDrillBlockPoints(int drillBlockPointsId)
   {
       var drillBlockPointsById = await _repositoryService.GetById(drillBlockPointsId, false);
       
       if (drillBlockPointsById is null)
           return NotFound();
       
       await _repositoryService.DeleteAsync(drillBlockPointsId);
       await _repositoryService.SaveAsync();
       return Ok();
   }
}