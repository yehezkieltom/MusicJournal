using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicJournal.Api.Data;
using MusicJournal.Api.Models;

namespace MusicJournal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    // Temporary in-memory list - EF Core + SQL Lite replaces this next
    private readonly AppDbContext _db;
    private readonly ILogger<TracksController> _logger;

    public TracksController(AppDbContext db, ILogger<TracksController> logger)
    {
        _db = db;
        _logger = logger;
        _logger.LogWarning("TEST - if you see this, logging works");

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Track>>> GetAll() 
        => Ok(await _db.Tracks.ToListAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Track?>> GetById(int id)
    {
        Track? track = await _db.Tracks.FindAsync(id);
        return track is null ? NotFound() : Ok(track);
    }

    [HttpPost]
    public async Task<ActionResult<Track>> Create([FromBody] CreateTrackDto dto)
    {
        _logger.LogInformation("Received: {@Dto}", dto);
        var track = new Track
        {
            Title = dto.Title,
            Artist = dto.Artist,
            Album = dto.Album,
            Genre = dto.Genre,
            SourceUrl = dto.SourceUrl,
            Notes = dto.Notes,
            ReleaseYear = dto.ReleaseYear,
            Status = dto.Status,
            DateAdded = DateTime.UtcNow,
        };
        
        _db.Tracks.Add(track);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = track.Id }, track);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Track updated)
    {
        var track = await _db.Tracks.FindAsync(id);
        if (track is null) return NotFound();

        track.Title = updated.Title;
        track.Artist = updated.Artist;
        track.Genre = updated.Genre;
        track.SourceUrl = updated.SourceUrl;
        track.Notes = updated.Notes;
        track.Status = updated.Status;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var track = await _db.Tracks.FindAsync(id);
        if (track is null) return NotFound();

        _db.Tracks.Remove(track);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}