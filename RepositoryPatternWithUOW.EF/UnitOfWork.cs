using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.EF.Data;
using RepositoryPatternWithUOW.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IBaseRepository<Author> Authors { get; private set; }
    public IBookRepository Books { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Authors = new BaseRepository<Author>(_context);
        Books = new BookRepository(_context);
    }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
