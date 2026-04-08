using System.ComponentModel.DataAnnotations;
namespace MusicJournal.Api.Models;

public class Track
{

    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Artist { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Album { get; set; }

    [MaxLength(100)]
    public string? Genre { get; set; }

    [MaxLength(500)]
    public string? SourceUrl { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public int? ReleaseYear { get; set; }

    public TrackStatus Status { get; set; } = TrackStatus.Interested; //"interested" | "listening" | "pass"
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    public DateTime? LastUpdated { get; set; }
}

public enum TrackStatus
{
    Interested, //Just found
    Listening, //Actively listening
    Pass, //nah
    Loved //its in
}