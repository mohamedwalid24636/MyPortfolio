using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts
{
    public interface IProject_Technology_Table_Repository
    {
        Task<IEnumerable<Project_Technology>> GetAllAsync();
        Task<Project_Technology?> GetByIdAsync(int id);
        Task AddAsync(Project_Technology entity);
        void Update(Project_Technology entity);
        void Delete(Project_Technology entity);
    }
}
