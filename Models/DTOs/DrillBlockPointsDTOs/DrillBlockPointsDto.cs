using RitRestAPI.Models.DTOs.DrillBlockDTOs;

namespace RitRestAPI.Models.DTOs.DrillBlockPointsDTOs;

public record DrillBlockPointsDto(int Id, DrillBlockDto DrillBlock, int Sequence, double X, double Y, double Z);