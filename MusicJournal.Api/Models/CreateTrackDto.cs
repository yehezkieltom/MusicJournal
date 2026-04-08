using System.ComponentModel.DataAnnotations;
using MusicJournal.Api.Models;

public class CreateTrackDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Artist { get; set; } = string.Empty;

    public string? Album { get; set; }
    public string? Genre { get; set; }
    public string? SourceUrl { get; set; }
    public string? Notes { get; set; }
    public int? ReleaseYear { get; set; }
    public TrackStatus Status { get; set; } = TrackStatus.Interested;


}