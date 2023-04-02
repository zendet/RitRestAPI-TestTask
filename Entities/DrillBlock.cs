using System.ComponentModel.DataAnnotations;

namespace RitRestAPI.Entities;

public class DrillBlock
{
    public int Id { get; set; }
    [Required, MaxLength(30)] public string Name { get; set; } = default!;
    [Required] public DateTime UpdateDate { get; set; }
}