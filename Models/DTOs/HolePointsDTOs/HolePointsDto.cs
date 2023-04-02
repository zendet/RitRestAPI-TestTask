using RitRestAPI.Models.DTOs.HoleDTOs;

namespace RitRestAPI.Models.DTOs.HolePointsDTOs;

public record HolePointsDto(int Id, HoleDto Hole, double X, double Y, double Z);