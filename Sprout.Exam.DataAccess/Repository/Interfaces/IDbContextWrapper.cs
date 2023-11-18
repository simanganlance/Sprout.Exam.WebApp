using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Sprout.Exam.DataAccess.Repository.Models;

namespace Sprout.Exam.DataAccess.Repository.Interfaces
{
    public interface IDbContextWrapper
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : class;

        IQueryable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class;

        Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;

        Task SaveAsync();
        DbSet<Employee> Employee { get; }
    }
}
