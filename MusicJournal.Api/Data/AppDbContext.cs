using Microsoft.EntityFrameworkCore;
using MusicJournal.Api.Models;

namespace MusicJournal.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Track> Tracks => Set<Track>();
}