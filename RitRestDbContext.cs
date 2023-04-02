using Microsoft.EntityFrameworkCore;
using RitRestAPI.Abstractions;
using RitRestAPI.Entities;

namespace RitRestAPI;

public sealed class RitRestDbContext : DbContext, IRitRestDbContext
{
    public DbSet<DrillBlock> DrillBlocks => Set<DrillBlock>();
    public DbSet<DrillBlockPoints> DrillBlockPoints => Set<DrillBlockPoints>();
    public DbSet<Hole> Holes => Set<Hole>();
    public DbSet<HolePoints> HolePoints => Set<HolePoints>();

    public RitRestDbContext(DbContextOptions<RitRestDbContext> options) : base(options)
    { Database.EnsureCreated(); }
}