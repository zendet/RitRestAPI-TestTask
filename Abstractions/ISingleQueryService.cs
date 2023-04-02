using RitRestAPI.Entities;

namespace RitRestAPI.Abstractions;

/// <summary>
/// It's worst and goofy interface, nevermind
/// </summary>
public interface ISingleQueryService
{
    Task<Hole?> GetHoleByIdWithDrillBlock(int holeId, bool wantToTrack = false);
}