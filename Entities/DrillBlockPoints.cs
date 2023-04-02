using System.ComponentModel.DataAnnotations;

namespace RitRestAPI.Entities;

public class DrillBlockPoints
{
    public int Id { get; set; }
    [Required] public DrillBlock DrillBlock { get; set; } = default!;
    public int Sequence { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}