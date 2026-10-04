using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Repositories
{
    public class Project_Category_Table_Repository(StoreDbContext _DbContext) : IProject_Category_Table_Repository
    {
        public async Task<IEnumerable<Project_Category>> GetAllAsync() => await _DbContext.Set<Project_Category>().ToListAsync();

        public async Task<Project_Category?> GetByIdAsync(int id) => await _DbContext.Set<Project_Category>().FindAsync(id);

        public async Task AddAsync(Project_Category entity) => await _DbContext.Set<Project_Category>().AddAsync(entity);

        public void Update(Project_Category entity) => _DbContext.Set<Project_Category>().Update(entity);

        public void Delete(Project_Category entity) => _DbContext.Set<Project_Category>().Remove(entity);
    }
}
