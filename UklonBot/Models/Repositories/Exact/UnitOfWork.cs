using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UklonBot.Models.Repositories.Abstract;

namespace UklonBot.Models.Repositories.Exact
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private GenericRepository<User> _usersRepository;
        public void Dispose()
        {
            _context.Dispose();
        }

        public GenericRepository<User> Users => _usersRepository ?? (_usersRepository = new GenericRepository<User>(_context));

       

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}