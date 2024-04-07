using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public T GetById(int id) => _context.Set<T>().Find(id);

    public IEnumerable<T> GetAll() => _context.Set<T>().ToList();

    public T Find(Expression<Func<T, bool>> match, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if(includes is not null)
        {
            foreach(var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.FirstOrDefault(match);
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> match, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.Where(match).ToList();
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> match, int take, int skip)
    {
        return _context.Set<T>().Where(match).Skip(skip).Take(take).ToList();
    }
}
