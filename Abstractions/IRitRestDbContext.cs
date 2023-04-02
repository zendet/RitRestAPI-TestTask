using Microsoft.EntityFrameworkCore;
using RitRestAPI.Entities;

namespace RitRestAPI.Abstractions;

public interface IRitRestDbContext
{
    public DbSet<DrillBlock> DrillBlocks { get; }
    public DbSet<DrillBlockPoints> DrillBlockPoints { get; }
    public DbSet<Hole> Holes { get; }
    public DbSet<HolePoints> HolePoints { get; }
}