using RitRestAPI.Models.DTOs.DrillBlockDTOs;

namespace RitRestAPI.Models.DTOs.HoleDTOs;

public record HoleDto(int Id, string Name, DrillBlockDto DrillBlock, int Depth);