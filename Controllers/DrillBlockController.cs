using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RitRestAPI.Abstractions;
using RitRestAPI.Entities;
using RitRestAPI.Models.DTOs.DrillBlockDTOs;

namespace RitRestAPI.Controllers;

[ApiController, Route("api/[controller]/[action]")]
public class DrillBlockController : ControllerBase
{
   private readonly IGenericRepositoryService<DrillBlock> _repositoryService;

   public DrillBlockController(IGenericRepositoryService<DrillBlock> repositoryService)
   { _repositoryService = repositoryService; }

   [HttpGet("{offset:int}/{size:int}")]
   public async Task<ActionResult<IEnumerable<DrillBlockDto>>> GetAllAsync([FromRoute] int offset = 0, [FromRoute] int size = 100)
   {
       var drillBlocks = _repositoryService.GetAll(offset, size).OrderBy(a => a.Id);
       if (drillBlocks.Any())
           return await drillBlocks.ProjectToType<DrillBlockDto>().ToListAsync();
       
       return NotFound();
   }

   [HttpGet("{drillBlockId:int}")]
   public async Task<ActionResult<DrillBlockDto>> GetById([FromRoute] int drillBlockId)
   {
       var drillBlockById = await _repositoryService.GetById(drillBlockId, false);

       if (drillBlockById is null)
           return NotFound();

       return Ok(drillBlockById);
   }

   [HttpPost]
   public async Task<ActionResult<DrillBlockDto>> AddDrillBlock([FromBody] PostDrillBlockDto drillBlockDto)
   {
       var drillBlock = drillBlockDto.Adapt<DrillBlock>();
       
       if (!ModelState.IsValid) return StatusCode(400); // Bad Request
       
       var addedDrillBlock = _repositoryService.Add(drillBlock); 
       await _repositoryService.SaveAsync();

       var mappedDrillBlock = addedDrillBlock.Adapt<DrillBlockDto>();
       
       return Ok(mappedDrillBlock);
   }
   
   [HttpPut]
   public async Task<ActionResult> EditDrillBlock([FromBody] DrillBlockDto editDrillBlockDto)
   {
       var drillBlockById = await _repositoryService.GetById(editDrillBlockDto.Id, false);
       
       if (!ModelState.IsValid) return StatusCode(400); // Bad Request

       if (drillBlockById is null)
           return NotFound();

       var updatedDrillBlock = _repositoryService.Update(editDrillBlockDto.Adapt<DrillBlock>());
       await _repositoryService.SaveAsync();
       
       return Ok(updatedDrillBlock);
   }

   [HttpDelete("{drillBlockId:int}")]
   public async Task<ActionResult> DeleteDrillBlock(int drillBlockId)
   {
       var drillBlockById = await _repositoryService.GetById(drillBlockId, true);
       
       if (drillBlockById is null)
           return NotFound();
       
       await _repositoryService.DeleteAsync(drillBlockId);
       await _repositoryService.SaveAsync();
       return Ok();
   }
}