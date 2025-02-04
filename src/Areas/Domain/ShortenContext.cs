using Microsoft.EntityFrameworkCore;
using Shorten.Areas.Domain; 

namespace Shorten.Areas.Domain;

public class ShortenContext : DbContext
{
    public ShortenContext(DbContextOptions<ShortenContext> options) : base(options)
    {
    }

    public DbSet<UrlMapping> UrlMappings { get; set; } 
}
