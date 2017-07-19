using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UklonBot.Models.Repositories.Exact;

namespace UklonBot.Models.Repositories.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<User> Users { get; }

        void Save();
    }
}