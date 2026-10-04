using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Persistance.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistance.Repositories
{
    public class UnitOfWork(StoreDbContext _DbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];

        private IProject_Category_Table_Repository? _projectCategoryRepository;
        private IProject_Tag_Table_Repository? _projectTagRepository;
        private IProject_Technology_Table_Repository? _projectTechnologyRepository;
        private ISkill_Type_Table_Repository? _skillTypeRepository;

        public IProject_Category_Table_Repository Project_Category_Table_Repository
            => _projectCategoryRepository ??= new Project_Category_Table_Repository(_DbContext);

        public IProject_Tag_Table_Repository Project_Tag_Table_Repository
            => _projectTagRepository ??= new Project_Tag_Table_Repository(_DbContext);

        public IProject_Technology_Table_Repository Project_Technology_Table_Repository
            => _projectTechnologyRepository ??= new Project_Technology_Table_Repository(_DbContext);

        public ISkill_Type_Table_Repository Skill_Type_Table_Repository
            => _skillTypeRepository ??= new Skill_Type_Table_Repository(_DbContext);

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;

            if (_repositories.TryGetValue(typeName, out var repository))
                return (IGenericRepository<TEntity, TKey>)repository;

            var newRepository = new GenericRepository<TEntity, TKey>(_DbContext);
            _repositories[typeName] = newRepository;

            return newRepository;
        }

        public Task<int> SaveChangesAsync() => _DbContext.SaveChangesAsync();

        public async Task<IUnitOfWorkTransaction> BeginTransactionAsync()
            => new DbContextTransaction(await _DbContext.Database.BeginTransactionAsync());

        /// <summary>Keeps <see cref="IDbContextTransaction"/> out of the contracts layer.</summary>
        private sealed class DbContextTransaction(IDbContextTransaction transaction) : IUnitOfWorkTransaction
        {
            public Task CommitAsync() => transaction.CommitAsync();

            public Task RollbackAsync() => transaction.RollbackAsync();

            public ValueTask DisposeAsync() => transaction.DisposeAsync();
        }
    }
}
