using System.ComponentModel.DataAnnotations;

namespace StudyRoomBooking.Mvc.DTOs;

public sealed class CreateBookingRequest : IValidatableObject
{
    [Range(1, int.MaxValue)]
    public int RoomId { get; init; }

    [Required]
    public DateTime? StartTime { get; init; }

    [Required]
    public DateTime? EndTime { get; init; }

    [Required]
    [StringLength(50)]
    public string Course { get; init; } = string.Empty;

    [StringLength(500)]
    public string Description { get; init; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartTime.HasValue &&
            EndTime.HasValue &&
            StartTime.Value >= EndTime.Value)
        {
            yield return new ValidationResult(
                "EndTime must be later than StartTime.",
                [nameof(EndTime)]);
        }
    }
}
