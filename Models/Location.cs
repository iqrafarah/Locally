using System.ComponentModel.DataAnnotations;

namespace Locally.Models;

public class Location
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    public string? Website { get; set; }

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; } = string.Empty;

    public decimal? PriceFrom { get; set; }

    public decimal? PriceTo { get; set; }

    public string? Description { get; set; }

    public LocationStatus Status { get; set; } = LocationStatus.Pending;

    public string UserId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<string> ImagePaths { get; set; } = new();
}