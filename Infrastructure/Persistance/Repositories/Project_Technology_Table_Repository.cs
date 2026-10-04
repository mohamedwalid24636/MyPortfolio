using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Repositories
{
    public class Project_Technology_Table_Repository(StoreDbContext _DbContext) : IProject_Technology_Table_Repository
    {
        public async Task<IEnumerable<Project_Technology>> GetAllAsync() => await _DbContext.Set<Project_Technology>().ToListAsync();

        public async Task<Project_Technology?> GetByIdAsync(int id) => await _DbContext.Set<Project_Technology>().FindAsync(id);

        public async Task AddAsync(Project_Technology entity) => await _DbContext.Set<Project_Technology>().AddAsync(entity);

        public void Update(Project_Technology entity) => _DbContext.Set<Project_Technology>().Update(entity);

        public void Delete(Project_Technology entity) => _DbContext.Set<Project_Technology>().Remove(entity);
    }
}
