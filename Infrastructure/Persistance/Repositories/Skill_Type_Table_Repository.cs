using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Repositories
{
    public class Skill_Type_Table_Repository(StoreDbContext _DbContext) : ISkill_Type_Table_Repository
    {
        public async Task<IEnumerable<Skill_Type>> GetAllAsync() => await _DbContext.Set<Skill_Type>().ToListAsync();

        public async Task<Skill_Type?> GetByIdAsync(int id) => await _DbContext.Set<Skill_Type>().FindAsync(id);

        public async Task AddAsync(Skill_Type entity) => await _DbContext.Set<Skill_Type>().AddAsync(entity);

        public void Update(Skill_Type entity) => _DbContext.Set<Skill_Type>().Update(entity);

        public void Delete(Skill_Type entity) => _DbContext.Set<Skill_Type>().Remove(entity);
    }
}
