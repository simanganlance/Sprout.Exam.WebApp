using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.WebApp.Data;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Threading.Tasks;
using Sprout.Exam.DataAccess.Repository.Interfaces;
using Sprout.Exam.DataAccess.Repository.Models;

namespace Sprout.Exam.WebApp.DataAccessWrapper
{
    public class DbContextWrapper : IDbContextWrapper
    {
        private readonly ApplicationDbContext _dbContext;
        
        public DbContextWrapper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public DbSet<Employee> Employee => _dbContext.Employee;
        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : class
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }
    }
}
