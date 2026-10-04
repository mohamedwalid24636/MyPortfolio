using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts
{
    public interface IProject_Category_Table_Repository
    {
        Task<IEnumerable<Project_Category>> GetAllAsync();
        Task<Project_Category?> GetByIdAsync(int id);
        Task AddAsync(Project_Category entity);
        void Update(Project_Category entity);
        void Delete(Project_Category entity);
    }
}
