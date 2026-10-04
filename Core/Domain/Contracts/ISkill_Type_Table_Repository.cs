using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts
{
    public interface ISkill_Type_Table_Repository
    {
        Task<IEnumerable<Skill_Type>> GetAllAsync();
        Task<Skill_Type?> GetByIdAsync(int id);
        Task AddAsync(Skill_Type entity);
        void Update(Skill_Type entity);
        void Delete(Skill_Type entity);
    }
}
