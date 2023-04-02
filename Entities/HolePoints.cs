using System.ComponentModel.DataAnnotations;

namespace RitRestAPI.Entities;

public class HolePoints
{
    public int Id { get; set; }
    [Required] public Hole Hole { get; set; } = default!;
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}