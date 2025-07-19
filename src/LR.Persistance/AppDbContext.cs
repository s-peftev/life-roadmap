using Microsoft.EntityFrameworkCore;

namespace LR.Persistance
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) 
        : DbContext(options)
    {
    }
}
