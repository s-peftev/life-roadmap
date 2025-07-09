using LifeRoadmap.Application.Interfaces.Persistence;
using LifeRoadmap.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeRoadmap.Infrastructure.Persistence
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext context)
        { 
            _context = context;
            dbSet = context.Set<T>();
        }
    }
}
