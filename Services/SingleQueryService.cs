using Microsoft.EntityFrameworkCore;
using RitRestAPI.Abstractions;
using RitRestAPI.Entities;

namespace RitRestAPI.Services;

/// <summary>
/// This class is temporarily solution. This isn't best practice
/// </summary>
public class SingleQueryService : ISingleQueryService
{
    private readonly RitRestDbContext _dbContext;

    public SingleQueryService(
        RitRestDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Hole?> GetHoleByIdWithDrillBlock(int holeId, bool wantToTrack = false)
    {
        return await _dbContext.Holes.Include(hole => hole.DrillBlock).FirstOrDefaultAsync(hole => hole.Id == holeId);
    }
}