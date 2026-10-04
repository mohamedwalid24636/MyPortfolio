using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Repositories
{
    public class Project_Tag_Table_Repository(StoreDbContext _DbContext) : IProject_Tag_Table_Repository
    {
        public async Task<IEnumerable<Project_Tag>> GetAllAsync() => await _DbContext.Set<Project_Tag>().ToListAsync();

        public async Task<Project_Tag?> GetByIdAsync(int id) => await _DbContext.Set<Project_Tag>().FindAsync(id);

        public async Task AddAsync(Project_Tag entity) => await _DbContext.Set<Project_Tag>().AddAsync(entity);

        public void Update(Project_Tag entity) => _DbContext.Set<Project_Tag>().Update(entity);

        public void Delete(Project_Tag entity) => _DbContext.Set<Project_Tag>().Remove(entity);
    }
}
