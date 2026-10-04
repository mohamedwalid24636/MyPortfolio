using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts
{
    public interface IProject_Tag_Table_Repository
    {
        Task<IEnumerable<Project_Tag>> GetAllAsync();
        Task<Project_Tag?> GetByIdAsync(int id);
        Task AddAsync(Project_Tag entity);
        void Update(Project_Tag entity);
        void Delete(Project_Tag entity);
    }
}
