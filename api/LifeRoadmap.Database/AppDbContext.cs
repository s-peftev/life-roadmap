using Microsoft.EntityFrameworkCore;

namespace LifeRoadmap.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) 
        : DbContext(options)
    {
    }
}
