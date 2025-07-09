using LifeRoadmap.Application.Interfaces.Persistence;
using LifeRoadmap.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeRoadmap.Infrastructure.Persistence
{
    public class UnitOfwork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfwork(AppDbContext context)
        { 
            _context = context;
        }

        public async Task<bool> SaveAsync()
        { 
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
