using RitRestAPI.Models.DTOs.DrillBlockDTOs;

namespace RitRestAPI.Models.DTOs.DrillBlockPointsDTOs;

public record PostDrillBlockPointsDto(int HoleId, int Sequence, double X, double Y, double Z);