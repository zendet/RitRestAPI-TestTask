using System.ComponentModel.DataAnnotations;

namespace RitRestAPI.Entities;

public class Hole
{
    public int Id { get; set; }
    [Required, MaxLength(30)] public string Name { get; set; } = default!;
    [Required] public DrillBlock DrillBlock { get; set; } = default!;
    [Required] public int Depth { get; set; }
}