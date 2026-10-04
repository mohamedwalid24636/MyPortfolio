using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

        IProject_Category_Table_Repository Project_Category_Table_Repository { get; }
        IProject_Tag_Table_Repository Project_Tag_Table_Repository { get; }
        IProject_Technology_Table_Repository Project_Technology_Table_Repository { get; }
        ISkill_Type_Table_Repository Skill_Type_Table_Repository { get; }

        Task<int> SaveChangesAsync();

        /// <summary>
        /// Starts a transaction so a service that needs more than one save for a single request can
        /// have them commit or fail together.
        /// </summary>
        Task<IUnitOfWorkTransaction> BeginTransactionAsync();
    }
}
