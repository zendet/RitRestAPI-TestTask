using System.ComponentModel.DataAnnotations;

namespace RitRestAPI.Models.DTOs.DrillBlockDTOs;

public record DrillBlockDto(int Id, string Name, [Required] DateTime UpdateDate);